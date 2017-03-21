using Digipost.Api.Client.Common.Recipient;

namespace Digipost.Api.Client.Common.Identify
{
    public class Identification : IIdentification
    {
        public Identification(DigipostRecipient digipostRecipient)
        {
            DigipostRecipient = digipostRecipient;
        }

        public DigipostRecipient DigipostRecipient { get; set; }
    }
}