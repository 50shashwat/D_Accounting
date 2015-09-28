namespace D_AccountingCore
{
    public class DescriptionCase : AbstractCase
    {
        public new const string XMLNAME = "DescriptionCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public DescriptionCase() : base() { }
        public DescriptionCase(System.Xml.XmlReader r) : base(r) { }

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
                OnPropertyChanged("Description");
            }
        }
        private string mDescription;

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement();
            Description = reader.ReadElementContentAsString(XmlTags.Description, "");
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString(XmlTags.Description, Description);
        }
    }
}
