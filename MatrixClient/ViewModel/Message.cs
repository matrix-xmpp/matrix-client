namespace MatrixClient.ViewModel
{
    using ReactiveUI;
    using System;

    /// <summary>
    /// Represents a incoming or outgoing messgae
    /// </summary>
    public class Message : ReactiveObject
    {
        public Message()
        {            
        }

        public Message(bool autoGenerateId = false) : this()
        {
            // generate a unique Id for this message object
            if (autoGenerateId)
            {
                this.Id = Matrix.Id.GenerateShortGuid();
            }            
        }

        public Message(bool autoGenerateId = false, bool generateNowTimeStamp = false) : this(autoGenerateId)
        {
            // set timestamp to current time
            if (generateNowTimeStamp)
            {
                this.TimeStamp = DateTime.Now;
            }            
        }

        /// <summary>
        /// Unique Id of the messahe
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text of the message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Nickname of the sender
        /// </summary>
        public string Nickname { get; set; }

        //public Jid FromJid { get; set; }

        /// <summary>
        /// Gets or set the timestamp of the message
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets whether or not we are the sender of the message
        /// </summary>
        public bool IsSelf { get; set; }        

        /// <summary>
        /// Gets or sets the Avatar Uri
        /// </summary>
        public string AvatarUri { get; set; }
    }
}
