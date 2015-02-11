using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class AmountCase : AbstractCase
    {
        public decimal Amount
        {
            get
            {
                return mAmount;
            }
            set
            {
                OldAmount = mAmount;
                mAmount = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("IsNegative");
            }
        }
        private decimal mAmount;

        public decimal OldAmount
        {
            get;
            private set;
        }

        public bool IsNegative
        {
            get
            {
                return mAmount < 0;
            }
        }
    }
}
