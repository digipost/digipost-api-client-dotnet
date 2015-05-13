using System;

namespace Digipost.Api.Client.Domain
{
    public class ClientResponse
    {
        public ClientResponse(MessageDeliveryResult messageDeliveryResult,string xml)
        {
            StatusMessage = messageDeliveryResult.Status.ToString();
            DeliveryTime = messageDeliveryResult.Deliverytime;
            DeliveryMethod = messageDeliveryResult.Deliverymethod.ToString();
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
            return string.Format("StatusMessage[{0}] \n DeliveryTime[{1}] \n DeliveryMethod[{2}]   \n ErrorCode[{3}] \n ErrorType[{4}] \n ResponseMessage[{5}]]", StatusMessage, DeliveryTime, DeliveryMethod, ErrorCode, ErrorType, ResponseXml);
        }
    }
}
