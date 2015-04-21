using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Domain
{
    public class DigipostClientResponse
    {
        public DigipostClientResponse(Messagedelivery messagedelivery,string xml)
        {
            StatusMessage = messagedelivery.Status.ToString();
            DeliveryTime = messagedelivery.Deliverytime;
            ResponseXml = xml;
        }

        public DateTime DeliveryTime { get; set; }
        public string StatusMessage { get; private set; }
        public string ResponseXml { get; private set; }
    }
}
