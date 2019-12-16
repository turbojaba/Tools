using System;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Tools
{
    public static class Helper
    {
        public static string AppName => Assembly.GetEntryAssembly()?.GetName().Name;

        public static string Version => Assembly.GetEntryAssembly()?.GetName().Version.ToString(3);

        public static string AppNameWithVersion => $"{AppName} v{Version}";

        public static string DefaultLogLayout =
            "${time}|${level:uppercase=true}|${processid}|${threadid}|${callsite}|${message}";

        public static LoggingConfiguration DefaultLogConfig(bool entryAssemblyNameAsFileName = true)
        {
            var appName = entryAssemblyNameAsFileName ? $"{AppName}-" : "";
            var layout = DefaultLogLayout;
            var config = new LoggingConfiguration();

            var logfile = new FileTarget("logfile")
            {
                FileName = "logs\\" + appName + "${shortdate}.txt",
                Layout = layout
            };
            var logconsole = new ColoredConsoleTarget("logconsole")
            {
                Layout = layout
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            return config;
        }

        public static LoggingConfiguration SetFileNamePrefix(this LoggingConfiguration config, string prefix)
        {
            var logFile = config.FindTargetByName<FileTarget>("logfile");
            logFile.FileName = $"{prefix}{logFile.FileName.ToString().Replace("'", "")}";
            return config;
        }

        public static void HelloWorld(this Logger logger)
        {
            var str = "------ " + AppNameWithVersion +
#if DEBUG
                " DEBUG" +
#else
                " RELEASE" +
#endif
                      " ------";
            logger.Info(str);
        }
    }
}