namespace MatrixClient.ViewModel
{
    /// <summary>
    /// Interface for objects which can be displayed in a UI menu
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Gets the display text for the menu item
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a string which is used to sort the menu items
        /// </summary>
        string SortString { get; }

        /// <summary>
        /// Gets or sets a is which is used for grouping menu items. The GroupId also affects the ordering
        /// </summary>
        int GroupId { get; set; }

        /// <summary>
        /// Gets or sets whether this menu item can be focused or not
        /// </summary>
        bool Focusable { get; }
    }
}
