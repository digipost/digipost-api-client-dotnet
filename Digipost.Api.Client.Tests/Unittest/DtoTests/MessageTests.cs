using System;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class MessageTests
    {
        [TestClass]
        public class ConstructorMethod : MessageTests
        {
            [TestMethod]
            public void ConstructWithRecipientAndPrimaryDocument()
            {
                //Arrange
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(), 
                    primaryDocument: DomainUtility.GetDocument()
                    );                

                //Act

                //Assert
                Assert.IsNotNull(message.PrimaryDocument);
                Assert.IsNull(message.SenderId);
            }

            [TestMethod]
            public void ConstructWithRecipientByIdAndPrintDetailsMethod()
            {
                //Arrange
                var recipient = DomainUtility.GetRecipientByDigipostId();
                var document = DomainUtility.GetDocument();
                var printDetails = DomainUtility.GetPrintDetails();
                
                Message message = new Message(
                    digipostRecipient: recipient, 
                    primaryDocument: document
                    );
                message.PrintDetails = printDetails;

                //Act

                //Assert
                Assert.AreEqual(recipient, message.DigipostRecipient);
                Assert.AreEqual(document, message.PrimaryDocument);
                Assert.AreEqual(printDetails, message.PrintDetails);
            }
        }

        [TestClass]
        public class DeliveryTimeSpecifiedMethod : MessageTests
        {
            [TestMethod]
            public void DeliveryTimeNotSpecifiedGivesFalse()
            {
                //Arrange
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(),
                    primaryDocument: DomainUtility.GetDocument()
                    );               
                
                //Act

                //Assert
                Assert.IsFalse(message.DeliveryTimeSpecified);
            }

            [TestMethod]
            public void DeliveryTimeSpecifiedGivesTrue()
            {
                //Arrange
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(),
                    primaryDocument: DomainUtility.GetDocument()
                    ) {DeliveryTime = DateTime.Today};

                //Act

                //Assert
                Assert.IsTrue(message.DeliveryTimeSpecified);
            }

        }
    }
}
