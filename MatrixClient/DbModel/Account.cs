namespace MatrixClient.DbModel
{
   public class Account : IId
    {
        /// <inheritdoc/>      
        public int Id { get; set; }

        /// <summary>
        /// gets or set the Jabberid of this account
        /// </summary>
        public string Jid { get; set; }
    }
}
