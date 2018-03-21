namespace MatrixClient.DbModel
{
    /// <summary>
    /// Represents a entity capability (Caps)
    /// </summary>
    public class Capability : IId
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Caps Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the Disco information Xml for this entity capability
        /// </summary>
        public string Xml { get; set; }
    }
}
