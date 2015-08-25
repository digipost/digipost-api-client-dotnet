using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class RecipientTests
    {
        readonly Comparator _comparator = new Comparator();

        [TestClass]
        public class ConstructorMethod : RecipientTests
        {
            [TestMethod]
            public void ConstructWithRecipientByNameAndAddress()
            {
                //Arrange

                var recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann","0001", "Oslo", "Osloveien 22");
                var printDetails = DomainUtility.GetPrintDetails();
                Recipient recipient = new Recipient(
                    recipientByNameAndAddress, printDetails);

                //Act

                //Assert
                IEnumerable<IDifference> recipientDifferences;
                _comparator.AreEqual(recipientByNameAndAddress, recipient.IdentificationValue, out recipientDifferences);
                Assert.AreEqual(0,recipientDifferences.Count());

                IEnumerable<IDifference> printDifferences;
                _comparator.AreEqual(printDetails, recipient.PrintDetails, out printDifferences);
                Assert.AreEqual(0, printDifferences.Count());
            }

            [TestMethod]
            public void ConstructWithIdentificationChoiceType()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var recipient = new Recipient(IdentificationChoiceType.DigipostAddress, "ola.nordmann#24HH", printDetails);

                //Act

                //Assert
                Assert.AreEqual("ola.nordmann#24HH", recipient.IdentificationValue as string);
                Assert.AreEqual(IdentificationChoiceType.DigipostAddress, recipient.IdentificationType);
                Assert.IsNotNull(recipient.PrintDetails);

                IEnumerable<IDifference> printDifferences;
                _comparator.AreEqual(printDetails, recipient.PrintDetails, out printDifferences);
                Assert.AreEqual(0, printDifferences.Count());
            }

            [TestMethod]
            public void ConstructWithOnlyPrintDetails()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var recipient = new Recipient(printDetails);

                //Act

                //Assert
                IEnumerable<IDifference> printDifferences;
                _comparator.AreEqual(printDetails, recipient.PrintDetails, out printDifferences);
                Assert.AreEqual(0, printDifferences.Count());
            }
        }

        
    }
}
