using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class DateCase : AbstractCase
    {
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
    }
}
