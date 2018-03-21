namespace MatrixClient
{    
    using MatrixClient.ViewModel;
    using MatrixClient.Xmpp;

    using Microsoft.Extensions.DependencyInjection;    
    using Microsoft.Win32;

    using System;
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using System.Windows.Media;
    
    using ReactiveUI;    

    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl
    {
        public Account Account { get; }
        public MyViewModel MyViewModel { get; }
        public XmppClientEx XmppClientEx { get; }


        public ReactiveCommand SelectProfileImageCommand { get; }
        public ReactiveCommand PublishProfileDataCommand { get; }

        public ProfilePage()
        {
            this.Account = ServiceLocator.Current.GetService<Account>();
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();
            this.XmppClientEx = ServiceLocator.Current.GetService<XmppClientEx>();

            SelectProfileImageCommand = ReactiveCommand.Create(() =>
               {
                   // Wrap the creation of the OpenFileDialog instance in a using statement,
                   // rather than manually calling the Dispose method to ensure proper disposal
                   OpenFileDialog dlg = new OpenFileDialog();

                   dlg.Title = "Open Image";
                   dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png,*.gif)|*.BMP;*.JPG;*.JPEG;*.PNG;*.GIF";

                   if (dlg.ShowDialog() == true)
                   {
                       imgProfile.Source = CreateResizedImage(new BitmapImage(new Uri(dlg.FileName)), 250);
                       // set the tag to null, because we have a custom image now
                       imgProfile.Tag = null;
                   }
               }              
            );


            PublishProfileDataCommand = ReactiveCommand.Create(async () =>
                {
                    byte[] imageBytes = null;
                    if ((string)imgProfile.Tag != "default")
                    {
                        imageBytes = GetJpgBytesFromBitmapSource(imgProfile.Source as BitmapSource);
                    }
                    
                    //await PublishVCard(imageBytes);

                    if (imageBytes != null)
                    {
                        var publishAvatarDataIq = AvatarManager.CreatePublishAvatarDataStanza(this.GetJpgBytesFromBitmapSource(imgProfile.Source as BitmapSource));
                        var res1 = await this.XmppClientEx.SendIqAsync(publishAvatarDataIq);

                        var publishAvatarMetadataIq = AvatarManager.CreatePublishAvatarMetadataStanza(imageBytes, (int)imgProfile.Source.Height, (int)imgProfile.Source.Height, "image/jpg");
                        var res2 = await this.XmppClientEx.SendIqAsync(publishAvatarMetadataIq);

                        var publishNickIq = NicknameManager.CreatePublishNicknameStanza(txtNickname.Text);
                        var res3 = await this.XmppClientEx.SendIqAsync(publishNickIq);
                    }
                    

                }
                , MyViewModel.IsConnectedObervalble
            );

            InitializeComponent();
        }      

        private static ImageSource CreateResizedImage(ImageSource source, int max)
        {
            // from:
            // https://stackoverflow.com/questions/15779564/resize-image-in-wpf
            if (Math.Max(source.Width, source.Height) <= max)
            {
                // no resize required
                return source;
            }                

            // we allow the max of 200 in either width or height
            var ratioX = (double)max / source.Width;
            var ratioY = (double)max / source.Height;

            var ratio = Math.Min(ratioX, ratioY);

            var width = (int)(source.Width * ratio);
            var height = (int)(source.Height * ratio);

            var rect = new System.Windows.Rect(0, 0, width, height);

            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);

            var resizedImage = new RenderTargetBitmap(
                width, height,         // Resized dimensions
                96, 96,                // Default DPI values
                PixelFormats.Default); // Default pixel format
            resizedImage.Render(drawingVisual);

            return BitmapFrame.Create(resizedImage);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((string) imgProfile.Tag != "default")
            { 
                var bytes = GetJpgBytesFromBitmapSource(imgProfile.Source as BitmapSource);
                Account.AvatarBytes = bytes;
            }
        }

        private byte[] GetJpgBytesFromBitmapSource(BitmapSource image)
        {           
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            using (var ms = new MemoryStream())
            { 
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.QualityLevel = 95;
                encoder.Save(ms);
            
                return ms.ToArray();
            }
        }


        //private async Task PublishVCard(byte[] imagebytes)
        //{
        //    var viq = new VcardIq { Type = IqType.Set };

        //    var vcard = viq.Vcard;

        //    if (!String.IsNullOrEmpty(txtFullname.Text))
        //        vcard.Fullname = txtFullname.Text;

        //    if (!String.IsNullOrEmpty(txtNickname.Text))
        //        vcard.Nickname = txtNickname.Text;

        //    if (imagebytes != null)
        //    {
        //        var photo = new Photo { Type = "image/jpg", Binval = imagebytes };
        //        photo.SetTag("TYPE", "image/jpg");

        //        vcard.Photo = photo;
        //    }

        //    var res = await this.XmppClientEx.SendIqAsync(viq);
        //    if (res.Type == IqType.Result)
        //    {

        //    }
        //    else if (res.Type == IqType.Error)
        //    {

        //    }
        //    // send it now
        //}
    }
}
