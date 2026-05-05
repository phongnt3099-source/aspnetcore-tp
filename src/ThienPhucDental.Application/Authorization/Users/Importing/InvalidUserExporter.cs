using System.Collections.Generic;
using Abp.Collections.Extensions;
using ThienPhucDental.Authorization.Users.Importing.Dto;
using ThienPhucDental.DataExporting.Excel.MiniExcel;
using ThienPhucDental.DataImporting.Excel;
using ThienPhucDental.Dto;
using ThienPhucDental.Storage;

namespace ThienPhucDental.Authorization.Users.Importing
{
    public class InvalidUserExporter(ITempFileCacheManager tempFileCacheManager)
        : MiniExcelExcelExporterBase(tempFileCacheManager), IExcelInvalidEntityExporter<ImportUserDto>
    {
        public FileDto ExportToFile(List<ImportUserDto> userList)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var user in userList)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("UserName"), user.UserName},
                    {L("Name"), user.Name},
                    {L("Surname"), user.Surname},
                    {L("EmailAddress"), user.EmailAddress},
                    {L("PhoneNumber"), user.PhoneNumber},
                    {L("Password"), user.Password},
                    {L("Roles"), user.AssignedRoleNames?.JoinAsString(",")},
                    {L("Refuse Reason"), user.Exception}, //TODO@MiniExcel -> localize
                });
            }

            return CreateExcelPackage("InvalidUserImportList.xlsx", items);
        }
    }
}
