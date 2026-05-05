using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ThienPhucDental.Procedures.Attributes
{
    public class StoreParamAttribute : Attribute
    {
        public StoreParamAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
