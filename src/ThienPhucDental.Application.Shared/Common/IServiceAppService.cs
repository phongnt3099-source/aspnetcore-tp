using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Common
{
    public interface IServiceAppService: IApplicationService
    {
        Task<List<CM_SERVICES_ENTITY>> CM_SERVICES_GetByType(string Keyword, string ST_ID);
    }
}
