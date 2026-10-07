using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThienPhucDental.Configuration;
using ThienPhucDental.Consts;
using ThienPhucDental.Helper;
using ThienPhucDental.Procedures.Attributes;
using static Dapper.SqlMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using Abp.Runtime.Caching;

namespace ThienPhucDental.ProcedureHelpers
{
    public class ArgumentExceptionEx : ArgumentException
    {
        public int ErrorCode { get; }
        public string PropertyName { get; }

        public ArgumentExceptionEx(string paramName, int errorCode, string propertyName)
            : base(paramName)
        {
            ErrorCode = errorCode;
            PropertyName = propertyName;
        }
    }

    public class StoreProcedureProvider : IStoreProcedureProvider, ITransientDependency
    {
        private readonly IClientConnection _clientConnection;
        private readonly IDetailLoggerHelper _detailLoggerHelper;
        private readonly SemaphoreSlim _connStrLock = new(1, 1);
        private readonly int commandTimeout;
        private string _connectionString;
        private static readonly TimeSpan ParamsCacheDuration = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan DefinitionCacheDuration = TimeSpan.FromHours(6);
        private readonly ICacheManager _cacheManager;

        private const string ParamsCacheName = "SP_Params_Cache";
        private const string DefinitionCacheName = "SP_Definition_Cache";


        public StoreProcedureProvider(
            IWebHostEnvironment ev,
            IClientConnection clientConnection,
            IDetailLoggerHelper detailLoggerHelper,
            IMemoryCache memoryCache,
            ICacheManager cacheManager)
        {
            _detailLoggerHelper = detailLoggerHelper;
            _clientConnection = clientConnection;
            _cacheManager = cacheManager;


            try
            {
                var timeout = ev.GetAppConfiguration().GetValue<int>("App:SqlServerCommandTimeout");
                commandTimeout = timeout > 0 ? timeout : 30;
            }
            catch (Exception ex)
            {
                detailLoggerHelper.Logger(
                $"[StoreProcedureProvider] Không đọc được App:SqlServerCommandTimeout. " +
                $"Dùng default 30 giây. Lỗi: {ex.Message}");
                commandTimeout = 30;
            }

        }

        private async Task<string> GetConnectionStringAsync()
        {
            if (_connectionString != null)
                return _connectionString;

            await _connStrLock.WaitAsync();
            try
            {
                if (_connectionString == null)
                {
                    var cs = await _clientConnection.GetConnectionString();
                    if (string.IsNullOrWhiteSpace(cs))
                        throw new UserFriendlyException("Connection string rỗng hoặc null.");
                    _connectionString = cs;
                }
            }
            finally
            {
                _connStrLock.Release();
            }

            return _connectionString;
        }

        public class NullableDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
        {
            public override void SetValue(IDbDataParameter parameter, DateTime? value)
            {
                parameter.Value = value.HasValue ? value.Value : (object)DBNull.Value;
            }

            public override DateTime? Parse(object value)
            {
                if (value == null || value is DBNull) return null;
                if (value is DateTime dt) return dt;
                return null;
            }
        }

        //public async Task<List<TModel>> GetDataFromStoredProcedure<TModel>(string storedProcName, object parameters) where TModel : class
        //{   
        //    var parameterInfos = await GetParameterInfos(storedProcName);
        //    var dapperParams = new DynamicParameters();
        //    var outputPropertyTable = new Dictionary<string, PropertyInfo>();

        //    if (parameters != null)
        //    {
        //        var properties = parameters.GetType().GetProperties().Where(x => x != null);

        //        List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();

        //        foreach (var property in properties)
        //        {
        //            var paramName = GetParameterName(property);

        //            var parameterInfo = GetParameterInfo(parameterInfos, paramName);

        //            procedureInfoInProperties.Add(parameterInfo);

        //            if (parameterInfo == null)
        //            {
        //                continue;
        //            }

        //            var direction = GetParameterDirection(parameterInfo);

        //            if (direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
        //            {
        //                outputPropertyTable.Add(parameterInfo.PARAMETER_NAME, property);
        //            }

