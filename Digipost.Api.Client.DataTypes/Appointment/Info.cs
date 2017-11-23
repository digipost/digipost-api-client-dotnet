namespace Digipost.Api.Client.DataTypes
{
    public class Info
    {
        public Info(string title, string text)
        {
            Title = title;
            Text = text;
        }

        /// <summary>
        ///     150 characters or less.
        /// </summary>
        public string Title { get; set; }

        public string Text { get; set; }

        internal info AsDataTransferObject()
        {
            return new info
            {
                title = Title,
                text = Text
            };
        }
    }
}
