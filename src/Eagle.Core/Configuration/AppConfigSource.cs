using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Eagle.Core.Configuration
{
    public class EAppConfigSource : IConfigSource
    {
        private EAppConfigurationSection configurationSection;

        private string defaultConfigSectionName = "EApp";

        public EAppConfigSource() 
        {
            this.LoadConfig();
        }

        public EAppConfigSource(string configFilePath)
        {
            this.LoadConfig(configFilePath);
        }

        private void LoadConfig() 
        {
            this.configurationSection = (EAppConfigurationSection)ConfigurationManager.GetSection(defaultConfigSectionName);
        }

        public void LoadConfig(string configFilePath)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap() { ExeConfigFilename = configFilePath };

            System.Configuration.Configuration mappedExeConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            this.configurationSection = (EAppConfigurationSection)mappedExeConfiguration.GetSection(defaultConfigSectionName);
        }

        public EAppConfigurationSection Config
        {
            get 
            {
                return this.configurationSection;
            }
        }
    }
}
