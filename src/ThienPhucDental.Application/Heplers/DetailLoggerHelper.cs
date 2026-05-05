using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using ThienPhucDental.Helper;
using ThienPhucDental.Configuration;

namespace ThienPhucDental.Heplers
{
    public class DetailLoggerHelper: IDetailLoggerHelper
    {
        private readonly IConfigurationRoot appConfiguration;
        private readonly string logPath;
        private readonly bool enableEventLog;

        public DetailLoggerHelper(IWebHostEnvironment env)
        {
            appConfiguration = env.GetAppConfiguration();
            logPath = env.ContentRootPath + "/wwwroot/EventLogger.txt";
            enableEventLog = appConfiguration["App:EnableEventLog"] == "1";
        }


        public static int ProcessId = 0;
        public static IDictionary<string, DateTime> StopwatchMap { get; set; }
        public static IDictionary<string, DateTime> ActionTimerMap { get; set; } = new Dictionary<string, DateTime>();
        public static IDictionary<string, DateTime> GetStopwatchMap
        {
            get
            {
                if (StopwatchMap == null)
                {
                    StopwatchMap = new Dictionary<string, DateTime>();
                }
                return StopwatchMap;
            }
        }

        public string StartLog(string label)
        {
            if (!enableEventLog)
            {
                return "";
            }
            var key = label + "-" + (ProcessId++);
            var startDate = DateTime.Now;
            GetStopwatchMap.Add(key, startDate);
            ActionTimerMap.Add(key, startDate);
            Logger(key + "\tstart.\t" + DateTime.Now.ToString("yyyy/mm/dd hh:mm:ss.fff tt") + ".");
            return key;
        }

        public void ActionLog(string key, string actionName)
        {
            if (!enableEventLog)
            {
                return;
            }
            if (!GetStopwatchMap.Keys.Contains(key))
            {
                Logger(key + " not found");
            }

            var currentDate = DateTime.Now;

            var lastDate = ActionTimerMap[key];

            var elap = (currentDate - lastDate).TotalMilliseconds;

            ActionTimerMap[key] = currentDate;

            Logger(key + "(" + actionName + ")" + "\tend.\t" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff tt") + "\tElapsedMilliseconds:\t" + elap.ToString().Replace(",", "."));
        }

        public void EndLog(string key)
        {
            if (!enableEventLog)
            {
                return;
            }
            if (!GetStopwatchMap.Keys.Contains(key))
            {
                Logger(key + " not found");
            }

            var lastDate = GetStopwatchMap[key];
            var currentDate = DateTime.Now;
            var elap = (currentDate - lastDate).TotalMilliseconds;

            Logger(key + "\tend.\t" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff tt") + "\tElapsedMilliseconds:\t" + elap.ToString().Replace(",", "."));
        }

        public void Logger(string log)
        {
            if (!enableEventLog)
            {
                return;
            }
            var lo = true;
            while (lo)
            {
                try
                {
                    File.AppendAllText(logPath, log + Environment.NewLine);
                    lo = false;
                }
                catch
                {

                }
            }

        }
    }
}
