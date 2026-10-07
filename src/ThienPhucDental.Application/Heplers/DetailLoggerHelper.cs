using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using ThienPhucDental.Helper;
using ThienPhucDental.Configuration;
using System.Threading;

namespace ThienPhucDental.Heplers
{
    public class DetailLoggerHelper: IDetailLoggerHelper
    {
        private readonly IConfigurationRoot appConfiguration;
        private readonly string logPath;
        private readonly bool enableEventLog;
        private static readonly object _logLock = new object();

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
                return;

            if (string.IsNullOrWhiteSpace(logPath))
                return;

            const int maxRetries = 3;
            const int delayMs = 100;

            lock (_logLock)
            {
                for (int attempt = 0; attempt < maxRetries; attempt++)
                {
                    try
                    {
                        File.AppendAllText(logPath, log + Environment.NewLine);
                        return;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Logger failed (attempt {attempt + 1}/{maxRetries}): {ex.Message}");

                        if (attempt < maxRetries - 1)
                            Thread.Sleep(delayMs * (attempt + 1));
                    }
                }
            }
        }
    }
}
