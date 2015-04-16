using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using D_AccountingCore;

namespace D_Accounting
{
    internal class ListCasesXmlWriterParser
    {
        private XmlWriter writer;

        internal ListCasesXmlWriterParser(FileInfo filePath)
        {
            filePath.Directory.Create();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            writer = XmlWriter.Create(filePath.ToString(), settings);
        }

        internal void Write(ListCases cases)
        {
            writer.WriteStartElement(XmlTags.ListCases);
            cases.WriteXml(writer);
            writer.WriteEndElement();

            writer.Close();
        }
    }
}
