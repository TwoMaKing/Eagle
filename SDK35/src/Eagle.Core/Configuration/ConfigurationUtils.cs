using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Eagle.Core.Configuration
{
    public static class ConfigurationUtils
    {
        public static TConfigurationSection GetConfigurationSection<TConfigurationSection>(string configFilePath, string configSectionName) 
            where TConfigurationSection : ConfigurationSection, new()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();

            configFileMap.ExeConfigFilename = configFilePath;

            System.Configuration.Configuration mappedExeConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            return (TConfigurationSection)mappedExeConfiguration.GetSection(configSectionName);
        }
    }
}
