using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class ComparatorTests
    {
        [TestClass]
        public class AreEqualMethod
        {
            [TestMethod]
            public void GivesErrorOnDeepCompareDifference()
            {
                //Arrange
                MessageDataTransferObject message1;
                MessageDataTransferObject message2;

                // Three differences: 'skuff 4' insted of 'skuff 3, 'danskegatan' instead of 'svenskegatan' and sms notification after 1 hour instead of 2.

                {
                    //Message 1
                    var printDetails = new PrintDetails(new PrintRecipient("Ola Nordmann", new NorwegianAddress("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")), new PrintReturnAddress("Ola Digipost", new ForeignAddress(CountryIdentifier.Country, "SE", "svenskegatan 1", " leil h101", "pb 12", "skuff 3")));

                    var recipient =
                        new RecipientDataTransferObject(
                            new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"), printDetails);

                    var document = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"), AuthenticationLevel.TwoFactor,
                        SensitivityLevel.Sensitive) { Guid = "1222222", SmsNotification = new SmsNotification(2) };

                    message1 = new MessageDataTransferObject(recipient, document);
                }

                {
                    // Message 2
                    var printDetails2 =
                        new PrintDetails(
                            new PrintRecipient("Ola Nordmann",
                                new NorwegianAddress("0460", "Oslo", "Collettsgate 68", "Leil h401", "dør 2")),
                            new PrintReturnAddress("Ola Digipost",
                                new ForeignAddress(CountryIdentifier.Country, "SE", "danskegatan 1", " leil h101",
                                    "pb 12", "skuff 4")));

                    var recipient2 =
                        new RecipientDataTransferObject(
                            new RecipientByNameAndAddress("Ola Nordmann", "0460", "Oslo", "Colletts gate 68"),
                            printDetails2);

                    var document2 = new DocumentDataTransferObject("Subject", "txt", ByteUtility.GetBytes("test"),
                        AuthenticationLevel.TwoFactor,
                        SensitivityLevel.Sensitive) {Guid = "1222222", SmsNotification = new SmsNotification(3)};

                    message2 = new MessageDataTransferObject(recipient2, document2);
                }
                //Act
                Comparator comparator = new Comparator();
                IEnumerable<IDifference> differences = new List<Difference>();
                var result = comparator.AreEqual(message1, message2, out differences);

                //Assert
                Assert.AreEqual(3, differences.Count());
            }

        }

    }
}
