using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Helper
{
    public interface IDetailLoggerHelper
    {
        void Logger(string log);
        void ActionLog(string log, string actionName);
        void EndLog(string key);
        string StartLog(string label);
    }
}
