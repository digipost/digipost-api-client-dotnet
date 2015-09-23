namespace Digipost.Api.Client.Domain.Identify
{
    public class Identification : IIdentification
    {
        public DigipostRecipient DigipostRecipient { get; set; }

        public Identification(DigipostRecipient digipostRecipient)
        {
            DigipostRecipient = digipostRecipient;
        }
    }
}
