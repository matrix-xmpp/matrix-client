namespace MatrixClient
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    using System.Reactive.Linq;
    using System.ComponentModel;
    using System.Windows.Documents;
    using MatrixClient.Xmpp.Logging;
    using ReactiveUI;

    /// <summary>
    /// Interaction logic for Debug.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        XmlLoggerConfiguration loggerConfig;
        public DebugWindow()
        {
            InitializeComponent();

            Observable
               .FromEventPattern<CancelEventHandler, CancelEventArgs>(
                       ce => this.Closing += ce,
                       ce => this.Closing -= ce)                
                .Subscribe(_ => loggerConfig.Enabled = false);

            loggerConfig = ServiceLocator.Current.GetService<XmlLoggerConfiguration>();
            loggerConfig.Enabled = true;
            loggerConfig
                .LogMessages
                //.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                msg =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        flowDocument.Blocks.Add(new Paragraph(new Run(msg)));
                        rtfDebug.ScrollToEnd();
                    });

                    //flowDocument.Blocks.Add(new Paragraph(new Run(msg)));
                    //rtfDebug.ScrollToEnd();
                });            
        }
    }
}
