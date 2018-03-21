namespace MatrixClient.DbModel
{
    public class UserAvatar
    {      
        /// <summary>
        /// gets or set the Jabberid
        /// </summary>
        public string Jid { get; set; }

        /// <summary>
        /// Gets or sets the hash of the image
        /// </summary>        
        public string Hash { get; set; }
    }
}
