using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ThienPhucDental.Editions.Dto
{
    public class EditionWithFeaturesDto
    {
        public EditionWithFeaturesDto()
        {
            FeatureValues = new List<NameValueDto>();
        }

        public EditionSelectDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }
    }
}