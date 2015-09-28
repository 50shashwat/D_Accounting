using System.Globalization;

namespace D_AccountingCore
{
    public class ReadonlyAmountCase : AbstractCase
    {
        public new const string XMLNAME = "ReadonlyAmountCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public ReadonlyAmountCase() : base() { }
        public ReadonlyAmountCase(System.Xml.XmlReader r) : base(r) { }

        public decimal Amount
        {
            get
            {
                return mAmount;
            }
            set
            {
                mAmount = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("IsNegative");
            }
        }
        private decimal mAmount;

        public bool IsNegative
        {
            get
            {
                return mAmount < 0;
            }
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement();
            Amount = reader.ReadElementContentAsDecimal(XmlTags.Amount, "");
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString(XmlTags.Amount, Amount.ToString(new CultureInfo("en-US")));
        }
    }
}
