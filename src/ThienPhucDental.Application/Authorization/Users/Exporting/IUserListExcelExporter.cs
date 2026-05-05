using System.Collections.Generic;
using ThienPhucDental.Authorization.Users.Dto;
using ThienPhucDental.Dto;

namespace ThienPhucDental.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}