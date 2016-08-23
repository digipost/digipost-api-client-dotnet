using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-details", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-details", Namespace = "http://api.digipost.no/schema/v6",IsNullable = true)]
    public class PrintDetailsDataTransferObject
    {
        /// <summary>
        ///     Constructor to send physical letter.
        /// </summary>
        public PrintDetailsDataTransferObject(PrintRecipientDataTransferObject printRecipientDataTransferObject, PrintReturnRecipientDataTransferObject printReturnRecipientDataTransferObject,
            PostType postType = PostType.B,
            PrintColors color = PrintColors.Monochrome,
            NondeliverableHandling nondeliverableHandling = NondeliverableHandling.ReturnToSender)
        {
            PrintReturnRecipientDataTransferObject = printReturnRecipientDataTransferObject;
            PrintRecipientDataTransferObject = printRecipientDataTransferObject;
            Color = color;
            NondeliverableHandling = nondeliverableHandling;
            PostType = postType;
        }

        private PrintDetailsDataTransferObject()
        {
            /**must exist for serializing**/
        }

        [XmlElement("recipient")]
        public PrintRecipientDataTransferObject PrintRecipientDataTransferObject { get; set; }

        [XmlElement("return-address")]
        public PrintReturnRecipientDataTransferObject PrintReturnRecipientDataTransferObject { get; set; }

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