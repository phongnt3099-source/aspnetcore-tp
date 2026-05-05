using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Procedures.Attributes
{
    public class StoreParameterInfoDto
    {
        public string PARAMETER_NAME { get; set; }
        public string PARAMETER_MODE { get; set; }
        public string DATA_TYPE { get; set; }
        public int CHARACTER_MAXIMUM_LENGTH { get; set; }
    }
}
