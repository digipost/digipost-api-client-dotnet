using System;

namespace Digipost.Api.Client.Domain
{
    public class ClientResponse
    {
        public ClientResponse(MessageDelivery messageDelivery,string xml)
        {
            StatusMessage = messageDelivery.Status.ToString();
            DeliveryTime = messageDelivery.Deliverytime;
            DeliveryMethod = messageDelivery.Deliverymethod.ToString();
            ResponseXml = xml;
            HasErrors = false;
        }

        public ClientResponse(Error error, string xml)
        {

            StatusMessage = error.Errormessage;
            ResponseXml = xml;
            ErrorCode = error.Errorcode;
            ErrorType = error.Errortype;
            HasErrors = true;
        }

        public DateTime DeliveryTime { get; private set; }
        public string DeliveryMethod { get; private set; }
        public string StatusMessage { get; private set; }
        public string ResponseXml { get; private set; }
        public bool HasErrors { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorType { get; private set; }

        public override string ToString()
        {
            return string.Format("StatusMessage[{0}] \n Deliverytime[{1}] \n Deliverytime[{2}]   \n ErrorCode[{3}] \n ErrorType[{4}] \n ResponseMessage[{5}]]", StatusMessage, DeliveryTime, DeliveryMethod, ErrorCode, ErrorType, ResponseXml);
        }
    }
}
