using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tools.Tests
{
    public class Tests
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Helper.MagickInitMethod();
            //var productName = "";
            //var entryAssembly = Assembly.GetEntryAssembly();
            //var customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            //if (customAttributes != null && customAttributes.Length != 0)
            //{
            //    productName = ((AssemblyProductAttribute) customAttributes[0]).Product;
            //    Log.Info($"1) {productName}");
            //}
        }

        [SetUp]
        public void SetUp()
        {
            Helper.MagickInitMethod();
        }

        //public Settings Settings { get; set; } = new Settings();

        [Test]
        public void Test1()
        {
            //Settings.Keks.Add(new Kek { Name = "test1" });
            //var q = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            //Log.Info($"\r\n{q}");
        }
    }
}