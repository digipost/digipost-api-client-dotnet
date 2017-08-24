using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Utilities;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Actions
{
    public class DigipostActionTests
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
