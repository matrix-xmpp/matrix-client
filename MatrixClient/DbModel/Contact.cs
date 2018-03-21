namespace MatrixClient.DbModel
{
    public class Contact : IId
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account id this contact belongs to
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// The JabberId of this contact
        /// </summary>
        public string Jid { get; set; }

        /// <summary>
        /// The Nickname of this contact
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the subscription state
        /// </summary>
        public string Subscription { get; set; }

        /// <summary>
        /// Gets or sets the ask state
        /// </summary>
        public string Ask { get; set; }

        /// <summary>
        /// The Groups assigned to this contect
        /// </summary>
        public string[] Groups { get; set; }
    }
}
