using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TestProfitCenterClient
{
    class SettingsLoad
    {
        private string setting_file_name = "settings.xml";

        public Setting setting { get; set; }
        private static SettingsLoad instance;
        private static object syncRoot = new object();
        private static object syncLoadSet = new object();
        private SettingsLoad()
        {
            if (setting == null)
                LoadSettings();
        }

        public static SettingsLoad getInstance()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new SettingsLoad();
                }
            }
            return instance;
        }

        public bool LoadSettings()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Setting));
            string patch = get_setting_file_patch();
            lock (syncLoadSet)
            {
                if (System.IO.File.Exists(patch))
                {
                    try
                    {
                        using (StreamReader writer = new StreamReader(get_setting_file_patch()))
                        {
                            IExtendedXmlSerializer serializer = new ConfigurationContainer().UseAutoFormatting()
                                                                    .UseOptimizedNamespaces()
                                                                    .EnableImplicitTyping(typeof(Setting))
                                                                    .Create();
                            setting = serializer.Deserialize<Setting>(writer);
                        }
                    }
                    catch (Exception ex)
                    {
                        setting = new Setting();
                        Console.WriteLine("Error load setting: "+ ex);
                    }

                }
                else
                {
                    setting = new Setting();
                }
            }
            return true;
        }

        public bool SaveSattings()
        {
            lock (syncLoadSet)
            {
                try
                {
                    IExtendedXmlSerializer serializer = new ConfigurationContainer().UseAutoFormatting()
                                                                    .UseOptimizedNamespaces()
                                                                    .EnableImplicitTyping(typeof(Setting))
                                                                    .Create();
                    var document = serializer.Serialize(new XmlWriterSettings { Indent = true }, setting);
                    using (StreamWriter writer = new StreamWriter(get_setting_file_patch()))
                    {
                        writer.WriteLine(document);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error save setting: "+ ex);
                }
            }
            return true;
        }

        private string get_setting_file_patch()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + setting_file_name;
        }
    }
}
