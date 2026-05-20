using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.Medical.Dto;

namespace ThienPhucDental.Common
{
    public interface IServiceTypeAppService : IApplicationService
    {
        Task<List<CM_SERVICE_TYPE_ENTITY>> CM_SERVICES_GetAll();
    }
}
