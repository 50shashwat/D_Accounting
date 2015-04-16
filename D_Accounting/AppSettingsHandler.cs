using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace D_Accounting
{
    public class AppSettingsHandler : IXmlSerializable
    {
        private const string SettingsFileName = "settings.xml";

        public FileInfo DataFilePath
        {
            get
            {
                return mDataFilePath;
            }
            set
            {
                mDataFilePath = value;
            }
        }
        private FileInfo mDataFilePath = new FileInfo(String.Format("{0}\\data\\{1}",
                                            Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                                            "d_accounting_data.xml"));

        public AppSettingsHandler()
        {
            // Read settings file
            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreWhitespace = true;
                readerSettings.IgnoreComments = true;
                readerSettings.IgnoreProcessingInstructions = true;

                using (XmlReader reader = XmlReader.Create(SettingsFileName, readerSettings))
                {
                    ReadXml(reader);
                }

            }
            // If does not exist, write it
            catch (Exception)
            {
                WriteXml();
            }
        }

        public void SaveSettings()
        {
            WriteXml();
        }
        
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            DataFilePath = new FileInfo(reader.ReadElementContentAsString(XmlSettingTags.DataFilePath, ""));
            reader.ReadEndElement();
        }

        public void WriteXml()
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.UTF8;
            writerSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(SettingsFileName, writerSettings))
            {
                writer.WriteStartElement(XmlSettingTags.RootTag);
                WriteXml(writer);
                writer.WriteEndElement();
            }
        }
        
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(XmlSettingTags.DataFilePath, DataFilePath.ToString());
        }


        /// <summary>
        /// Class with xml setting tags
        /// </summary>
        private static class XmlSettingTags
        {
            /// <summary>
            /// D_Accounting_Settings
            /// </summary>
            public const string RootTag = "D_Accounting_Settings";

            /// <summary>
            /// DataFilePath
            /// </summary>
            public const string DataFilePath = "DataFilePath";
        }
    }
}
