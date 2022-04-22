using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TreadSys.Common
{
    class ConfigurationClass
    {
        /// <summary>
        /// 读取客户设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="valueName"></param>
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="valueName"></param> 
        public static void RemoveSettingString(string settingName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
