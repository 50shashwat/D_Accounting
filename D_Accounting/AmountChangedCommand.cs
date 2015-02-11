using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class AmountChangedCommand : D_Command
    {
        private decimal OldAmount;
        private decimal Amount;
        private D_AccountingCore.AmountCase AmountCase;

        public AmountChangedCommand(MainViewModel vm, ListCases lc, D_AccountingCore.AmountCase c, decimal amount, decimal oldAmount) : base (vm, lc)
        {
            AmountCase = c;
            Amount = amount;
            OldAmount = oldAmount;
        }

        public override void Execute()
        {
            AmountCase.Amount = Amount;
        }

        public override void ExecuteReverse()
        {
            AmountCase.Amount = OldAmount;
        }
    }
}
