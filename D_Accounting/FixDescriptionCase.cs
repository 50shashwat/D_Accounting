using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class FixDescriptionCase : AbstractCase
    {
        public String Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
            }

        }
        private String mName;
    }
}
