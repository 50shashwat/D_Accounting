using System;
using System.Xml;

namespace D_AccountingCore
{
    public class DateCase : AbstractCase
    {
        public new const string XMLNAME = "DateCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public DateCase() : base() { }
        public DateCase(System.Xml.XmlReader r) : base(r) { }

        public DateTime Date
        {
            get
            {
                return mDate;
            }
            set
            {
                mDate = value;
                OnPropertyChanged("Date");
            }
        }
        private DateTime mDate;

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement();
            Date = XmlConvert.ToDateTime(reader.ReadElementContentAsString(XmlTags.Date, ""), XmlDateTimeSerializationMode.Utc);
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString(XmlTags.Date, Date.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        public override AbstractCase Clone()
        {
            DateCase c = new DateCase();
            c.Date = Date;
            c.Column = Column;
            c.Row = Row;
            return c;
        }
    }
}
