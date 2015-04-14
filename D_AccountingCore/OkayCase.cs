using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace D_AccountingCore
{
    public class OkayCase : AbstractCase
    {
        public new const string XMLNAME = "OkayCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public OkayCase() : base() { }
        public OkayCase(System.Xml.XmlReader r) : base(r) { }

        public bool IsOkay
        {
            get
            {
                return mIsOkay;
            }
            set
            {
                mIsOkay = value;
                OnPropertyChanged("IsOkay");
            }
        }
        private bool mIsOkay;

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement();
            if (reader.ReadElementContentAsString(XmlTags.Okay, "").Equals(Boolean.TrueString))
                IsOkay = true;
            else
                IsOkay = false;
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString(XmlTags.Okay, IsOkay.ToString());
        }
    }
}
