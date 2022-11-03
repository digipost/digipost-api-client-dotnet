using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Send.Actions;
using Xunit;

namespace Digipost.Api.Client.Send.Tests
{
    public class IdentificationActionTests
    {
        public class RequestContentBody
        {
            [Fact]
            public void ReturnsCorrectDataForIdentification()
            {
                //Arrange
                var identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "00000000000"));

                //Act
                var action = new IdentificationAction(identification);
                var content = action.RequestContent;

                //Assert
                var identificationDto = DataTransferObjectConverter.ToDataTransferObject(identification);
                var expected = SerializeUtil.Serialize(identificationDto);
                Assert.Equal(expected, content.InnerXml);
            }
        }
    }
}
