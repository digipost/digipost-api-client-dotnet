using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Test.CompareObjects
{
    public class ComparatorTests
    {
        public class EqualMethod
        {
            [Fact]
            public void GivesErrorOnDeepCompareDifference()
            {
                //Arrange
                MessageDataTransferObject message1;
                MessageDataTransferObject message2;

                // Three differences: 'skuff 4' insted of 'skuff 3, 'danskegatan' instead of 'svenskegatan' and sms notification after 1 hour instead of 2.

                {
                    //Message 1
                    var printDetails = new PrintDetailsDataTransferObject(new PrintRecipientDataTransferObject("Ola Nordmann", new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnRecipientDataTransferObject("Ola Digipost", new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));

                    var recipient =
                        new RecipientDataTransferObject(
                            new RecipientByNameAndAddressDataTranferObject("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);

                    var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                        SensitivityLevel.Sensitive)
                    {Guid = "1222222", SmsNotification = new SmsNotificationDataTransferObject(2)};

                    message1 = new MessageDataTransferObject(recipient, document);
                }

                {
                    // Message 2
                    var printDetails2 =
                        new PrintDetailsDataTransferObject(
                            new PrintRecipientDataTransferObject("Ola Nordmann",
                                new NorwegianAddressDataTransferObject("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")),
                            new PrintReturnRecipientDataTransferObject("Ola Digipost",
                                new ForeignAddressDataTransferObject(CountryIdentifier.Country, "SE", "danskegatan 1", " leil h101",
                                    "pb 12", "skuff 4")));

                    var recipient2 =
                        new RecipientDataTransferObject(
                            new RecipientByNameAndAddressDataTranferObject("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"),
                            printDetails2);

                    var document2 = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"),
                        AuthenticationLevel.TwoFactor,
                        SensitivityLevel.Sensitive)
                    {Guid = "1222222", SmsNotification = new SmsNotificationDataTransferObject(3)};

                    message2 = new MessageDataTransferObject(recipient2, document2);
                }
                //Act
                var comparator = new Comparator();
                IEnumerable<IDifference> differences = new List<Difference>();
                var result = comparator.Equal(message1, message2, out differences);

                //Assert
                Assert.Equal(3, differences.Count());
            }
        }
    }
}