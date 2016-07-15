namespace Digipost.Api.Client.Domain.Identify
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