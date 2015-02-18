using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D_AccountingCore;
using System.ComponentModel;
using System.Collections.Specialized;

namespace D_Accounting
{
    /// <summary>
    /// Drescribes the data grid : case by case from the left to the right, than the next row (like reading a book)
    /// </summary>
    public class ListCases : ObservableCollection<AbstractCase>
    {
        /// <summary>
        /// If we are actually adding/deleting a column (do not call CaseChanged while modifiying the model)
        /// </summary>
        private bool ModifyingColumns = false;

        /// <summary>
        /// The view model
        /// </summary>
        private MainViewModel ViewModel;

        public ListCases(MainViewModel vm)
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

            ViewModel = vm;
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
            ModifyingColumns = true;

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
            Add(new ReadonlyAmountCase() { Row = RowCount - 2, Column = addedColumnIndex, Amount = 0.0M });
            Add(new ReadonlyAmountCase() { Row = RowCount - 1, Column = addedColumnIndex, Amount = 0.0M });

            OnPropertyChanged(new PropertyChangedEventArgs("AccountNames"));

            ModifyingColumns = false;
        }

        /// <summary>
        /// Removes an account
        /// </summary>
        /// <param name="accountName">Name of the account that will be removed</param>
        public void RemoveAccount(string accountName)
        {
            ModifyingColumns = true;

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

            // Delete useless rows
            DeleteUselessRows();

            OnPropertyChanged(new PropertyChangedEventArgs("AccountNames"));

            ModifyingColumns = false;
        }

        /// <summary>
        /// Deletes the useless rows in the table
        /// </summary>
        private void DeleteUselessRows()
        {
            // Delete rows with only GrayUnaccessibleCases
            int rowIdx = 2;
            while (rowIdx < RowCount - 2)
            {
                bool isOnlyUnaccessible = true;
                for (int colIdx = 1; colIdx < ColumnCount - 2; ++colIdx)
                {
                    if (!(this[GetCaseIndex(colIdx, rowIdx)] is GrayUnaccessibleCase))
                    {
                        isOnlyUnaccessible = false;
                        break;
                    }
                }

                // Delete row + move up all below rows
                if (isOnlyUnaccessible)
                {
                    // Deleting (at position 0, because when you delete => everything moves from one case
                    for (int colIdx = 0; colIdx < ColumnCount; ++colIdx)
                    {
                        this.RemoveAt(GetCaseIndex(0, rowIdx));
                    }
                    // Moving
                    for (int rowIdx2 = rowIdx; rowIdx2 < RowCount; ++rowIdx2)
                    {
                        for (int colIdx = 0; colIdx < ColumnCount; ++colIdx)
                        {
                            this[GetCaseIndex(colIdx, rowIdx2)].Row--;
                        }
                    }
                }
                else
                    ++rowIdx;
            }
        }

        /// <summary>
        /// Adds an operation to the selected account
        /// </summary>
        /// <param name="selectedAccount">The selected account</param>
        public void AddOperation(string selectedAccount)
        {
            ModifyingColumns = true;

            int addedRowIndex = RowCount - 2;
            int accountCol = 0;
            while (!AccountNames.ElementAt(accountCol++).Equals(selectedAccount)) ; // Take the index of the AccountName + 1 (because the accounts begin on the second column)

            // Move the two last rows down
            foreach (AbstractCase c in this.Where(c => c.Row >= addedRowIndex))
                c.Row++;

            // Add date case
            Add(new DateCase() { Row = addedRowIndex, Column = 0, Date = DateTime.Now });

            // Add amount case
            Add(new AmountCase() { Row = addedRowIndex, Column = accountCol, Amount = 0.0M });

            // Add Gray cases (for not selected accounts)
            for (int col = 1; col < ColumnCount - 2; ++col)
                if (col != accountCol)
                    Add(new GrayUnaccessibleCase() { Row = addedRowIndex, Column = col });

            // Add Okay case
            Add(new OkayCase() { Row = addedRowIndex, Column = ColumnCount - 2, IsOkay = false });

            // Add description case
            Add(new DescriptionCase() { Row = addedRowIndex, Column = ColumnCount - 1, Description = ""});

            ModifyingColumns = false;
            UpdateAmounts(accountCol);
        }

