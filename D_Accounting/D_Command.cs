using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public abstract class D_Command
    {
        protected MainViewModel ViewModel;

        protected ListCases ListCases;

        public D_Command(MainViewModel vm, ListCases lc)
        {
            ViewModel = vm;
            ListCases = lc;
        }

        public abstract void Execute();

        public abstract void ExecuteReverse();
    }
}
