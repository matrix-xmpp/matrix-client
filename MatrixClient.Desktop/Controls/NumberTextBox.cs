namespace MatrixClient.Controls
{
    using System.Reactive.Linq;
    using System.Windows.Controls;
    using System;
    using System.Windows.Input;
    using System.Windows;
    using System.Text.RegularExpressions;

    public class NumberTextBox : TextBox
    {
        public NumberTextBox()
        {
            Observable
              .FromEventPattern<TextCompositionEventHandler, TextCompositionEventArgs>(
                      tcea => this.PreviewTextInput += tcea,
                      tcea => this.PreviewTextInput -= tcea)
               .Subscribe(tcea => tcea.EventArgs.Handled = !IsTextAllowed(tcea.EventArgs.Text));

            DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(TextBoxPasting));
        }
        private static bool IsTextAllowed(string text)
        {
            //regex that matches disallowed text
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        // Use the DataObject.Pasting Handler 
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
