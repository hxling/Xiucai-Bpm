using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Xiucai.Common
{
    public class ConfigHelper
    {
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }


    }
}
