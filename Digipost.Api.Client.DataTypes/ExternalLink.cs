using System;

namespace Digipost.Api.Client.DataTypes
{
    public class ExternalLink : DataType
    {
        /// <summary>
        /// Target URL of this link. Must be http or https.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Optional deadline for the user to respond. ISO8601 full DateTime.
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// A short, optional text-field, describing the external link.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional text which will be displayed on the button.
        /// </summary>
        public string ButtonText { get; set; }

        public ExternalLink(Uri url)
        {
            Url = url;
        }

        protected override object AsDataTransferObject()
        {
            var dto = new externalLink()
            {
                buttontext = ButtonText,
                description = Description,
                url = Url.AbsoluteUri
            };
            if (Deadline.HasValue)
            {
                dto.deadline = Deadline.Value;
                dto.deadlineSpecified = true;
            }
            return dto;
        }

    }
}
