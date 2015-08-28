//using Digipost.Api.Client.Domain.Enums;
//using Digipost.Api.Client.Domain.Print;

//namespace Digipost.Api.Client.Domain.SendMessage
//{
//    public class RecipientDataTransferObject
//    {
//        /// <summary>
//        ///     Preferred digital delivery with fallback to physical delivery.
//        /// </summary>
//        public RecipientDataTransferObject(RecipientByNameAndAddress recipientByNameAndAddress, PrintDetailsDataTransferObject printDetailsDataTransferObject = null)
//            :this(IdentificationChoiceType.NameAndAddress, recipientByNameAndAddress, printDetailsDataTransferObject)
//        {
            
//        }

//        /// <summary>
//        ///     Preferred digital delivery with fallback to physical delivery.
//        /// </summary>
//        public RecipientDataTransferObject(IdentificationChoiceType identificationChoiceType, string id, PrintDetailsDataTransferObject printDetailsDataTransferObject = null)
//            :this(identificationChoiceType, (object) id, printDetailsDataTransferObject)
//        {
//        }

//        private RecipientDataTransferObject(IdentificationChoiceType identificationChoiceType, object value, PrintDetailsDataTransferObject printDetailsDataTransferObject = null)
//        {
//            IdentificationType = identificationChoiceType;
//            IdentificationValue = value;
//            PrintDetailsDataTransferObject = printDetailsDataTransferObject;
//        }

//        /// <summary>
//        ///     Preferred physical delivery (not Digital).
//        /// </summary>
//        public RecipientDataTransferObject(PrintDetailsDataTransferObject printDetailsDataTransferObject)
//        {
//            PrintDetailsDataTransferObject = printDetailsDataTransferObject;
//        }

//        public object IdentificationValue { get; set; }
        
//        public PrintDetailsDataTransferObject PrintDetailsDataTransferObject { get; set; }
        
//        public IdentificationChoiceType? IdentificationType { get; set; }
//    }
//}
