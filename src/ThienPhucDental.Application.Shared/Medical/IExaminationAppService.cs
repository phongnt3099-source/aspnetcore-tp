using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Medical.Dto;

namespace ThienPhucDental.Medical
{
    public interface IExaminationAppService: IApplicationService
    {
        Task<PagedResultDto<MED_EXAMINATION_ENTITY>> MED_EXAMINATION_Search(MED_EXAMINATION_ENTITY input);
        Task<MED_EXAMINATION_ENTITY> MED_EXAMINATION_GetById(string Id);
        Task<InsertResult> MED_EXAMINATION_Ins(MED_EXAMINATION_ENTITY input);
        Task<InsertResult> MED_EXAMINATION_Upd(MED_EXAMINATION_ENTITY input);
        Task<CommonResult> MED_EXAMINATION_Del(string Id);
        Task<List<MED_EXAMINATION_ENTITY>> MED_EXAMINATION_DROPDOWNLIST(); 
    }
}
