using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MiniSen_Common.Helpers.Config
{
    public static class ConfigHelper
    {
        // <summary>
        /// 获取配置文件中的内容，继承自IConfiguration
        /// </summary>
        private static IConfiguration _configuration { get; set; }

        static ConfigHelper()
        {
            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "appsettings.json";

            var directory = AppContext.BaseDirectory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            _configuration = builder.Build();
        }

        public static string GetConfig(string configName)
        {
            return _configuration[configName];
        }
    }
}
