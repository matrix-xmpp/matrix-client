namespace MatrixClient.ViewModel
{
    /// <summary>
    /// Represents a group chat
    /// </summary>
    public class GroupChatItem : ChatItemBase
    {        
        public GroupChatItem()
        {
            GroupId = 1;
        }

        /// <summary>
        /// Display name for rooms. prepends the hash sign.
        /// </summary>
        public string DisplayName => $"#{this.Text}";        
    }
}
