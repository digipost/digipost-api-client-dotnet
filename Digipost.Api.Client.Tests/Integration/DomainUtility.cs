using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;

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

        public static PrintDetails GetPrintDetails()
        {
            return 
                new PrintDetails(
                    new PrintRecipient("Ola Nordmann", new NorwegianAddress("0115", "Oslo" , "Osloveien 15" )),
                    new PrintReturnRecipient("Returkongen",
                        new NorwegianAddress("5510", "Sophaugen", "Sophauggata 22")));
        }

        public static NorwegianAddress GetNorwegianAddress()
        {
            return new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");
        }

        public static ForeignAddress GetForeignAddress()
        {
            return new ForeignAddress(
                    CountryIdentifier.Country, 
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4");
        }

        public static IPrintRecipient GetPrintRecipientWithNorwegianAddress()
        {
            return new PrintRecipient("name", GetNorwegianAddress());
        }

        public static IPrintReturnRecipient GetPrintReturnRecipientWithNorwegianAddress()
        {
            return new PrintReturnRecipient("name", GetNorwegianAddress());
        }
    }
}