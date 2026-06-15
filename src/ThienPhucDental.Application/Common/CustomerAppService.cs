using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Editions;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class CustomerAppService: ICustomerAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public CustomerAppService( IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<CM_CUSTOMER_ENTITY> CM_CUSTOMER_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_BYID, new
            {
                P_CUS_ID = Id
            })).FirstOrDefault();

            result.RelationList = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_RELATIONSHIP_ENTITY>(
                    CommonStoreProcedureConsts.CM_RELATIONSHIP_BYID,
                    new { P_CUS_ID = Id }
                ));
            return result;
        }

        public async Task<List<CM_CUSTOMER_ENTITY>> CM_CUSTOMER_DROPDOWNLIST()
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_DROPDOWNLIST, new
                {
                });

            return result;
        }
        public async Task<List<CM_CUSTOMER_ENTITY>> CM_CUSTOMER_CheckPhone(string phone,string current_cus_id)
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_CHECKDUPLICATEPHONE, new
                {
                    P_CUS_PHONE = phone,
                    P_CURRENT_CUS_ID = current_cus_id
                });

            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<CM_CUSTOMER_ENTITY>> CM_CUSTOMER_Search(CM_CUSTOMER_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_SEARCH, input);
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> CM_CUSTOMER_Ins(CM_CUSTOMER_ENTITY input)
        {
            string relationXml = null;
            if (input.RelationList != null && input.RelationList.Any())
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = false;

                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<CM_RELATIONSHIP_ENTITY>));
                        xmlSerializer.Serialize(xmlWriter, input.RelationList);

                        xmlWriter.Flush();
                    }

                    relationXml = stringWriter.ToString();
                }
            }
            input.RELATION_XML = relationXml;
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_CUSTOMER_Upd(CM_CUSTOMER_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_CUSTOMER_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CUSTOMER_DEL, new
                {
                    CUS_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}
