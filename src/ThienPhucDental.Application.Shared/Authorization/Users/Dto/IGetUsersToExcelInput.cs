using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace ThienPhucDental.Authorization.Users.Dto
{
    public interface IGetUsersToExcelInput : IGetUsersInput
    {
        List<string> SelectedColumns { get; set; }
    }
}
