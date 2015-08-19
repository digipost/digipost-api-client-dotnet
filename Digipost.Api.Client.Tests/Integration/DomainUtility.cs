using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identification;

namespace Digipost.Api.Client.Tests.Integration
{
    public class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");

        public static Message GetSimpleMessage()
        {
            var message = new Message(
                new Recipient(IdentificationChoice.PersonalidentificationNumber, "00000000000"),
                new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Hoveddokument.txt"))
                );
            return message;
        }

        public static IIdentification GetPersonalIdentification()
        {
            var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "00000000000");
            return identification;
        }
    }
}