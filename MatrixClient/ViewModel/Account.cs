namespace MatrixClient.ViewModel
{
    using ReactiveUI;
    using System;
    using System.Security;

    /// <summary>
    /// Viewmodel for the XMPP account settings
    /// </summary>
    public class Account : ReactiveObject
    {        
        SecureString securePassword;

        string xmppId;
        string fullname;
        string nickname;
        string hostname;
        string resource = $"MatriX-{Matrix.Id.GenerateShortGuid()}"; // generate a random resource
        int port = 5222;
        bool specifyHostnameAndPort = false;
        Presence presence;
        byte[] avatarBytes;

        public Account()
        {
            presence = new Presence() { StatusText = "Offline", OnlineStatus = OnlineStatus.Offline };
        }

        private bool CredentialsValid => XmppId?.Length > 0 && SecurePassword?.Length > 0;

        /// <summary>
        /// Gets or sets the XMPP id
        /// </summary>
        public string XmppId
        {
            get { return xmppId; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.xmppId, value);
                this.RaisePropertyChanged(nameof(CredentialsValid));
            }
        }

        /// <summary>
        /// Gest or sets the password
        /// </summary>
        public string Password
        {
            get { return SecurePassword.ToUnsecureString(); }
        }

        /// <summary>
        /// Gest or sets the password
        /// </summary>
        public string Resource
        {
            get { return resource; }
            set { this.RaiseAndSetIfChanged(ref this.resource, value); }
        }
        /// <summary>
        /// Gest or sets the password
        /// </summary>
        public SecureString SecurePassword
        {
            get { return securePassword; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.securePassword, value);
                this.RaisePropertyChanged(nameof(CredentialsValid));
            }
        }

        /// <summary>
        /// Gets or sets the Nickname for this account
        /// </summary>
        public string Nickname
        {
            get { return nickname ?? Username; }
            set { this.RaiseAndSetIfChanged(ref this.nickname, value); }
        }

        /// <summary>
        /// Gets or sets the full name for this account
        /// </summary>
        public string Fullname
        {
            get { return fullname; }
            set { this.RaiseAndSetIfChanged(ref this.fullname, value); }
        }

        /// <summary>
        /// Gets or sets our presence
        /// </summary>
        public Presence Presence
        {
            get { return presence; }
            set { this.RaiseAndSetIfChanged(ref this.presence, value); }
        }        

        /// <summary>
        /// Gets or sets the hostname
        /// </summary>
        public string Hostname
        {
            get { return hostname; }
            set { this.RaiseAndSetIfChanged(ref this.hostname, value); }
        }

        /// <summary>
        /// Gets or sets the port number
        /// </summary>
        public int Port
        {
            get { return port; }
            set { this.RaiseAndSetIfChanged(ref this.port, value); }
        }

        /// <summary>
        /// Gets or sets if the Hostname and Port is set manual
        /// </summary>
        public bool SpecifyHostnameAndPort
        {
            get { return specifyHostnameAndPort; }
            set { this.RaiseAndSetIfChanged(ref this.specifyHostnameAndPort, value); }
        }

        /// <summary>
        /// Gets the username (node part of Xmpp Id)
        /// </summary>
        public string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(XmppId) && XmppId.Contains("@"))
                {
                    return xmppId.Substring(0, XmppId.IndexOf("@"));
                }
                else
                {
                    return Environment.MachineName;
                }                
            }
        }

        /// <summary>
        /// Gets or set the bytes of the avatar image
        /// </summary>
        public byte[] AvatarBytes
        {
            get { return avatarBytes; }
            set { this.RaiseAndSetIfChanged(ref this.avatarBytes, value); }
        }
    }
}
