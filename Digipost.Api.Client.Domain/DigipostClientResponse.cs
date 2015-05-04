using System;

namespace Digipost.Api.Client.Domain
{
    public class DigipostClientResponse
    {
        public DigipostClientResponse(MessageDelivery messageDelivery,string xml)
        {
            StatusMessage = messageDelivery.Status.ToString();
            DeliveryTime = messageDelivery.Deliverytime;
            ResponseXml = xml;
            HasErrors = false;
        }

        public DigipostClientResponse(Error error, string xml)
        {

            StatusMessage = error.Errormessage;
            ResponseXml = xml;
            ErrorCode = error.Errorcode;
            ErrorType = error.Errortype;
            HasErrors = true;
        }

        public DateTime DeliveryTime { get; private set; }
        public string StatusMessage { get; private set; }
        public string ResponseXml { get; private set; }
        public bool HasErrors { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorType { get; private set; }

        public override string ToString()
        {
            return string.Format("StatusMessage[{0}] \n Deliverytime[{1}]  \n ErrorCode[{2}] \n ErrorType[{3}] \n ResponseMessage[{4}]]", StatusMessage, DeliveryTime, ErrorCode, ErrorType, ResponseXml);
        }
    }
}
