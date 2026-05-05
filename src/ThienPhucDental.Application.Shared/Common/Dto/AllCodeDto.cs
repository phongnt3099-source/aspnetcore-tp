using ThienPhucDental.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class AllCodeDto: PagedAndSortedInputDto
    {
        public int? Id { get; set; }
        public string CDNAME { get; set; }
        public string CDVAL { get; set; }
        public string CONTENT { get; set; }
        public string CDTYPE { get; set; }
        public int? LSTODR { get; set; }
        public int? TotalCount { get; set; }

    }
}
