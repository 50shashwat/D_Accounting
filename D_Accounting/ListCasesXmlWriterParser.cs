﻿using System.IO;
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
