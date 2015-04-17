using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace D_AccountingCore
{
    public abstract class AbstractCase : INotifyPropertyChanged, IXmlSerializable
    {
        public int Row
        {
            get
            {
                return mRow;
            }
            set
            {
                mRow = value;
                OnPropertyChanged("Row");
            }
        }
        private int mRow;

        public int Column
        {
            get
            {
                return mColumn;
            }
            set
            {
                mColumn = value;
                OnPropertyChanged("Column");
            }
        }
        private int mColumn;

        public const string XMLNAME = "AbstractCase";
        public abstract string XmlName { get ; }

        public AbstractCase() {}
        public AbstractCase(XmlReader r)
        {
            ReadXml(r);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Compares an case to another one (with the row and column)
        /// </summary>
        /// <param name="item">A case</param>
        /// <returns>==0 if both are equal OR less than 0 if the calling case if inferior</returns>
        public decimal CompareTo(AbstractCase item)
        {
            decimal thisSuperior = 1;
            decimal itemSuperior = -1;

            if (item.Row < this.Row)
                return thisSuperior;

            if (item.Row > this.Row)
                return itemSuperior;

            if (item.Column < this.Column)
                return thisSuperior;

            if (item.Column > this.Column)
                return itemSuperior;

            return 0;
        }

        /// <summary>
        /// Returns a hash code in order to use this class in hash table
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return Row.GetHashCode() * Column.GetHashCode();
        }

        /// <summary>
        /// Checks if the "right" object is equal to this AbstractCase or not
        /// </summary>
        /// <param name="right">The other object to be compared with this AbstractCase</param>
        /// <returns>True if equal, false if not</returns>
        public override bool Equals(object right)
        {
            // Check if null
            if (object.ReferenceEquals(right, null))
                return false;

            // Check if the references are the same
            if (object.ReferenceEquals(this, right))
                return true;

            // Check if the types are not the same
            if (this.GetType() != right.GetType())
                return false;

            return this.Equals(right as AbstractCase);
        }

        /// <summary>
        /// Checks if this AbstractCase is equal to the other AbstractCase
        /// </summary>
        /// <param name="other">The other AbstractCase to be compared with</param>
        /// <returns>True if equal</returns>
        public bool Equals(AbstractCase other)
        {
            return (this.Column == other.Column) && (this.Row == other.Row);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads only the start element attributes
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReadXml(XmlReader reader)
        {
            Row = Int32.Parse(reader.GetAttribute(XmlTags.Row));
            Column = Int32.Parse(reader.GetAttribute(XmlTags.Column));
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(XmlTags.Row, Row.ToString());
            writer.WriteAttributeString(XmlTags.Column, Column.ToString());
        }

        public abstract AbstractCase Clone();
    }
}
