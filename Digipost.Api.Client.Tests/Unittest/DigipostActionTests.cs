using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
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
            [ExpectedException(typeof(ConfigException), "Should throw exception on empty or null")]
            public void EmptyOrNullBodyThrowsException()
            {
                //Arrange
                ClientConfig clientConfig = new ClientConfig("123");
                X509Certificate2 certificate = TestProperties.Certificate();
                string uri = "AFakeUri";
                var message = new Message(
                        new Recipient(IdentificationChoice.PersonalidentificationNumber, "00000000000"),
                        new Document("Integrasjonstjest", "txt", ResourceUtility.ReadAllBytes(true, "Vedlegg.txt"))
                    );
                
                //Act
                var action = new MessageAction(message, clientConfig, certificate, uri);
                var content = action.RequestContent;
                
            }
        }
    }
}
