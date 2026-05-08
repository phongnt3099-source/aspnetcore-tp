using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class PagedAndSortedInputDto: PagedInputDto, ISortedResultRequest
    {
        public int? TOP { get; set; }
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}
