using System;
using System.Windows;

namespace D_Accounting
{
    public class ShowMessageBoxEventArgs : EventArgs
    {
        public ShowMessageBoxEventArgs(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            this.resultAction = resultAction;
            this.messageBoxText = messageBoxText;
            this.caption = caption;
            this.button = button;
            this.icon = icon;
            this.defaultResult = defaultResult;
            this.options = options;
        }

        Action<MessageBoxResult> resultAction;
        string messageBoxText;
        string caption;
        MessageBoxButton button;
        MessageBoxImage icon;
        MessageBoxResult defaultResult;
        MessageBoxOptions options;

        public void Show(Window owner)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult, options);
            if (resultAction != null) resultAction(messageBoxResult);
        }

        public void Show()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
            if (resultAction != null) resultAction(messageBoxResult);
        }
    }
}
