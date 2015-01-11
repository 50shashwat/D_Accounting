using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D_AccountingCore;

namespace D_Accounting
{
    /// <summary>
    /// Drescribes the data grid : case by case from the left to the right, than the next row (like reading)
    /// </summary>
    public class ListCases : ObservableCollection<AbstractCase>
    {
        public ListCases()
        {
            Add(new FixDescriptionCase() { Row = 0, Column = 0, Name = "Account name" });

            Add(new FixDescriptionCase() { Row = 1, Column = 0, Name = "Initial amount" });
            Add(new FixDescriptionCase() { Row = 2, Column = 0, Name = "Real amount" });
            Add(new FixDescriptionCase() { Row = 3, Column = 0, Name = "Theoretical amount" });

            Add(new FixDescriptionCase() { Row = 0, Column = 1, Name = "OK?" });
            Add(new FixDescriptionCase() { Row = 0, Column = 2, Name = "Description" });

            Add(new GrayUnaccessibleCase() { Row = 1, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 1, Column = 2 });
            Add(new GrayUnaccessibleCase() { Row = 2, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 2, Column = 2 });
            Add(new GrayUnaccessibleCase() { Row = 3, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 3, Column = 2 });
        }

        /// <summary>
        /// The number of rows contained by the data table
        /// </summary>
        public int RowCount
        {
            get
            {
                int nbRow = 0;
                List<int> countedRows = new List<int>();
                foreach (AbstractCase c in this)
                    if (!countedRows.Contains(c.Row))
                    {
                        ++nbRow;
                        countedRows.Add(c.Row);
                    }

                return nbRow;
            }
        }

        /// <summary>
        /// The number of columns contained by the data table
        /// </summary>
        public int ColumnCount
        {
            get
            {
                int nbColumn = 0;
                List<int> countedColumns = new List<int>();
                foreach (AbstractCase c in this)
                {
                    if (!countedColumns.Contains(c.Column))
                    {
                        ++nbColumn;
                        countedColumns.Add(c.Column);
                    }
                }
                return nbColumn;
            }
        }
        
        /// <summary>
        /// Get the names on the accounts
        /// </summary>
        public IEnumerable<string> AccountNames
        {
            get
            {
                return  this.Where(c => c.Row == 0 && c.Column >= 1 && c.Column < ColumnCount - 2)
                            .Select(c => (c as FixDescriptionCase).Name);
            }
        }

        /// <summary>
        /// Get the names of the column titles (cases on the first row)
        /// </summary>
        public IEnumerable<string> ColumnTitles
        {
            get
            {
                return  this.Where(c => c.Row == 0)
                            .Select(c => (c as FixDescriptionCase).Name);
            }
        }

        /// <summary>
        /// Override of the InsertItem method of ObservableCollection
        ///     - every add or insert goes by this method
        ///     - insert so that the list is sorted by rows and columns
        /// </summary>
        /// <param name="index">Index where to add : modified to add it to the right position</param>
        /// <param name="item">Item to insert</param>
        protected override void InsertItem(int index, AbstractCase item)
        {
            for (int i = 0; i < Count; ++i)
            {
                switch (Math.Sign(this[i].CompareTo(item)))
                {
                    case 0:
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;
                }
            }  

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Add an account
        /// </summary>
        /// <param name="newAccountName">The name of the new account</param>
        public void AddAccount(string newAccountName)
        {
            int oldColCount = ColumnCount;
            int addedColumnIndex = oldColCount - 2;

            // Move to the right
            foreach (AbstractCase c in this.Where(c => c.Column >= oldColCount - 2))
                c.Column++;

            // Title case
            Add(new FixDescriptionCase() { Row = 0, Column = addedColumnIndex, Name = newAccountName });

            // Initial amount case
            Add(new AmountCase() { Row = 1, Column = addedColumnIndex, Amount = 0.0M});

            // Full & empty oprations
            for (int i = 2; i < RowCount - 2; ++i)
                Add(new GrayUnaccessibleCase() { Row = i, Column = addedColumnIndex });

            // Real & theoretical amount
            Add(new GrayUnaccessibleCase() { Row = RowCount - 2, Column = addedColumnIndex });
            Add(new GrayUnaccessibleCase() { Row = RowCount - 1, Column = addedColumnIndex });
        }

        /// <summary>
        /// Removes an account
        /// </summary>
        /// <param name="accountName">Name of the account that will be removed</param>
        public void RemoveAccount(string accountName)
        {
            // Find the column index
            int colIndex = -1;
            for (int col = 1; col < ColumnCount - 2; ++col)
            {
                if ((this[col] as FixDescriptionCase).Name.Equals(accountName))
                {
                    colIndex = col;
                    break;
                }
            }

            if (colIndex == -1) // Column not found
                return;

            // Remove the whole column (cases at all rows)
            var cases = this.Where(c => c.Column == colIndex);
            for (int i = 0; cases.Count() != 0 ; ++i)
                this.Remove(cases.ElementAt(0));

            // Move all column one to the left
            foreach (AbstractCase c in this.Where(c => c.Column > colIndex))
                c.Column--;
        }
    }
}
