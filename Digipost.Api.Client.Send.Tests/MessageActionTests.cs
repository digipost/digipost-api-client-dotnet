using System;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.DataTypes.Core;
using Digipost.Api.Client.Send.Actions;
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
                var expected = SerializeUtil.Serialize(message.ToDataTransferObject());
                Assert.Equal(expected, content.InnerXml);
            }

            [Fact]
            public void SerializedXmlContainsDataType()
            {
                ExternalLink externalLink = new ExternalLink(new Uri("https://digipost.no"));

                var message = DomainUtility.GetSimpleMessageWithRecipientById(DomainUtility.GetDocument(externalLink));

                var action = new MessageAction(message);
                var content = action.RequestContent;

                Assert.Contains("<externalLink", content.InnerXml);
            }
        }
    }
}
