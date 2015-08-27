using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Print
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-details", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-details", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public class PrintDetails : IPrintDetails
    {
        /// <summary>
        ///     Constructor to send physical letter.
        /// </summary>
        public PrintDetails(PrintRecipient recipient, PrintReturnRecipient printReturnRecipient,
            PostType postType = PostType.B,
            PrintColors color = PrintColors.Monochrome,
            NondeliverableHandling nondeliverableHandling = NondeliverableHandling.ReturnToSender)
        {
            PrintReturnRecipient = printReturnRecipient;
            Recipient = recipient;
            Color = color;
            NondeliverableHandling = nondeliverableHandling;
            PostType = postType;
        }

        private PrintDetails()
        {
            /**must exist for serializing**/
        }

        [XmlElement("recipient")]
        public PrintRecipient Recipient { get; set; }

        [XmlElement("return-address")]
        public PrintReturnRecipient PrintReturnRecipient { get; set; }

        [XmlElement("post-type")]
        public PostType PostType { get; set; }

        
        [XmlElement("color")]
        public PrintColors Color { get; set; }

        /// <summary>
        ///     Determines the exception handling that will occur when the letter can not be delivered.
        /// </summary>
        [XmlElement("nondeliverable-handling")]
        private NondeliverableHandling NondeliverableHandling { get; set; }
    }
}