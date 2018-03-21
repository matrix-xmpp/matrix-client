namespace MatrixClient.Controls
{
    using System.Reactive.Linq;
    using System.Windows.Controls;
    using System;
    using System.Windows.Input;
    using System.Windows;
  
    public class ChatTextBox : TextBox
    {
        public static DependencyProperty CommandProperty
               = DependencyProperty.Register(
                   "Command",
                   typeof(ICommand),
                   typeof(ChatTextBox));

        public static DependencyProperty CommandParameterProperty
               = DependencyProperty.Register(
                   "CommandParameter",
                   typeof(object),
                   typeof(ChatTextBox));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public ChatTextBox()
        {
            Observable
               .FromEventPattern<KeyEventHandler, KeyEventArgs>(
                       kea => this.PreviewKeyDown += kea,
                       kea => this.PreviewKeyDown -= kea)
                .Subscribe(kea =>
                {
                    // Enter key is routed and the PreviewKeyDown is also fired with the 
                    // Enter key

                    // You don't want to clear the box when CTRL and/or SHIFT is down
                    if (kea.EventArgs.Key == Key.Enter &&
                        !(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                        !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                    {
                        Command.Execute(CommandParameter);                   
                        kea.EventArgs.Handled = true;
                    }
                });
        }
    }
}
