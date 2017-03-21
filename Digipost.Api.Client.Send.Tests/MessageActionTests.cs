using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Tests;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class MessageActionTests
    {
        public class RequestContentBody
        {
            [Fact]
            public void ReturnsCorrectDataForMessage()
            {
                //Arrange
                var message = DomainUtility.GetSimpleMessageWithRecipientById();

                //Act
                var action = new MessageAction(message);
                var content = action.RequestContent;

                //Assert
                var expected = SerializeUtil.Serialize(SendDataTransferObjectConverter.ToDataTransferObject(message));
                Assert.Equal(expected, content.InnerXml);
            }
        }
    }
}