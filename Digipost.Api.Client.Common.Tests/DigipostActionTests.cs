using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Resources.Certificate;
using Digipost.Api.Client.Send;
using Xunit;
using Environment = Digipost.Api.Client.Common.Environment;

namespace Digipost.Api.Client.Tests.Action
{
    public class DigipostActionTests
    {
        public class RequestContentBody
        {
            [Fact]
            public void ReturnsCorrectDataForIdentification()
            {
                //Arrange
                var clientConfig = new ClientConfig(new Broker(123), Environment.Production);
                var certificate = CertificateResource.Certificate();
                var uri = new Uri("http://fakeuri.no");
                var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));

                //Act
                var action = new IdentificationAction(identification, clientConfig, certificate);
                var content = action.RequestContent;

                //Assert
                var identificationDto = DataTransferObjectConverter.ToDataTransferObject(identification);
                var expected = SerializeUtil.Serialize(identificationDto);
                Assert.Equal(expected, content.InnerXml);
            }
        }
    }
}