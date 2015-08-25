using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using IMessage = Digipost.Api.Client.Domain.SendMessage.IMessage;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");

        public static IMessage GetSimpleMessage()
        {
            var message = new Message(
                new Recipient(IdentificationChoiceType.PersonalidentificationNumber, "00000000000"),
                GetDocument()
                );
            return message;
        }

        public static IDocument GetDocument()
        {
            return new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt"));
        }

        public static IIdentification GetPersonalIdentification()
        {
            var identification = new Identification(IdentificationChoiceType.PersonalidentificationNumber, "00000000000");
            return identification;
        }

        public static IRecipient GetRecipientWithDigipostId()
        {
            return new Recipient(IdentificationChoiceType.DigipostAddress, "ola.nordmann#246BB");
        }
    }
}