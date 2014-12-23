using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public abstract class AbstractCase : INotifyPropertyChanged
    {
        public int Row
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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
    }
}
