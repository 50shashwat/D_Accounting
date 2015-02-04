using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_AccountingCore
{
    public class DescriptionCase : AbstractCase
    {
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
    }
}
