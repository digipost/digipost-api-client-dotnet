using System;
using ApiClientShared;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Xunit;

namespace Digipost.Api.Client.Test.Action
{
    public class DigipostActionTests
    {
        public class RequestContentBody
        {
            internal ResourceUtility ResourceUtility;

            public RequestContentBody()
            {
                ResourceUtility = new ResourceUtility("Digipost.Api.Client.Test.Resources");
            }

            [Fact]
            public void ReturnsCorrectDataForMessage()
            {
                //Arrange
                var clientConfig = new ClientConfig("123", Environment.Qa);
                var certificate = TestProperties.Certificate();
                Uri uri = new Uri("http://fakeuri.no");
                var message = DomainUtility.GetSimpleMessageWithRecipientById();

                //Act
                var action = new MessageAction(message, clientConfig, certificate, uri);
                var content = action.RequestContent;

                //Assert
                var expected = SerializeUtil.Serialize(DataTransferObjectConverter.ToDataTransferObject(message));
                Assert.Equal(expected, content.InnerXml);
            }

            [Fact]
            public void ReturnsCorrectDataForIdentification()
            {
                //Arrange
                var clientConfig = new ClientConfig("123", Environment.Qa);
                var certificate = TestProperties.Certificate();
                Uri uri = new Uri("http://fakeuri.no");
                var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));

                //Act
                var action = new IdentificationAction(identification, clientConfig, certificate, uri);
                var content = action.RequestContent;

                //Assert
                var identificationDto = DataTransferObjectConverter.ToDataTransferObject(identification);
                var expected = SerializeUtil.Serialize(identificationDto);
                Assert.Equal(expected, content.InnerXml);
            }
        }
    }
}