namespace MatrixClient.DbModel
{
    /// <summary>
    /// Represents a model for storing user avatars
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Gets or sets the image as byte array
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Gets or sets the hash of the image
        /// </summary>        
        public string ImageHash { get; set; }
    }
}
