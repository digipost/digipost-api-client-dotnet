using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class DigipostActionTests
    {

        [TestClass]
        public class RequestContentBody
        {
            internal ResourceUtility ResourceUtility;

            [TestInitialize]
            public void TestInit()
            {
                ResourceUtility = new ResourceUtility("Digipost.Api.Client.Tests.Resources");
            }

            [TestMethod]
            public void ReturnsCorrectDataForMessage()
            {
                //Arrange
                var clientConfig = new ClientConfig("123");
                var certificate = TestProperties.Certificate();
                const string uri = "AFakeUri";
                var message = DomainUtility.GetSimpleMessage();

                //Act
                var action = new MessageAction(message, clientConfig, certificate, uri);
                var content = action.RequestContent;

                //Assert
                var expected = SerializeUtil.Serialize(DataTransferObjectConverter.ToDataTransferObject(message));
                Assert.AreEqual(expected, content.InnerXml);
            }

            [TestMethod]
            public void ReturnsCorrectDataForIdentification()
            {
                //Arrange
                var clientConfig = new ClientConfig("123");
                var certificate = TestProperties.Certificate();
                const string uri = "AFakeUri";
                var identification = new Identification(IdentificationChoiceType.PersonalidentificationNumber, "00000000000");

                //Act
                var action = new IdentificationAction(identification, clientConfig, certificate, uri);
                var content = action.RequestContent;

                //Assert
                IdentificationDataTransferObject identificationDto = DataTransferObjectConverter.ToDataTransferObject(identification);
                var expected = SerializeUtil.Serialize(identificationDto);
                Assert.AreEqual(expected, content.InnerXml);
            }
        }
    }
}
