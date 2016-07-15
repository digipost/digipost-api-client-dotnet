using System;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Tests.Integration;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    
    public class MessageTests
    {
        
        public class ConstructorMethod : MessageTests
        {
            [Fact]
            public void ConstructWithRecipientAndPrimaryDocument()
            {
                //Arrange
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(), 
                    primaryDocument: DomainUtility.GetDocument()
                    );                

                //Act

                //Assert
                Assert.NotNull(message.PrimaryDocument);
                Assert.Null(message.SenderId);
            }

            [Fact]
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
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(),
                    primaryDocument: DomainUtility.GetDocument()
                    );               
                
                //Act

                //Assert
                Assert.False(message.DeliveryTimeSpecified);
            }

            [Fact]
            public void DeliveryTimeSpecifiedGivesTrue()
            {
                //Arrange
                Message message = new Message(
                    digipostRecipient: DomainUtility.GetRecipientByDigipostId(),
                    primaryDocument: DomainUtility.GetDocument()
                    ) {DeliveryTime = DateTime.Today};

                //Act

                //Assert
                Assert.True(message.DeliveryTimeSpecified);
            }

        }
    }
}
