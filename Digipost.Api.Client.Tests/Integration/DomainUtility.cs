using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Autocomplete;
using Digipost.Api.Client.Domain.Enums;

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

        public static Identification GetPersonalIdentification()
        {
            var identification = new Identification(IdentificationChoice.PersonalidentificationNumber, "00000000000");
            return identification;
        }

        public static AutocompleteSuggestion GetSuggestion()
        {
            return new AutocompleteSuggestion("Marit", new Link("https://qa2.api.digipost.no/relations/search", "https://qa2.api.digipost.no/recipients/search/marit%20johansen", "application/vnd.digipost-v6+xml"));
        }
    }
}