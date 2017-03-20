using System;
using Digipost.Api.Client.Domain.SendMessage;
using Xunit;

namespace Digipost.Api.Client.Test.DataTransferObjects
{
    public class MessageTests
    {
        public class ConstructorMethod : MessageTests
        {
            [Fact]
            public void ConstructWithRecipientAndPrimaryDocument()
            {
                //Arrange
                var message = new Message("1010", DomainUtility.GetRecipientByDigipostId(), DomainUtility.GetDocument()
                );

                //Act

                //Assert
                Assert.NotNull(message.PrimaryDocument);
            }

            [Fact]
            public void ConstructWithRecipientByIdAndPrintDetailsMethod()
            {
                //Arrange
                var recipient = DomainUtility.GetRecipientByDigipostId();
                var document = DomainUtility.GetDocument();
                var printDetails = DomainUtility.GetPrintDetails();

                var message = new Message("1010", recipient, document) {PrintDetails = printDetails};

                //Act

                //Assert
                Assert.Equal(recipient, message.DigipostRecipient);
                Assert.Equal(document, message.PrimaryDocument);
                Assert.Equal(printDetails, message.PrintDetails);
            }
        }

        public class DeliveryTimeSpecifiedMethod : MessageTests
        {
            [Fact]
            public void DeliveryTimeNotSpecifiedGivesFalse()
            {
                //Arrange
                var message = new Message("1010", DomainUtility.GetRecipientByDigipostId(), DomainUtility.GetDocument()
                );

                //Act

                //Assert
                Assert.False(message.DeliveryTimeSpecified);
            }

            [Fact]
            public void DeliveryTimeSpecifiedGivesTrue()
            {
                //Arrange
                var message = new Message("1010", DomainUtility.GetRecipientByDigipostId(), DomainUtility.GetDocument()
                ) {DeliveryTime = DateTime.Today};

                //Act

                //Assert
                Assert.True(message.DeliveryTimeSpecified);
            }
        }
    }
}