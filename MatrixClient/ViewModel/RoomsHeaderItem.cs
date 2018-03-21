namespace MatrixClient.ViewModel
{
    /// <summary>
    /// Represents a header item for chat rooms
    /// </summary>
    public class RoomsHeaderItem : HeaderItem
    {
        public RoomsHeaderItem()
        {
            Text = "Rooms";
            GroupId = 1;
        }

        public int CountUnread => 0;     
        public bool ShowUnreadCounter => CountUnread > 0;
    }
}