        /// <summary>
        /// Deletes the last operation of the selected account
        /// </summary>
        /// <param name="selectedAcccount">The selected account</param>
        public void DeleteOperation(string selectedAcccount)
        {
            ModifyingColumns = true;

            // Find the column index
            int colIdx = -1;
            for (int col = 1; col < ColumnCount - 2; ++col)
            {
                if ((this[col] as FixDescriptionCase).Name.Equals(selectedAcccount))
                {
                    colIdx = col;
                    break;
                }
            }

            if (colIdx == -1)
                return;

            // Find the row index of the last operation
            int rowIdx = -1;
            for (int row = RowCount - 2; row > 1; --row)
            {
                if (this[GetCaseIndex(colIdx, row)] is AmountCase)
                {
                    rowIdx = row;
                    break;
                }
            }

            if (rowIdx == -1)
                return;

            // Delete the whole row
            for (int i = 0; i < ColumnCount; ++i)
                this.RemoveAt(GetCaseIndex(0, rowIdx));


            // Move all rows below => one row up
            foreach (AbstractCase c in this.Where(c => c.Row > rowIdx))
                c.Row--;

            ModifyingColumns = false;
            UpdateAmounts(colIdx);
        }

        /// <summary>
        /// Overriden method : if the collection changes : 
        /// adding an event (if case added) or deleting an event (if case is deleted)
        /// => If an AmountCase or an OkayCase value changes =>  call method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            // If an item changes : call CaseChanged
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (AbstractCase item in e.NewItems)
                    if (item is AmountCase || item is OkayCase)
                        item.PropertyChanged += CaseChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (AbstractCase item in e.OldItems)
                    if (item is AmountCase || item is OkayCase)
                        item.PropertyChanged -= CaseChanged;
            }
        }

        /// <summary>
        /// If the value of an AmountCase or an OkayCase changes => this method is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaseChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ModifyingColumns)
                return;
            
            AmountCase c = sender as AmountCase;
            OkayCase c1 = sender as OkayCase;

            int col = -1;

            if (c == null)
            {
                if (c1 == null) // If the case isn't an OkayCase or AmountCase
                    return;
                else // It's an okay case
                {
                    int colC = ColumnCount - 2;
                    for (int i = 1; i < colC; ++i) // Find the AmountCase in the row (the rest is GrayUnaccessible)
                    {
                        if (this[GetCaseIndex(i, c1.Row)] is AmountCase)
                        {
                            col = i;
                            break;
                        }
                    }
                }
            }
            else // If it's an amount case, we have the column that needs to be updated
            {
                col = c.Column;
            }

            if (col != -1)
                UpdateAmounts(col);
        }

        /// <summary>
        /// Calculates the index of an item in the collection by knowing its column and row
        /// </summary>
        /// <param name="col">The column of the searched case</param>
        /// <param name="row">The row of the searched case</param>
        /// <returns>The index in th    e collection of the searched case</returns>
        private int GetCaseIndex(int col, int row)
        {
            return col + row * ColumnCount;
        }

        /// <summary>
        /// Updates the real and theoretical amount of a column
        /// </summary>
        /// <param name="col">The updated column</param>
        private void UpdateAmounts(int col)
        {
            // It's not an account column
            if (col == 0 || col >= ColumnCount - 2)
                return;


            int colC = ColumnCount;
            int colC2 = colC - 2;
            int rowC = RowCount;

            // Both amounts begin with the initial amount
            decimal initialAmount = (this[GetCaseIndex(col, 1)] as AmountCase).Amount;
            decimal sumReal = initialAmount;
            decimal sumTheo = initialAmount;
            // For every operation row
            for (int i = 2; i < rowC - 2; ++i)
            {
                AmountCase c = this[GetCaseIndex(col, i)] as AmountCase;

                if (c == null)
                    continue;
                decimal caseAmount = c.Amount;

                // Theoretical amount
                sumTheo += caseAmount;

                // Real amount
                OkayCase okayC = this[GetCaseIndex(colC2, i)] as OkayCase;
                if (okayC != null && okayC.IsOkay)
                    sumReal += caseAmount;
            }

            // Update real amount & theoretical amount
            (this[GetCaseIndex(col, rowC - 2)] as ReadonlyAmountCase).Amount = sumReal;
            (this[GetCaseIndex(col, rowC - 1)] as ReadonlyAmountCase).Amount = sumTheo;
        }
    }
}
