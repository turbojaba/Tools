using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using NLog.Config;
using NLog.Targets;
using Squirrel;

namespace Tools
{
    public static class Helper
    {
        public static string AppName => Assembly.GetEntryAssembly()?.GetName().Name;

        public static string Version => Assembly.GetEntryAssembly()?.GetName().Version.ToString(3);

        public static string AppNameWithVersion => $"{AppName} v{Version}";

        public static string DefaultLogLayout = "${time}|${level:uppercase=true}|${processid}|${threadid}|${callsite}|${message}";

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // 12 часов
        public static Timer UpdateTimer { get; set; } = new Timer(43_200_000);

        public delegate void AppUpdateHandler();

        public static event AppUpdateHandler AppUpdated;

        // http://buherpet.tk:9999/updates/{Helper.AppName}
        public static string UpdateUrl { get; private set; }

        public static void MagickInitMethod(string updateUrl)
        {
            var debug = true;
#if !DEBUG
            debug = false;
#endif

            UpdateUrl = updateUrl;
            var appWillBeUpdated = !UpdateUrl.IsNullOrEmpty();
            var pathPrefix = !debug && appWillBeUpdated ? "..\\" : "";
            LogManager.Configuration = DefaultLogConfig(pathPrefix: pathPrefix);

            Log.HelloWorld();

            if (!debug && appWillBeUpdated)
            {
                UpdateTimer.Elapsed += UpdateTimerOnElapsed;
                UpdateTimer.Start();
                UpdateApp();
            }
        }

        private static void UpdateTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateApp();
        }

        //Если ЕХЕшник вытащили из аппдаты, то логи будут неправильно писаться и приложение не будет обновляться
        public static async Task UpdateApp()
        {
            Log.Info($"Start");

            if (UpdateUrl.IsNullOrEmpty())
            {
                Log.Info($"Ссылка на апдейты не установлена, скип");
                return;
            }

            try
            {
                Log.Info($"UpdateUrl: {UpdateUrl}");
                using (var mgr = new UpdateManager(UpdateUrl))
                {
                    Log.Info($"CheckForUpdate...");
                    var updateInfo = await mgr.CheckForUpdate();
                    Log.Info($"ReleasesToApply.Count: {updateInfo.ReleasesToApply.Count}");
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        Log.Info($"UpdateApp()");
                        await mgr.UpdateApp(UpdateProgress).ConfigureAwait(false);
                        Log.Info($"UpdateHelper.AppUpdated?.Invoke()");
                        AppUpdated?.Invoke();
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void UpdateProgress(int obj)
        {
            Log.Info($"{obj}");
        }

        public static void RestartApp()
        {
            UpdateManager.RestartApp();
        }

        public static LoggingConfiguration DefaultLogConfig(bool entryAssemblyNameAsFileName = true, string pathPrefix = "")
        {
            var appName = entryAssemblyNameAsFileName ? $"{AppName}-" : "";
            var layout = DefaultLogLayout;
            var config = new LoggingConfiguration();

            var logfile = new FileTarget("logfile")
            {
                FileName = pathPrefix + "logs\\" + appName + "${shortdate}.txt",
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

        public static void Bye(this Logger log, int? exitCode = null)
        {
            var exitCodeStr = $". ExitCode: {exitCode}";

            log.Info($"Bye-bye{(exitCode == null ? "" : exitCodeStr)}");
        }
    }
}