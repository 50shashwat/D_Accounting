using System;

namespace D_AccountingCore
{
    public class FixDescriptionCase : AbstractCase
    {
        public new const string XMLNAME = "FixDescriptionCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public FixDescriptionCase() : base() { }
        public FixDescriptionCase(System.Xml.XmlReader r) : base(r) { }

        public String Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
            }

        }
        private String mName;

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement();
            Name = reader.ReadElementContentAsString(XmlTags.Description, "");
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString(XmlTags.Description, Name);
        }
    }
}