        //            var parameterValue = GetParameterValue(property, parameters);

        //            dapperParams.Add(parameterInfo.PARAMETER_NAME, parameterValue, null, direction);
        //        }


        //        // add property not include in class parameters
        //        //foreach (var parameterInfo in parameterInfos.Where(x => x!=null && !procedureInfoInProperties.Any(pi => pi != null &&  x.PARAMETER_NAME.ToLower().Replace("@", "").Replace("p_", "") == pi.PARAMETER_NAME.ToLower().Replace("@", "").Replace("p_", ""))))
        //        //{
        //        //    dapperParams.Add(parameterInfo.PARAMETER_NAME);
        //        //}

        //        var names = dapperParams.ParameterNames.ToList();
        //        foreach (var parameterInfo in parameterInfos)
        //        {
        //            if (!names.Any(x => "@" + x == parameterInfo.PARAMETER_NAME))
        //            {
        //                dapperParams.Add(parameterInfo.PARAMETER_NAME, null, null, GetParameterDirection(parameterInfo));
        //            }
        //        }

        //    }
        //    try
        //    {
        //        foreach (var item in dapperParams.ParameterNames)
        //        {
        //            var tmp = item;
        //            var value = dapperParams.Get<object>(tmp);
        //        }
        //        using (var conn = new SqlConnection(ConnectionString))
        //        {
        //            //          var rr = await conn.QueryAsync<TModel>(storedProcName, dapperParams, null, null, System.Data.CommandType.StoredProcedure);
        //            var rr = (List<TModel>)conn.Query<TModel>(storedProcName, dapperParams, null, true, commandTimeout, System.Data.CommandType.StoredProcedure);
        //            foreach (var pair in outputPropertyTable)
        //            {
        //                pair.Value.SetValue(parameters, dapperParams.Get<object>(pair.Key));
        //            }
        //            return rr;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new UserFriendlyException(e.Message);
        //    }

        //}
        public async Task<List<TModel>> GetDataFromStoredProcedure<TModel>(
        string storedProcName, object parameters) where TModel : class
        {
            var parameterInfos = await GetParameterInfosCachedAsync(storedProcName); 
            var dapperParams = new DynamicParameters();
            var outputPropertyTable = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            if (parameters != null)
            {
                var properties = parameters.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var paramName = GetParameterName(property);
                    var parameterInfo = GetParameterInfo(parameterInfos, paramName);
                    if (parameterInfo == null) continue;

                    var direction = GetParameterDirection(parameterInfo);

                    
                    var bareName = parameterInfo.PARAMETER_NAME.TrimStart('@');

                    if (direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        outputPropertyTable[bareName] = property;  
                    }

                   
                    var parameterValue = direction == ParameterDirection.Output
                        ? null
                        : GetParameterValue(property, parameters);

                    dapperParams.Add(bareName, parameterValue, null, direction);
                }

                
                var names = new HashSet<string>(dapperParams.ParameterNames, StringComparer.OrdinalIgnoreCase);
                foreach (var parameterInfo in parameterInfos)
                {
                    var bareName = parameterInfo.PARAMETER_NAME.TrimStart('@');
                    if (names.Contains(bareName)) continue;

                    dapperParams.Add(bareName, null, null, GetParameterDirection(parameterInfo));
                }
            }

