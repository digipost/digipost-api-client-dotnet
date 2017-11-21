namespace Digipost.Api.Client.DataTypes
{
    public class Info
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public Info(string title, string text)
        {
            Title = title;
            Text = text;
        }

        internal info AsDataTransferObject()
        {
            return new info()
            {
                title = Title,
                text = Text
            };
        }

    }
}