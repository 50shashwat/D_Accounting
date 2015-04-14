using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class GrayUnaccessibleCase : AbstractCase
    {
        public new const string XMLNAME = "GrayUnaccessibleCase";
        public override string XmlName
        {
            get { return XMLNAME; }
        }

        public GrayUnaccessibleCase() : base() { }
        public GrayUnaccessibleCase(System.Xml.XmlReader r) : base(r) { }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            reader.ReadStartElement(); // not reading end, because it's on one line only
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
        }
    }
}
