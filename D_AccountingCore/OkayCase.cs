using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class OkayCase : AbstractCase
    {
        public bool IsOkay
        {
            get
            {
                return mIsOkay;
            }
            set
            {
                mIsOkay = value;
                OnPropertyChanged("IsOkay");
            }
        }
        private bool mIsOkay;
    }
}
