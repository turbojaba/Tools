﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using System.Windows.Markup;
using Newtonsoft.Json;
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

        public static string SettingsPath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppName}\\Settings.json";

        public static DirectoryInfo SettingsFolder => new DirectoryInfo(SettingsPath).Parent;

        public static void CreateSettingsFolderIfNotExist()
        {
            if (!SettingsFolder.Exists)
            {
                SettingsFolder.Create();
            }
        }

        public static void MagickInitMethod(string updateUrl = "")
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

        public static void Bye(this Logger log, int exitCode)
        {
            //var exitCodeStr = $". ExitCode: {exitCode}";

            //log.Info($"Bye-bye{(exitCode == null ? "" : exitCodeStr)}");

            log.Info($"ExitCode: {exitCode}");
        }

        // https://stackoverflow.com/questions/6145888/how-to-bind-an-enum-to-a-combobox-control-in-wpf
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            // If no description is found, the least we can do is replace underscores with spaces
            // You can add your own custom default formatting logic here
            //var ti = CultureInfo.CurrentCulture.TextInfo;
            //return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
            return value.ToString();
        }

        public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription { Value = e, Description = e.Description() }).ToList();
        }
    }

    [ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Helper.GetAllValuesAndDescriptions(value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ValueDescription
    {
        public Enum Value { get; set; }

        public string Description { get; set; }
    }

    //public abstract class Settings<T> where T : class
    //{
    //    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    //    [JsonIgnore]
    //    public Timer SavingTimer = new Timer(5_000)
    //    {
    //        AutoReset = false,
    //        Enabled = false,
    //    };

    //    public void Save()
    //    {
    //        var settingsJson = JsonConvert.SerializeObject(this, Formatting.Indented);
    //        //Log.Info($"settingsJson: {settingsJson}");

    //        Helper.CreateSettingsFolderIfNotExist();

    //        File.WriteAllText(Helper.SettingsPath, settingsJson, Encoding.UTF8);
    //    }

    //    public T Load()
    //    {
    //        if (File.Exists(Helper.SettingsPath))
    //        {
    //            var settingsJson = File.ReadAllText(Helper.SettingsPath);
    //            var settings = JsonConvert.DeserializeObject<T>(settingsJson);
    //            return settings;
    //        }
    //        else
    //        {
    //            // first run
    //            return null;
    //        }
    //    }

    //    public void RestartSavingTimer()
    //    {
    //        //Log.Info("Start");
    //        SavingTimer.Stop();
    //        SavingTimer.Start();
    //    }
    //}
}