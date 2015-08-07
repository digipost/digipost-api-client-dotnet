using System;
using System.Collections.Generic;
using System.Security.Policy;
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

        public static AutocompleteSuggestionResults GetAutocompleteSuggestionResults()
        {
            
            var autocompleteSuggestionList = new List<AutocompleteSuggestion>
            {
                GetSuggestion("marit johansen")
            };

            return new AutocompleteSuggestionResults( ){AutcompleteSuggestions = autocompleteSuggestionList};
        }

        public static AutocompleteSuggestion GetSuggestion(string searchString = "marit", string name = "marit johansen")
        {
            return new AutocompleteSuggestion(searchString, new Link("https://qa2.api.digipost.no/relations/search", "https://qa2.api.digipost.no/recipients/search/" + Uri.EscapeUriString(name), "application/vnd.digipost-v6+xml"));
        }
    }
}