using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace DKC.JBus
{
    public class AppSettings
    {
        static AppSettings()
        {
            Refresh();
        }

        //[Default(true)]
        //[Default(120)]
        //[Default(-1)]
        //[Default("")]
        //[Default(null)]
        [Default(false)]
        public static bool UseProxyServer { get; set; }

        [Default("")]
        public static string HttpProxyAddress { get; set; }

        [Default(80)]
        public static int HttpProxyPort { get; set; }

        [Default("")]
        public static string HttpProxyUsername { get; set; }

        [Default("")]
        public static string HttpProxyPassword { get; set; }

        [Default("")]
        public static string BizboxVdoUrl { get; set; }

        public static void Refresh()
        {
            var data = Current.DB.AppSettings.All().ToDictionary(v => v.Setting, v => v.Value);

            foreach (var property in typeof(AppSettings).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                string overrideData;

                if (data.TryGetValue(property.Name, out overrideData))
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        bool parsed = false;
                        Boolean.TryParse(overrideData, out parsed);
                        property.SetValue(null, parsed, null);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        int parsed = -1;
                        if (int.TryParse(overrideData, out parsed))
                        {
                            property.SetValue(null, parsed, null);
                        }
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(null, overrideData, null);
                    }
                    else if (overrideData[0] == '{' && overrideData[overrideData.Length - 1] == '}')
                    {
                        try
                        {
                            property.SetValue(null, JsonConvert.DeserializeObject(overrideData, property.PropertyType), null);
                        }
                        catch (JsonSerializationException)
                        {
                            // Just in case
                            property.SetValue(null, null, null);
                        }
                    }
                }
                else
                {
                    DefaultAttribute attrib = (DefaultAttribute)property.GetCustomAttributes(typeof(DefaultAttribute), false)[0];
                    property.SetValue(null, attrib.DefaultValue, null);
                }
            }
        }
    }
}