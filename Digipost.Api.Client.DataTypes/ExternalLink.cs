using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class ExternalLink : IDataType
    {
        public ExternalLink(Uri url)
        {
            Url = url;
        }

        /// <summary>
        ///     Target URL of this link. Must be http or https.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///     Optional deadline for the user to respond. ISO8601 full DateTime.
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        ///     A short, optional text-field, describing the external link.
        ///     70 characters or less.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Optional text which will be displayed on the button.
        ///     30 characters or less.
        /// </summary>
        public string ButtonText { get; set; }

        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }

        internal externalLink AsDataTransferObject()
        {
            var dto = new externalLink
            {
                buttontext = ButtonText,
                description = Description,
                url = Url.AbsoluteUri
            };
            if (Deadline.HasValue)
            {
                dto.deadline = Deadline.Value;
            }
            return dto;
        }

        public override string ToString()
        {
            return $"External link to '{Url}'. Description: '{Description ?? "<none>"}', ButtonText: '{ButtonText ?? "<none>"}', DeadLine: '{(Deadline.HasValue ? Deadline.ToString() : "<none>")}'";
        }
    }
}
