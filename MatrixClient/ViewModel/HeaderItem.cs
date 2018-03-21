namespace MatrixClient.ViewModel
{
    using ReactiveUI;

    public class HeaderItem : ReactiveObject, IMenu
    {
        private string text;

        /// <inheritdoc />
        public int GroupId { get; set; }

        /// <inheritdoc />
        public string SortString => $"{this.GroupId.ToString()}";

        /// <inheritdoc />
        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref this.text, value); }
        }   

        /// <inhertitdoc />
        public bool Focusable => false;   
    }
}
