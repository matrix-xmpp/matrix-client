namespace MatrixClient.Controls
{
    using System.Diagnostics;
    using System.Windows.Documents;

    /// <summary>
    /// Opens <see cref="Hyperlink.NavigateUri"/> in a default system browser
    /// </summary>
    public class DefaultBrowserHyperlink : Hyperlink
    {
        protected override void OnClick()
        {
            base.OnClick();

            new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = this.NavigateUri.AbsoluteUri
                }
            }
            .Start();
        }
    }
}
