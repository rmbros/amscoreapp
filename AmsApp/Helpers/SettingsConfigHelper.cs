// <copyright file="SettingsConfigHelper.cs" company="Thinksmart, Inc.">
// Copyright (c) Thinksmart, Inc.. All rights reserved.
// </copyright>

namespace AmsApp.Helpers
{
    using Microsoft.Extensions.Configuration;

    using System;
    using System.IO;

    public class SettingsConfigHelper
    {
        private static SettingsConfigHelper appSettings;

        public string AppSettingValue { get; set; }

        public static string AppSetting(string key)
        {
            appSettings = GetCurrentSettings(key);
            return appSettings.AppSettingValue;
        }

        public static string Get(string key)
        {
            return AppSetting(key);
        }

        public static int GetInt(string key)
        {
            return Convert.ToInt32(AppSetting(key));
        }

        public static bool GetBoolean(string key)
        {
            return Convert.ToBoolean(AppSetting(key));
        }


        public SettingsConfigHelper(IConfiguration config, string key)
        {
            this.AppSettingValue = config.GetValue<string>(key);
        }

        // Get a valued stored in the appsettings.
        // Pass in a key like TestArea:TestKey to get TestValue
        public static SettingsConfigHelper GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new SettingsConfigHelper(configuration.GetSection("AppSettings"), key);

            return settings;
        }
    }
}
