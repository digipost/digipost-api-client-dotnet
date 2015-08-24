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
                new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt"))
                );
            return message;
        }

        public static IIdentification GetPersonalIdentification()
        {
            var identification = new Identification(IdentificationChoiceType.PersonalidentificationNumber, "00000000000");
            return identification;
        }
    }
}