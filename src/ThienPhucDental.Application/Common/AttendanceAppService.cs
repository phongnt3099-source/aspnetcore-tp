using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ThienPhucDental.Biometrics;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class AttendanceAppService : ThienPhucDentalAppServiceBase, IAttendanceAppService
    {
        private const double SimilarityThreshold = 0.52; // Ngưỡng Cosine Similarity cho EdgeFace-XS
        private readonly IFaceEmbeddingService _faceEmbeddingService;
        private readonly IRepository<UserToken, long> _userTokenRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly bool _enableDebugImages;
        private readonly IWebHostEnvironment _env;
        private readonly IAntiSpoofingService _antiSpoofingService;
        private readonly IFaceDetectorService _faceDetectorService;

        public AttendanceAppService(
            IFaceEmbeddingService faceEmbeddingService,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IRepository<UserToken, long> userTokenRepository,
            ICacheManager cacheManager,
            IClientInfoProvider clientInfoProvider,
            IAntiSpoofingService antiSpoofingService,
            IFaceDetectorService faceDetectorService)
        {
            _faceEmbeddingService = faceEmbeddingService;
            _userTokenRepository = userTokenRepository;
            _cacheManager = cacheManager;
            _clientInfoProvider = clientInfoProvider;
            _env = env;
            _enableDebugImages = configuration.GetValue<bool>("Biometrics:EnableDebugImages", false);
            _antiSpoofingService = antiSpoofingService;
            _faceDetectorService = faceDetectorService;
        }

        [AbpAllowAnonymous]
        public async Task<ValidateFaceLinkOutputDto> ValidateFaceRegistrationLinkAsync(string c, long? userId, string token)
        {
            long targetUserId = 0;
            string targetToken = token;
            DateTimeOffset? expireDate = null;

            if (!string.IsNullOrWhiteSpace(c))
            {
                try
                {
                    string decrypted = SimpleStringCipher.Instance.Decrypt(c);
                    var queryParams = decrypted.Split('&')
                        .Select(p => p.Split('='))
                        .Where(parts => parts.Length == 2)
                        .ToDictionary(parts => parts[0], parts => parts[1], StringComparer.OrdinalIgnoreCase);

                    if (queryParams.TryGetValue("userId", out var uIdStr) && long.TryParse(uIdStr, out var parsedUserId))
                        targetUserId = parsedUserId;

                    if (queryParams.TryGetValue("token", out var tStr))
                        targetToken = tStr;

                    if (queryParams.TryGetValue("expireDate", out var expStr))
                    {
                        string decodedDate = Uri.UnescapeDataString(expStr);
                        if (DateTimeOffset.TryParse(decodedDate, out var parsedExp))
                            expireDate = parsedExp;
                    }
                }
                catch
                {
                    return new ValidateFaceLinkOutputDto { IsValid = false, Message = "Mã liên kết xác thực không hợp lệ." };
                }
            }
            else if (userId.HasValue)
            {
                targetUserId = userId.Value;
            }

            if (targetUserId <= 0 || string.IsNullOrWhiteSpace(targetToken))
            {
                return new ValidateFaceLinkOutputDto { IsValid = false, Message = "Thông tin liên kết không đầy đủ." };
            }

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var user = await UserManager.GetUserByIdAsync(targetUserId);
                if (user == null || !user.IsActive)
                {
                    return new ValidateFaceLinkOutputDto { IsValid = false, Message = "Tài khoản nhân sự không tồn tại hoặc đã bị khóa." };
                }

                var userToken = await _userTokenRepository.FirstOrDefaultAsync(t =>
                    t.UserId == targetUserId &&
                    t.LoginProvider == "FaceRegistration" &&
                    t.Name == "MagicLinkToken" &&
                    t.Value == targetToken
                );

                if (userToken == null)
                {
                    return new ValidateFaceLinkOutputDto
                    {
                        IsValid = false,
                        Message = "Mã xác thực không hợp lệ, bị giả mạo hoặc đã qua sử dụng!"
                    };
                }

                if (userToken.ExpireDate.HasValue && Clock.Now > userToken.ExpireDate.Value)
                {
                    return new ValidateFaceLinkOutputDto
                    {
                        IsValid = false,
                        Message = "Liên kết đăng ký Face ID này đã hết hạn sử dụng. Vui lòng yêu cầu cấp link mới!"
                    };
                }

                return new ValidateFaceLinkOutputDto
                {
                    IsValid = true,
                    UserId = targetUserId,
                    UserName = user.UserName,
                    FullName = user.FullName
                };
            }
        }

        #region 1. Đăng Ký Khuôn Mặt (EdgeFace-XS Singleton)

        [AbpAllowAnonymous]
        [EnableRateLimiting("BiometricsPolicy")]
        public async Task<InsertResult> RegisterEmployeeFaceAsync(RegisterFaceInputDto input)
        {
            await CheckRateLimitAsync("RegisterEmployeeFace", limitSeconds: 5);

            long targetUserId = 0;
            string targetToken = input.Token;

            // 1. Giải mã QueryParams nếu đi từ Magic Link mã hóa
            if (!string.IsNullOrWhiteSpace(input.C))
            {
                try
                {
                    string decrypted = SimpleStringCipher.Instance.Decrypt(input.C);
                    var queryParams = decrypted.Split('&')
                        .Select(p => p.Split('='))
                        .Where(parts => parts.Length == 2)
                        .ToDictionary(parts => parts[0], parts => parts[1], StringComparer.OrdinalIgnoreCase);

                    if (queryParams.TryGetValue("userId", out var uIdStr) && long.TryParse(uIdStr, out var parsedUserId))
                        targetUserId = parsedUserId;

                    if (queryParams.TryGetValue("token", out var tStr))
                        targetToken = tStr;
                }
                catch
                {
                    throw new UserFriendlyException("Mã liên kết xác thực không hợp lệ.");
                }
            }
            else if (input.UserId.HasValue)
            {
                targetUserId = input.UserId.Value;
            }

            if (targetUserId <= 0 || string.IsNullOrWhiteSpace(targetToken))
            {
                throw new UserFriendlyException("Thông tin định danh liên kết không đầy đủ.");
            }

            if (string.IsNullOrWhiteSpace(input.ImageBase64))
            {
                throw new UserFriendlyException("Dữ liệu hình ảnh không hợp lệ.");
            }

            // 2. Decode Base64 DUY NHẤT 1 LẦN & Validate dung lượng tối đa 5MB
            byte[] rawImageBytes;
            try
            {
                string rawBase64 = input.ImageBase64.Contains(",")
                    ? input.ImageBase64.Split(',')[1]
                    : input.ImageBase64;

                rawImageBytes = Convert.FromBase64String(rawBase64);
            }
            catch (FormatException)
            {
                throw new UserFriendlyException("Định dạng dữ liệu hình ảnh Base64 không hợp lệ.");
            }

            if (rawImageBytes.Length > 5 * 1024 * 1024)
            {
                throw new UserFriendlyException("Dung lượng ảnh vượt quá giới hạn 5MB.");
            }

            // 3. Xác thực tài khoản & Magic Token (bỏ qua Tenant Filter để tìm đúng tài khoản)
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var user = await UserManager.GetUserByIdAsync(targetUserId);
                if (user == null || !user.IsActive)
                {
                    throw new UserFriendlyException("Tài khoản không tồn tại hoặc đã bị khóa.");
                }

                var userToken = await _userTokenRepository.FirstOrDefaultAsync(t =>
                    t.UserId == targetUserId &&
                    t.LoginProvider == "FaceRegistration" &&
                    t.Name == "MagicLinkToken" &&
                    t.Value == targetToken
                );

                if (userToken == null)
                {
                    throw new UserFriendlyException("Mã xác thực không hợp lệ hoặc đã qua sử dụng!");
                }

                if (userToken.ExpireDate.HasValue && Clock.Now > userToken.ExpireDate.Value)
                {
                    throw new UserFriendlyException("Liên kết đăng ký Face ID này đã hết hạn sử dụng.");
                }

                // Thiết lập Tenant Context chuẩn cho User này
                using (CurrentUnitOfWork.SetTenantId(user.TenantId))
                {
                    string createdFilePath = null; // Theo dõi đường dẫn file vật lý để dọn dẹp khi lỗi

                    try
                    {
                        // 4. BE Authoritative: Detect khuôn mặt, kiểm tra 1 mặt, size, faceRatio và Affine Align 5 landmarks
                        var detectResult = _faceDetectorService.DetectAndAlign(rawImageBytes);
                        if (!detectResult.IsValid)
                        {
                            Logger.Warn($"[REGISTER-FACE] Khuôn mặt không hợp lệ cho User [{user.UserName}]: {detectResult.ErrorMessage}");
                            throw new UserFriendlyException(detectResult.ErrorMessage);
                        }

                        // 5. Anti-Spoofing: Kiểm tra Liveness bằng ảnh crop 2.7x (MiniFASNetV2)
                        bool isReal = _antiSpoofingService.IsRealFace(detectResult.CroppedFace80Bytes, out float realScore);
                        Logger.Warn($"[REGISTER-FACE] User [{user.UserName}] Anti-Spoofing Score: {realScore:F4} | IsReal: {isReal}");

                        if (!isReal)
                        {
                            throw new UserFriendlyException("Hệ thống phát hiện hình ảnh không được chụp trực tiếp hoặc có dấu hiệu giả mạo màn hình/ảnh in.");
                        }

                        // 6. Trích xuất vector EdgeFace-XS từ ảnh ALIGNED 112x112
                        float[] embeddingArray = _faceEmbeddingService.ExtractEmbedding(detectResult.AlignedFace112Tensor);
                        byte[] vectorBytes = FloatArrayToByte(embeddingArray);

                        // 7. Lưu ẢNH GỐC (rawImageBytes) vào ổ đĩa vật lý theo Tenant Folder
                        int tenantFolderId = user.TenantId ?? 0;
                        string webRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
                        string uploadFolder = Path.Combine(webRoot, "uploads", "faces", tenantFolderId.ToString());

                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string fileName = $"{user.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}.jpg";
                        createdFilePath = Path.Combine(uploadFolder, fileName);

                        // Lưu ảnh gốc nguyên bản chất lượng cao
                        await File.WriteAllBytesAsync(createdFilePath, rawImageBytes);
                        string relativeImageUrl = $"/uploads/faces/{tenantFolderId}/{fileName}";

                        int tenantId = AbpSession.TenantId ?? 0;
                        // 8. Thực thi lưu dữ liệu vào DB thông qua Stored Procedure
                        var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(
                            CommonStoreProcedureConsts.CM_EMPLOYEE_FACE_INS_OR_UPD,
                            new
                            {

                                P_TENANT_ID = (object)tenantId ?? DBNull.Value,
                                P_USER_ID = user.Id,
                                P_FACE_VECTOR = vectorBytes,
                                P_FACE_IMAGE_URL = relativeImageUrl,
                                P_USER_NAME = user.UserName
                            })).FirstOrDefault();

                        // 10. Xóa token chỉ khi mọi thao tác đã thành công mỹ mãn
                        await _userTokenRepository.DeleteAsync(userToken);

                        Logger.Info($"[REGISTER-FACE] Đăng ký khuôn mặt thành công cho User [{user.UserName}] - File: {relativeImageUrl}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        // Rollback ngay lập tức file ảnh vừa lưu nếu có bất kỳ lỗi nào xảy ra
                        RollbackCreatedFile(createdFilePath);

                        Logger.Error($"[REGISTER-FACE] Lỗi xử lý đăng ký khuôn mặt: {ex.Message}", ex);

                        if (ex is UserFriendlyException)
                            throw;

                        throw new UserFriendlyException("Đã xảy ra lỗi trong quá trình xử lý hình ảnh và đăng ký. Vui lòng thử lại!");
                    }
                }
            }
        }

        #endregion

        #region 2. Điểm Danh Thực Tế (Backend tự tính Vector và đối chiếu)
        [AbpAuthorize]
        [EnableRateLimiting("BiometricsPolicy")]
        public async Task<AttendanceResultDto> ProcessFaceAttendanceAsync(AttendanceInputDto input)
        {
            // 1. Kiểm tra tần suất gọi API
            await CheckRateLimitAsync("ProcessFaceAttendance", limitSeconds: 3);

            Logger.Warn("=== [ATTENDANCE] BẮT ĐẦU XỬ LÝ ĐIỂM DANH ===");

            if (string.IsNullOrWhiteSpace(input.Base64Image))
            {
                throw new UserFriendlyException("Dữ liệu hình ảnh không hợp lệ.");
            }

            var currentUser = await GetCurrentUserAsync();
            int tenantId = AbpSession.TenantId ?? 0;

            // 2. GIẢI MÃ BASE64 DUY NHẤT 1 LẦN TẠI ĐÂY
            byte[] rawImageBytes;
            try
            {
                string rawBase64 = input.Base64Image.Contains(",")
                    ? input.Base64Image.Split(',')[1]
                    : input.Base64Image;

                rawImageBytes = Convert.FromBase64String(rawBase64);
            }
            catch (FormatException ex)
            {
                Logger.Error($"[ATTENDANCE] Dữ liệu Base64 bị hỏng hoặc giải mã sai định dạng: {ex.Message}", ex);
                throw new UserFriendlyException("Dữ liệu hình ảnh gửi lên không đúng định dạng Base64.");
            }

            // 3. BE Authoritative: Detect & Align chỉ nhận mảng byte[] thô (không decode lại)
            var detectResult = _faceDetectorService.DetectAndAlign(rawImageBytes);
            if (!detectResult.IsValid)
            {
                Logger.Warn($"[ATTENDANCE] Kiểm tra vị trí mặt không hợp lệ [{currentUser.UserName}]: {detectResult.ErrorMessage}");
                return new AttendanceResultDto
                {
                    Result = "-1",
                    Message = detectResult.ErrorMessage,
                    ConfidenceScore = 0
                };
            }

            // 4. Anti-Spoofing nhận trực tiếp CroppedFace80Bytes (đã là byte[] của ảnh crop 2.7x)
            bool isReal = _antiSpoofingService.IsRealFace(detectResult.CroppedFace80Bytes, out float realScore);
            Logger.Warn($"[ATTENDANCE] Anti-Spoofing Score: {realScore:F4} | IsReal: {isReal}");

            if (!isReal)
            {
                Logger.Warn($"[ATTENDANCE] Cảnh báo giả mạo từ tài khoản [{currentUser.UserName}]");
                return new AttendanceResultDto
                {
                    Result = "-1",
                    Message = "Hệ thống phát hiện hình ảnh không chụp trực tiếp hoặc có dấu hiệu giả mạo.",
                    ConfidenceScore = 0
                };
            }

            // 5. Trích xuất vector AI nhận trực tiếp mảng tensor float[] (đã căn chỉnh và chuẩn hóa ImageNet)
            float[] targetEmbedding;
            try
            {
                targetEmbedding = _faceEmbeddingService.ExtractEmbedding(detectResult.AlignedFace112Tensor);
            }
            catch (Exception ex)
            {
                Logger.Error($"[ATTENDANCE] Lỗi trích xuất vector EdgeFace-XS: {ex.Message}", ex);
                throw new UserFriendlyException($"Lỗi trích xuất đặc trưng khuôn mặt: {ex.Message}");
            }

            // 6. Lấy dữ liệu mẫu của User theo Tenant
            var registeredFaces = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_EMPLOYEE_FACE_ENTITY>(
                CommonStoreProcedureConsts.CM_EMPLOYEE_FACE_BYID,
                new
                {
                    P_TENANT_ID = (object)tenantId ?? DBNull.Value,
                    P_USER_NAME = currentUser.UserName
                })).ToList();

            if (!registeredFaces.Any())
            {
                return new AttendanceResultDto
                {
                    Result = "-1",
                    Message = "Tài khoản của bạn chưa được đăng ký dữ liệu Face ID trên hệ thống.",
                    ConfidenceScore = 0
                };
            }

            string bestMatchEmpId = null;
            double maxSimilarity = -1;

            // 7. Đối chiếu Cosine Similarity
            foreach (var face in registeredFaces)
            {
                if (face.FACE_VECTOR == null || face.FACE_VECTOR.Length == 0)
                    continue;

                float[] dbEmbedding = ByteToFloatArray(face.FACE_VECTOR);
                double similarity = CalculateCosineSimilarity(targetEmbedding, dbEmbedding);

                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    bestMatchEmpId = face.EMP_ID;
                }
            }

            double confidencePercent = Math.Round(Math.Max(0, maxSimilarity) * 100, 2);
            bool isSuccess = !string.IsNullOrEmpty(bestMatchEmpId) && maxSimilarity >= SimilarityThreshold;

            if (!isSuccess)
            {
                return new AttendanceResultDto
                {
                    Result = "-1",
                    Message = "Khuôn mặt không trùng khớp với hồ sơ đã đăng ký hoặc góc chụp không đủ rõ.",
                    ConfidenceScore = confidencePercent
                };
            }

            string clientIp = _clientInfoProvider?.ClientIpAddress ?? input.DeviceIp ?? "127.0.0.1";
            string checkType = string.IsNullOrWhiteSpace(input.CheckType) ? "IN" : input.CheckType.ToUpper();

            // 8. Ghi nhận dữ liệu vào cơ sở dữ liệu qua Stored Procedure
            var spResult = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(
                CommonStoreProcedureConsts.AT_PROCESS_ATTENDANCE,
                new
                {
                    P_TENANT_ID = (object)tenantId ?? DBNull.Value,
                    P_EMP_ID = bestMatchEmpId,
                    P_CHECK_TYPE = checkType,
                    P_DEVICE_IP = clientIp,
                    P_CONFIDENCE_SCORE = confidencePercent,
                    P_NOTES = (string)null
                })).FirstOrDefault();

            if (spResult == null || spResult.Result != "1")
            {
                return new AttendanceResultDto
                {
                    Result = "-1",
                    Message = "Không thể lưu lượt điểm danh vào cơ sở dữ liệu.",
                    ConfidenceScore = confidencePercent
                };
            }

            // 9. Cập nhật Cache trong ngày
            var todayKey = $"{tenantId}_{Clock.Now:yyyyMMdd}_{currentUser.UserName}";
            var cache = _cacheManager.GetCache("TodayAttendanceCache").AsTyped<string, AttendanceStatusCacheItem>();

            var cachedStatus = await cache.GetOrDefaultAsync(todayKey) ?? new AttendanceStatusCacheItem
            {
                IsAttendanceRequired = true,
                FullName = currentUser.FullName
            };

            var now = Clock.Now;
            if (checkType == "IN")
            {
                cachedStatus.HasCheckedInToday = true;
                cachedStatus.CheckInTime = now;
            }
            else if (checkType == "OUT")
            {
                cachedStatus.HasCheckedOutToday = true;
                cachedStatus.CheckOutTime = now;
            }

            var endOfDay = now.Date.AddDays(1).AddTicks(-1);
            await cache.SetAsync(todayKey, cachedStatus, endOfDay - now);

            return new AttendanceResultDto
            {
                Result = "1",
                USER_NAME = currentUser.UserName,
                Message = checkType == "IN" ? "Điểm danh vào ca thành công." : "Điểm danh kết thúc ca thành công.",
                ConfidenceScore = confidencePercent
            };
        }

        #endregion

        #region 3. Tiền Xử Lý Ảnh & Đo Lường

        private double CalculateCosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA == null || vectorB == null || vectorA.Length != vectorB.Length)
                return 0;

            double dotProduct = 0.0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
            }

            return dotProduct;
        }

        private float[] ByteToFloatArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return Array.Empty<float>();

            var floats = new float[bytes.Length / sizeof(float)];
            Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
            return floats;
        }

        private byte[] FloatArrayToByte(float[] floatArray)
        {
            if (floatArray == null) return null;
            byte[] byteArray = new byte[floatArray.Length * sizeof(float)];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

       
        #endregion

        #region Status Checks

        [AbpAuthorize]
        public async Task<AttendanceCheckDto> GetTodayAttendanceStatusAsync()
        {
            var currentUserName = AbpSession.UserId.HasValue ? (await GetCurrentUserAsync()).UserName : null;
            if (string.IsNullOrEmpty(currentUserName))
            {
                return new AttendanceCheckDto { IsAttendanceRequired = false };
            }

            int tenantId = AbpSession.TenantId ?? 0;
            var todayKey = $"{tenantId}_{Clock.Now:yyyyMMdd}_{currentUserName}";
            var cache = _cacheManager.GetCache("TodayAttendanceCache").AsTyped<string, AttendanceStatusCacheItem>();

            // 1. Kiểm tra Cache
            var cached = await cache.GetOrDefaultAsync(todayKey);
            if (cached != null)
            {
                return new AttendanceCheckDto
                {
                    IsAttendanceRequired = cached.IsAttendanceRequired,
                    HasCheckedInToday = cached.HasCheckedInToday,
                    CheckInTime = cached.CheckInTime,
                    HasCheckedOutToday = cached.HasCheckedOutToday,
                    CheckOutTime = cached.CheckOutTime,
                    FullName = cached.FullName
                };
            }

            int? currentTenantId = AbpSession.TenantId;
            // 2. Cache Miss -> Query Stored Procedure
            var records = (await storeProcedureProvider.GetDataFromStoredProcedure<AttendanceRawStatusDto>(
                CommonStoreProcedureConsts.AT_CHECK_TODAY_ATTENDANCE_STATUS,
                new
                {
                    P_TENANT_ID = currentTenantId,
                    P_USER_NAME = currentUserName,
                    P_DATE = Clock.Now.Date
                }
            )).ToList();

            var empInfo = records.FirstOrDefault();
            if (empInfo == null || !empInfo.IS_ATTENDANCE_REQUIRED)
            {
                return new AttendanceCheckDto { IsAttendanceRequired = false };
            }

            var checkInLog = records.FirstOrDefault(x => string.Equals(x.CHECK_TYPE, "IN", StringComparison.OrdinalIgnoreCase));
            var checkOutLog = records.FirstOrDefault(x => string.Equals(x.CHECK_TYPE, "OUT", StringComparison.OrdinalIgnoreCase));

            bool hasCheckedIn = checkInLog != null && checkInLog.CHECK_TIME.HasValue;
            bool hasCheckedOut = checkOutLog != null && checkOutLog.CHECK_TIME.HasValue;

            var statusItem = new AttendanceStatusCacheItem
            {
                IsAttendanceRequired = true,
                FullName = empInfo.EMP_NAME,
                HasCheckedInToday = hasCheckedIn,
                CheckInTime = checkInLog?.CHECK_TIME,
                HasCheckedOutToday = hasCheckedOut,
                CheckOutTime = checkOutLog?.CHECK_TIME
            };

            if (hasCheckedIn)
            {
                var endOfDay = Clock.Now.Date.AddDays(1).AddTicks(-1);
                var remainingTime = endOfDay - Clock.Now;
                await cache.SetAsync(todayKey, statusItem, remainingTime);
            }

            return new AttendanceCheckDto
            {
                IsAttendanceRequired = true,
                HasCheckedInToday = hasCheckedIn,
                FullName = empInfo.EMP_NAME,
                CheckInTime = checkInLog?.CHECK_TIME,
                HasCheckedOutToday = hasCheckedOut,
                CheckOutTime = checkOutLog?.CHECK_TIME
            };
        }

        #endregion

        private async Task CheckRateLimitAsync(string actionKey, int limitSeconds = 3)
        {
            int tenantId = AbpSession.TenantId ?? 0;
            string clientIp = _clientInfoProvider?.ClientIpAddress ?? "UnknownIP";
            string identifier = AbpSession.UserId.HasValue
                ? $"User_{AbpSession.UserId.Value}"
                : $"IP_{clientIp}";

            string cacheKey = $"RateLimit_{tenantId}_{actionKey}_{identifier}";
            var cache = _cacheManager.GetCache("BiometricsRateLimitCache").AsTyped<string, DateTime>();

            var lastRequestTime = await cache.GetOrDefaultAsync(cacheKey);
            var now = Clock.Now;

            if (lastRequestTime != default && (now - lastRequestTime).TotalSeconds < limitSeconds)
            {
                double waitSec = Math.Ceiling(limitSeconds - (now - lastRequestTime).TotalSeconds);
                throw new UserFriendlyException($"Thao tác quá nhanh. Vui lòng thử lại sau {waitSec} giây.");
            }

            await cache.SetAsync(cacheKey, now, TimeSpan.FromSeconds(limitSeconds + 2));
        }

        // Hàm hỗ trợ rollback tệp vật lý an toàn
        private void RollbackCreatedFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Logger.Warn($"[ROLLBACK] Đã xóa tệp ảnh mồ côi do thao tác thất bại: {filePath}");
                }
            }
            catch (Exception ex)
            {
                // Ghi log để can thiệp thủ công nếu file bị lock bởi process khác
                Logger.Error($"[ROLLBACK-FAILED] Không thể xóa tệp ảnh: {filePath}. Lỗi: {ex.Message}", ex);
            }
        }
    }
}