            try
            {
                var connStr = await GetConnectionStringAsync();
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    var queryResult = await conn.QueryAsync<TModel>(
                        storedProcName, dapperParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: commandTimeout);

                    var rr = queryResult.ToList();

                    
                    foreach (var pair in outputPropertyTable)
                    {
                        var val = dapperParams.Get<object>(pair.Key);
                        if (val is DBNull) val = null;
                        pair.Value.SetValue(parameters, val);
                    }

                    return rr;
                }
            }
            catch (UserFriendlyException) { throw; }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message, e);  
            }
        }

        public async Task<List<dynamic>> GetMultiResultValueFromStore(string storedProcName, object parameters)
        {
            var list = await GetDataFromStoredProcedure<dynamic>(storedProcName, parameters);
            return list;
        }

        //public async Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class
        //{
        //    try
        //    {
        //        var parameterInfos = await GetParameterInfos(storedProcName);
        //        if (parameters != null)
        //        {
        //            var properties = parameters.GetType().GetProperties().Where(x => x != null);
        //            int maxResultCount = (int?)properties.Where(x => x.Name == "MaxResultCount").FirstOrDefault()?.GetValue(parameters) ?? 0;

        //            if (maxResultCount == -1)
        //            {
        //                Stopwatch st = new Stopwatch();

        //                st.Start();

        //                var items = await GetDataFromStoredProcedure<TModel>(storedProcName, parameters);

        //                st.Stop();

        //                return new PagedResultDto<TModel>()
        //                {
        //                    Items = items,
        //                    TotalCount = items.Count
        //                };
        //            }

        //            int totalCount = 0, skipCount = 0;
        //            string sorting = "";

        //            var totalCountProperty = properties.Where(x => x.Name == "TotalCount").FirstOrDefault();

        //            totalCount = (int?)totalCountProperty?.GetValue(parameters) ?? 0;
        //            skipCount = (int?)properties.Where(x => x.Name == "SkipCount").FirstOrDefault()?.GetValue(parameters) ?? 0;
        //            sorting = (string)properties.Where(x => x.Name == "Sorting").FirstOrDefault()?.GetValue(parameters) ?? "";

        //            string sortingInParam = sorting;

        //            List<ReplaceStringResult> stringReplacers = new List<ReplaceStringResult>();

        //            using (var conn = new SqlConnection(ConnectionString))
        //            {
        //                var procedureContent = (string)((IDictionary<string, object>)conn.Query("SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + storedProcName + "')) as CONTENT", null, null, true, commandTimeout, System.Data.CommandType.Text).First())["CONTENT"];

        //                procedureContent = ExtractFromString(procedureContent, "BEGIN -- PAGING", "END -- PAGING").First().Text;

        //                foreach (var text in ExtractFromString(procedureContent, "-- PAGING BEGIN", "-- PAGING END"))
        //                {
        //                    var stringReplacer = new ReplaceStringResult();
        //                    stringReplacer.IndexBegin = text.IndexBegin;
        //                    stringReplacer.IndexEnd = text.IndexEnd;

        //                    var orderBy = ExtractFromString(text.Text, "ORDER BY", "\n").Where(x => x.Text.IndexOf(")") == -1).FirstOrDefault();

        //                    if (orderBy != null)
        //                    {
        //                        if (sorting.IsNullOrWhiteSpace())
        //                        {
        //                            sorting = orderBy.Text;
        //                        }
        //                    }

        //                    if (sorting.IsNullOrWhiteSpace())
        //                    {
        //                        sorting = "(SELECT(1))";
        //                    }

        //                    if (orderBy != null)
        //                    {

        //                        var beginIndex = text.Text.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
        //                        var endIndex = text.Text.IndexOf("top", beginIndex, StringComparison.CurrentCultureIgnoreCase);
        //                        if (endIndex == -1 || text.Text.Substring(beginIndex + 6, endIndex - beginIndex - 6).Trim().Length != 0)
        //                        {
        //                            text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + text.Text.Substring(orderBy.IndexEnd);
        //                        }
        //                        else
        //                        {
        //                            text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + "ORDER BY " + sorting + text.Text.Substring(orderBy.IndexEnd);
        //                        }
        //                    }

        //                    int index = text.Text.IndexOf("-- SELECT END");

        //                    var textBetweenTop = ExtractFromString(text.Text.Substring(0, index).ToUpper(), "TOP", ")").FirstOrDefault();

        //                    if (totalCount == 0)
        //                    {
        //                        if (textBetweenTop == null)
        //                        {
        //                            stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) " + text.Text.Substring(index);
        //                        }
        //                        else
        //                        {
        //                            stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) FROM(" + text.Text + ") COUNTER_TOP";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        stringReplacer.Text = "\r\nBEGIN" + stringReplacer.Text;
        //                    }
        //                    //stringReplacer.Text = skipCount == 0 ? $"\r\nSELECT COUNT(*) AS [COUNTER] FROM ({text.Text}) a\r\n" : "";




        //                    if (!string.IsNullOrWhiteSpace(sortingInParam))
        //                    {
        //                        text.Text = "SELECT A.*, ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM FROM (" + text.Text + " ) A";
        //                    }
        //                    else
        //                    {
        //                        text.Text = text.Text.Insert(index, ", ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM");
        //                    }

        //                    //text.Text.Insert(index, "\r\n, ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM\r\n");

        //                    text.Text = ";WITH QUERY_DATA AS ( " + text.Text +
        //                        ") SELECT * FROM QUERY_DATA WHERE __ROWNUM > " + skipCount + " AND __ROWNUM <= " + (skipCount + maxResultCount) + "\r\nEND";

        //                    stringReplacer.Text += text.Text;
        //                    stringReplacers.Add(stringReplacer);
        //                }


        //                stringReplacers.Reverse();

        //                foreach (var item in stringReplacers)
        //                {
        //                    procedureContent = procedureContent.Substring(0, item.IndexBegin) + item.Text + procedureContent.Substring(item.IndexEnd);
        //                }

        //                var declareParam = "DECLARE " + string.Join(",\r\n", parameterInfos.Select(x =>
        //                {

        //                    object parameterValue = null;
        //                    var property = properties.Where(p => CompareName(p.Name, x.PARAMETER_NAME)).FirstOrDefault();

        //                    if (property != null)
        //                    {
        //                        parameterValue = GetParameterValue(property, parameters);

        //                    }
        //                    return GetValuePaging(x, parameterValue);
        //                }));

        //                procedureContent = declareParam + "\r\n" + procedureContent;

        //                var result = conn.QueryMultiple("-- PROCEDURE NAME: " + storedProcName + "\r\n\r\n" + procedureContent, null, null, commandTimeout);

        //                if (totalCount == 0)
        //                {
        //                    totalCount = result.Read<int>().FirstOrDefault();
        //                }

        //                return new PagedResultDto<TModel>()
        //                {
        //                    Items = result.Read<TModel>().ToList(),
        //                    TotalCount = totalCount
        //                };
        //            }
        //        }

        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new UserFriendlyException(e.Message);
        //    }
        //}

        // 1. Cache cho Parameter Infos (Chỉ cache khi list khác null và có phần tử)
        private async Task<List<StoreParameterInfoDto>> GetParameterInfosCachedAsync(string storedProcName)
        {
            string cacheKey = storedProcName.ToLowerInvariant();
            var cache = _cacheManager.GetCache<string, List<StoreParameterInfoDto>>(ParamsCacheName);

            var cachedParams = await cache.GetOrDefaultAsync(cacheKey);
            if (cachedParams != null && cachedParams.Count > 0)
            {
                return cachedParams;
            }

            var parameterInfos = await GetParameterInfos(storedProcName);

            if (parameterInfos != null && parameterInfos.Count > 0)
            {
                await cache.SetAsync(cacheKey, parameterInfos, ParamsCacheDuration);
            }

            return parameterInfos;
        }

        // 2. Cache cho OBJECT_DEFINITION (Chỉ cache khi nội dung khác null/whitespace)
        private async Task<string> GetProcedureContentCachedAsync(SqlConnection conn, string storedProcName)
        {
            string cacheKey = storedProcName.ToLowerInvariant();
            var cache = _cacheManager.GetCache<string, string>(DefinitionCacheName);

            var cachedContent = await cache.GetOrDefaultAsync(cacheKey);
            if (!string.IsNullOrWhiteSpace(cachedContent))
            {
                return cachedContent;
            }

            var procedureContent = await conn.QuerySingleOrDefaultAsync<string>(
                "SELECT OBJECT_DEFINITION(OBJECT_ID(@name))",
                new { name = storedProcName },
                commandTimeout: commandTimeout);

            if (!string.IsNullOrWhiteSpace(procedureContent))
            {
                await cache.SetAsync(cacheKey, procedureContent, DefinitionCacheDuration);
            }

            return procedureContent;
        }
        public async Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class
        {
            try
            {
                // Sử dụng phiên bản cache cho Parameter Infos
                var parameterInfos = await GetParameterInfosCachedAsync(storedProcName);
                if (parameters == null)
                {
                    return null;
                }

                var properties = parameters.GetType().GetProperties().Where(x => x != null).ToList();

                int maxResultCount = (int?)properties.FirstOrDefault(x => x.Name == "MaxResultCount")?.GetValue(parameters) ?? 0;

                if (maxResultCount == -1)
                {
                    var items = await GetDataFromStoredProcedure<TModel>(storedProcName, parameters);
                    return new PagedResultDto<TModel>()
                    {
                        Items = items,
                        TotalCount = items.Count
                    };
                }

                int totalCount = (int?)properties.FirstOrDefault(x => x.Name == "TotalCount")?.GetValue(parameters) ?? 0;
                int skipCount = (int?)properties.FirstOrDefault(x => x.Name == "SkipCount")?.GetValue(parameters) ?? 0;
                string sorting = (string)properties.FirstOrDefault(x => x.Name == "Sorting")?.GetValue(parameters) ?? "";

                string sortingInParam = sorting;
                var stringReplacers = new List<ReplaceStringResult>();

                var connStr = await GetConnectionStringAsync();
                using(var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Sử dụng phiên bản cache cho OBJECT_DEFINITION của Stored Procedure
                    var procedureContent = await GetProcedureContentCachedAsync(conn, storedProcName);

                    if (string.IsNullOrWhiteSpace(procedureContent))
                    {
                        throw new UserFriendlyException($"Không tìm thấy nội dung của Stored Procedure '{storedProcName}'.");
                    }

                    var pagingBlock = ExtractFromString(procedureContent, "BEGIN -- PAGING", "END -- PAGING").FirstOrDefault();
                    if (pagingBlock == null)
                    {
                        throw new UserFriendlyException($"Stored procedure '{storedProcName}' thiếu block -- PAGING.");
                    }
                    procedureContent = pagingBlock.Text;

                    foreach (var text in ExtractFromString(procedureContent, "-- PAGING BEGIN", "-- PAGING END"))
                    {
                        var stringReplacer = new ReplaceStringResult
                        {
                            IndexBegin = text.IndexBegin,
                            IndexEnd = text.IndexEnd
                        };

                        var orderBy = ExtractFromString(text.Text, "ORDER BY", "\n").FirstOrDefault(x => x.Text.IndexOf(")") == -1);

                        if (orderBy != null && sorting.IsNullOrWhiteSpace())
                        {
                            sorting = orderBy.Text;
                        }

                        if (sorting.IsNullOrWhiteSpace())
                        {
                            sorting = "(SELECT(1))";
                        }

                        if (orderBy != null)
                        {
                            var beginIndex = text.Text.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
                            var endIndex = text.Text.IndexOf("top", beginIndex, StringComparison.CurrentCultureIgnoreCase);

                            int cutPos = Math.Max(0, orderBy.IndexBegin - 8);

                            if (endIndex == -1 || text.Text.Substring(beginIndex + 6, endIndex - beginIndex - 6).Trim().Length != 0)
                            {
                                text.Text = text.Text.Substring(0, cutPos) + text.Text.Substring(orderBy.IndexEnd);
                            }
                            else
                            {
                                text.Text = text.Text.Substring(0, cutPos) + "ORDER BY " + sorting + text.Text.Substring(orderBy.IndexEnd);
                            }
                        }

                        int index = text.Text.IndexOf("-- SELECT END");
                        if (index == -1) continue;

                        var textBetweenTop = ExtractFromString(text.Text.Substring(0, index).ToUpper(), "TOP", ")").FirstOrDefault();

                        if (totalCount == 0)
                        {
                            if (textBetweenTop == null)
                            {
                                stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) " + text.Text.Substring(index);
                            }
                            else
                            {
                                stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) FROM(" + text.Text + ") COUNTER_TOP";
                            }
                        }
                        else
                        {
                            stringReplacer.Text = "\r\nBEGIN" + stringReplacer.Text;
                        }

                        if (!string.IsNullOrWhiteSpace(sortingInParam))
                        {
                            text.Text = "SELECT A.*, ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM FROM (" + text.Text + " ) A";
                        }
                        else
                        {
                            text.Text = text.Text.Insert(index, ", ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM");
                        }

                        text.Text = ";WITH QUERY_DATA AS ( " + text.Text +
                            ") SELECT * FROM QUERY_DATA WHERE __ROWNUM > " + skipCount + " AND __ROWNUM <= " + (skipCount + maxResultCount) + "\r\nEND";

                        stringReplacer.Text += text.Text;
                        stringReplacers.Add(stringReplacer);
                    }

                    stringReplacers.Reverse();

                    foreach (var item in stringReplacers)
                    {
                        procedureContent = procedureContent.Substring(0, item.IndexBegin) + item.Text + procedureContent.Substring(item.IndexEnd);
                    }

                    var declareParam = "DECLARE " + string.Join(",\r\n", parameterInfos.Select(x =>
                    {
                        object parameterValue = null;
                        var property = properties.FirstOrDefault(p => CompareName(p.Name, x.PARAMETER_NAME));

                        if (property != null)
                        {
                            parameterValue = GetParameterValue(property, parameters);
                        }
                        return GetValuePaging(x, parameterValue);
                    }));

                    procedureContent = declareParam + "\r\n" + procedureContent;

                    var multiResult = await conn.QueryMultipleAsync(
                        "-- PROCEDURE NAME: " + storedProcName + "\r\n\r\n" + procedureContent,
                        commandTimeout: commandTimeout
                    );

                    if (totalCount == 0)
                    {
                        totalCount = (await multiResult.ReadAsync<int>()).FirstOrDefault();
                    }

                    var itemsResult = (await multiResult.ReadAsync<TModel>()).ToList();

                    return new PagedResultDto<TModel>()
                    {
                        Items = itemsResult,
                        TotalCount = totalCount
                    };
                }
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message, e);
            }
        }

        public class ReplaceStringResult
        {
            public string Text { get; set; }
            public int IndexBegin { get; set; }
            public int IndexEnd { get; set; }
        }

        bool CompareName(string propertyName, string parameterName)
        {
            return parameterName.Replace("@", "").ToLower().Equals(propertyName.Replace("@", "").ToLower())
                 || parameterName.Replace("@", "").ToLower().Equals("p_" + propertyName.Replace("@", "").ToLower());
        }


        private async Task<List<StoreParameterInfoDto>> GetParameterInfos(string storeProcName)
        {
            var connStr = await GetConnectionStringAsync();
            using (var conn = new SqlConnection(connStr))
            {
                var rr = await conn.QueryAsync<StoreParameterInfoDto>($"select PARAMETER_NAME, PARAMETER_MODE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from information_schema.parameters where specific_name = @storeProcName", new
                {
                    storeProcName
                });
                return rr.ToList();
            }
        }

        private StoreParameterInfoDto GetParameterInfo(List<StoreParameterInfoDto> parameterInfos, string paramName)
        {
            var result = parameterInfos
                .Where(x => x.PARAMETER_NAME.Replace("@", "").ToLower().Equals(paramName.Replace("@", "").ToLower())
                || x.PARAMETER_NAME.Replace("@", "").ToLower().Equals("p_" + paramName.Replace("@", "").ToLower())
                || x.PARAMETER_NAME.Replace("@", "").ToLower().Equals("l_" + paramName.Replace("@", "").ToLower()))
                .SingleOrDefault();
            return result;
        }

        private ParameterDirection GetParameterDirection(StoreParameterInfoDto parameterInfo)
        {
            switch (parameterInfo.PARAMETER_MODE)
            {
                case ParameterSqlDirection.Input:
                    return ParameterDirection.Input;
                case ParameterSqlDirection.InputOutput:
                    return ParameterDirection.InputOutput;
                case ParameterSqlDirection.Output:
                    return ParameterDirection.Output;
            }
            return ParameterDirection.Input;
        }

        string GetValuePaging(StoreParameterInfoDto paramInfo, object value)
        {
            var name = paramInfo.PARAMETER_NAME;
            var type = paramInfo.DATA_TYPE.ToLower();

            switch (type)
            {
                case "bit":
                    // FIX: không cast cứng (bool), hỗ trợ bool?, int, string
                    bool? b = null;
                    if (value is bool bv) b = bv;
                    else if (value is bool?) b = (bool?)value;
                    else if (value is int iv) b = iv != 0;
                    return $"{name} {paramInfo.DATA_TYPE} = {(b.HasValue ? (b.Value ? "1" : "0") : "NULL")}";

                case "int":
                case "numeric":
                case "decimal":
                case "float":
                case "money":
                    // FIX: invariant culture
                    return $"{name} {paramInfo.DATA_TYPE} = " +
                           (value != null
                               ? Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)
                               : "NULL");

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    // FIX: hỗ trợ datetime — dùng parameter thay vì concat
                    return value is DateTime dt
                        ? $"{name} {paramInfo.DATA_TYPE} = '{dt:yyyy-MM-dd HH:mm:ss.fff}'"
                        : $"{name} {paramInfo.DATA_TYPE} = NULL";

                case "xml":
                    // FIX: escape ' để tránh SQL Injection
                    return $"{name} {paramInfo.DATA_TYPE} = " +
                           (value != null ? "N'" + value.ToString().Replace("'", "''") + "'" : "NULL");

                case "varchar":
                case "varchar2":
                case "nchar":
                case "char":
                case "nvarchar":
                    var len = paramInfo.CHARACTER_MAXIMUM_LENGTH == -1
                        ? "MAX"
                        : paramInfo.CHARACTER_MAXIMUM_LENGTH.ToString();
                    return $"{name} {paramInfo.DATA_TYPE}({len}) = " +
                           (value != null ? "N'" + value.ToString().Replace("'", "''") + "'" : "NULL");

                default:
                    throw new UserFriendlyException($"Chưa hỗ trợ kiểu dữ liệu: {paramInfo.DATA_TYPE}");
            }
        }



        private string GetParameterName(PropertyInfo property)
        {
            var paramName = "";

            var storeParameterAttribute = (StoreParamAttribute)property.GetCustomAttributes(typeof(StoreParamAttribute), false).FirstOrDefault();

            if (storeParameterAttribute == null)
            {
                paramName = property.Name;
            }
            else
            {
                paramName = storeParameterAttribute.Name;
            }
            return "@" + paramName;
        }

        private object GetParameterValue(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value.GetType() == typeof(DateTime?))
            {
                return ((DateTime?)value).Value.ToString(ThienPhucDentalCoreConst.DateTimeFormat);
            }
            if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToString(ThienPhucDentalCoreConst.DateTimeFormat);
            }
            return value;
        }

        private object GetParameterValue(PropertyInfo property, object obj)
        {
            var value = property.GetValue(obj);
            return GetParameterValue(value);
        }

        private static IEnumerable<ReplaceStringResult> ExtractFromString(string source, string start, string end)
        {
            ReplaceStringResult result = new ReplaceStringResult();

            result.IndexBegin = 0;
            result.IndexEnd = 0;
            result.Text = "";

            while ((result.IndexBegin = source.ToUpper().IndexOf(start.ToUpper(), result.IndexBegin)) >= 0 && (result.IndexEnd = source.ToUpper().IndexOf(end.ToUpper(), result.IndexBegin)) >= 0)
            {
                result.IndexBegin += start.Length;
                result.Text = source.Substring(result.IndexBegin, result.IndexEnd - result.IndexBegin);
                yield return result;
                result.IndexBegin = result.IndexEnd;
            }


        }

    }
}
