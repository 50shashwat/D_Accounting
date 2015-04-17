using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class AmountCase : AbstractCase
    {
        public new const string XMLNAME = "AmountCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public AmountCase() : base() { }
        public AmountCase(System.Xml.XmlReader r) : base(r) { }

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

        public override AbstractCase Clone()
        {
            AmountCase c = new AmountCase();
            c.Amount = Amount;
            c.Column = Column;
            c.Row = Row;
            return c;
        }
    }
}
