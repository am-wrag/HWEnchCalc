using System.IO;
using HWEnchCalc.Common;

namespace HWEnchCalc.Config
{
    public static class ConfigurationManager
    {
        private const string DefaultConfigFileName = "appsettings.json";

        public static Configuration GetConfig(string configFilePath = DefaultConfigFileName)
        {
            var fileInfo = new FileInfo(DefaultConfigFileName).FullName;

            var configData = File.ReadAllText(fileInfo);

            return JsonParser<Configuration>.Deserialize(configData);
        }
    }
}