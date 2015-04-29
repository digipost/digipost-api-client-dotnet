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
    public class PrintDetails
    {
        /// <summary>
        ///     Constructor to send physical mail.
        /// </summary>
        public PrintDetails(PrintRecipient recipient, PrintRecipient returnAddress, PostType postType = PostType.B,
            PrintColors color = PrintColors.Monochrome,
            NondeliverableHandling nondeliverableHandling = NondeliverableHandling.ReturnToSender)
        {
            ReturnAddress = returnAddress;
            Recipient = recipient;
            Color = color;
            NondeliverableHandling = nondeliverableHandling;
            PostType = postType;
        }

        private PrintDetails()
        {
            /**must exist for serializing**/
        }

        /// <summary>
        ///     Constructor to send physical mail, where the letter should be destroyed if the letter can not be delivered.
        /// </summary>
        public PrintDetails(PrintRecipient printRecipient, PostType postType = PostType.B,
            PrintColors printColors = PrintColors.Monochrome)
        {
            Recipient = printRecipient;
            NondeliverableHandling = NondeliverableHandling.Shred;
            PostType = postType;
            Color = printColors;
        }

        /// <summary>
        ///     The recipient of the physical mail.
        /// </summary>
        [XmlElement("recipient")]
        public PrintRecipient Recipient { get; set; }

        /// <summary>
        ///     The return address of the physical mail. (if nondeliverable AND the nondeliverable-handling is set to
        ///     ReturnToSender)
        /// </summary>
        [XmlElement("return-address")]
        public PrintRecipient ReturnAddress { get; set; }

        /// <summary>
        ///     Defines how fast you want the item delivered. Note: additional charges may apply
        /// </summary>
        [XmlElement("post-type")]
        public PostType PostType { get; set; }

        /// <summary>
        ///     Defines if you want the documents printed in black / white or color (Note: additional charges may apply).
        /// </summary>
        [XmlElement("color")]
        public PrintColors Color { get; set; }

        /// <summary>
        ///     Determines the exception handling that will occur when the letter can not be delivered.
        /// </summary>
        [XmlElement("nondeliverable-handling")]
        public NondeliverableHandling NondeliverableHandling { get; set; }
    }
}