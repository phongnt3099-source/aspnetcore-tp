using Abp.Application.Services.Dto;
using Abp.Authorization;
using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Medical.Dto;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Medical
{
    [AbpAuthorize]
    public class ExaminationAppService: IExaminationAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public ExaminationAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<MED_EXAMINATION_ENTITY> MED_EXAMINATION_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<MED_EXAMINATION_ENTITY>(CommonStoreProcedureConsts.MED_EXAMINATION_BYID, new
            {
                P_EXM_ID = Id
            })).FirstOrDefault();

            if(result == null)
            {
                return null;
            }

            result.TreatmentDetails = (await _storeProcedureProvider.GetDataFromStoredProcedure<MED_TREATMENT_DETAIL_ENTITY>(
                CommonStoreProcedureConsts.MED_TREATMENT_DETAIL_BYID,
                new { @p_TD_EXM_ID = Id }
            ));
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<MED_EXAMINATION_ENTITY>> MED_EXAMINATION_Search(MED_EXAMINATION_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<MED_EXAMINATION_ENTITY>(CommonStoreProcedureConsts.MED_EXAMINATION_SEARCH, input);
            
            if (result == null)
            {
                return null;
            }
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> MED_EXAMINATION_Ins(MED_EXAMINATION_ENTITY input)
        {
            string treatmentDetailsXml = null;
            if (input.TreatmentDetails != null && input.TreatmentDetails.Any())
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = false;

                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<MED_TREATMENT_DETAIL_ENTITY>));
                        xmlSerializer.Serialize(xmlWriter, input.TreatmentDetails);

                        xmlWriter.Flush();
                    }

                    treatmentDetailsXml = stringWriter.ToString();
                }
            }
            var parameters = new
            {
                P_EXM_PATIENT_ID = input.EXM_PATIENT_ID,
                P_EXM_DOCTOR_ID = input.EXM_DOCTOR_ID,
                P_EXM_CHIEF_COMPLAINT = input.EXM_CHIEF_COMPLAINT,
                P_EXM_CLINICAL_SIGNS = input.EXM_CLINICAL_SIGNS,
                P_EXM_DIAGNOSIS = input.EXM_DIAGNOSIS,
                P_EXM_PULSE = input.EXM_PULSE,
                P_EXM_BLOOD_PRESSURE = input.EXM_BLOOD_PRESSURE,
                P_EXM_NOTE = input.EXM_NOTE,
                P_EXM_STATUS = input.EXM_STATUS,
                P_EXM_TOTAL_DISCOUNT = input.EXM_TOTAL_DISCOUNT,
                P_EXM_DATE = input.EXM_DATE,
                P_EXM_FINAL_AMOUNT = input.EXM_FINAL_AMOUNT,
                P_EXM_SUB_TOTAL = input.EXM_SUB_TOTAL,
                P_EXM_TOTAL_RAW = input.EXM_TOTAL_RAW,
                P_MAKER_ID = input.MAKER_ID,
                TreatmentDetailsXML = treatmentDetailsXml
            };
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_EXAMINATION_INS, parameters)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> MED_EXAMINATION_Upd(MED_EXAMINATION_ENTITY input)
        {
            string treatmentDetailsXml = null;
            if (input.TreatmentDetails != null && input.TreatmentDetails.Any())
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = false;

                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<MED_TREATMENT_DETAIL_ENTITY>));
                        xmlSerializer.Serialize(xmlWriter, input.TreatmentDetails);

                        xmlWriter.Flush();
                    }

                    treatmentDetailsXml = stringWriter.ToString();
                }
            }
            var parameters = new
            {
                P_EXM_ID = input.EXM_ID,
                P_EXM_PATIENT_ID = input.EXM_PATIENT_ID,
                P_EXM_DOCTOR_ID = input.EXM_DOCTOR_ID,
                P_EXM_CHIEF_COMPLAINT = input.EXM_CHIEF_COMPLAINT,
                P_EXM_CLINICAL_SIGNS = input.EXM_CLINICAL_SIGNS,
                P_EXM_DIAGNOSIS = input.EXM_DIAGNOSIS,
                P_EXM_PULSE = input.EXM_PULSE,
                P_EXM_BLOOD_PRESSURE = input.EXM_BLOOD_PRESSURE,
                P_EXM_NOTE = input.EXM_NOTE,
                P_EXM_STATUS = input.EXM_STATUS,
                P_EXM_TOTAL_DISCOUNT = input.EXM_TOTAL_DISCOUNT,
                P_EXM_DATE = input.EXM_DATE,
                P_EXM_FINAL_AMOUNT = input.EXM_FINAL_AMOUNT,
                P_EXM_SUB_TOTAL = input.EXM_SUB_TOTAL,
                P_EXM_TOTAL_RAW = input.EXM_TOTAL_RAW,
                P_MAKER_ID = input.MAKER_ID,
                TreatmentDetailsXML = treatmentDetailsXml
            };
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_EXAMINATION_UPD, parameters)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> MED_EXAMINATION_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.MED_EXAMINATION_DEL, new
                {
                    P_MED_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}
