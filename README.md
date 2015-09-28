# D_Accounting
## Personal accounting software

### Description

I made this software originally to check my accounts in real-time.
When I pay or withdraw money with my debit card, it only shows up some days ago.
This bothered me when I made a lot of payments in a row, so I wrote this software, which also made me practice my C# and MVVM-Pattern.
This application is only useful if you input every banking transaction, otherwise it will not be up-to-date and so pointless.

### How it works & functionalities

When opening the application, it will try to load the settings file called *settings.xml*.
This file contains the data file path, by default this file is called *d_accounting_data.xml* and is located next to the executable.
If it doesn't find the data file, a new accounting sheet will be showed to the user. Otherwise, it will load the file if it's correct.
This xml file must have the right structure, but the application should totally take care of this part.

#### Accounts

You certainly own different accounts. Create them by typing their name into the text box at the top of the window.
If the name is valid and an account with the same name doesn't exist, the *Add account* button will be enabled and you can create the corresponding account.
If the name is valid and already exists, the *Remove account* button will show up. Click and delete the account and all its linked operations.

#### Operations

In the top drop-down box (or combo box), will be the existing accounts. You can select one and add an operation to it.
Each operation has a date (by default today), an amount, a description and a boolean value that expresses whether your bank has already taken the operation into account or not.

The ending rows of the table *Real amount* and *Theoretical amount* represent respectively the amount on your accounts as your bank sees it and the amount that soon will noticed by your bank.
The check box *OK?* should be checked is you can find the transaction on your bank's website.

All cases are real-time updated (and all impacted fields around it), except the date case, where you need to click somewhere else to check its correctness.

#### Undo/Redo

This application has a undo/redo mechanism. It can be used with the menu bar commands in the *Command* section, or with the usual shortcuts *Ctrl+Z* and *Ctrl+Y*.

These actions are undo- and redoable :
* Adding an account
* Removing an account
* Adding an operation
* Creating a new sheet
* Loading a file

#### Others

On application launch, it will look for the *d_accounting_data.xml* file (by default) and load it.
In the *File* menu bar, you can see the current file. If you press *Save*, it will save the file at this location and overwrite the older file if it exists.
*Save As...* will allow you to change the current file path, just as *Load*, which will set the selected file as the current.
The file path is stored in a settings file next to the executable (just like the default data file). This file is created on application launch if it doesn't exist, and is updated when the application is closed using the *Close* button.

### Technologies & implementation

I tried as much as possible (and it is still possible to do more) to use the Model-View-ViewModel architectural pattern. I didn't use any frameworks (like Prism) for this.
The application is entirely coded in C#, using WPF (and so XAML files for the view).

### TODO

Still, a project is never finished. A lot of things can be done. For example :

* An icon
* Not be able to save (and don't notice when closing app) when nothing has changed.
* Be able to rename and to move an account.
* Really connect the application to your bank. (but this will never happen I guess)
* And so on...

_By David W. (david.wobrock@gmail.com)_
