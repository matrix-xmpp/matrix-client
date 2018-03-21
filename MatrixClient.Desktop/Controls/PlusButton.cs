namespace MatrixClient.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The Cross Button is a very simple version of the button that displays as a discrete cross,
    /// similar to the buttons at the top of Google Chrome's tabs.
    /// </summary>
    public class PlusButton : Button
    {
        /// <summary>
        /// Initializes the <see cref="CrossButton"/> class.
        /// </summary>
        static PlusButton()
        {
            //  Set the style key, so that our control template is used.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlusButton), new FrameworkPropertyMetadata(typeof(PlusButton)));
        }
    }
}
