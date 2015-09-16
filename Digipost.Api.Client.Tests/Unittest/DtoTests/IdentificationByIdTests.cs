using System;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Tests.CompareObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class IdentificationByIdTests
    {
        Comparator _comparator = new Comparator();

        [TestClass]
        public class ConstructorMethod : IdentificationByIdTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                IdentificationById identificationById = new IdentificationById(IdentificationType.DigipostAddress, "Digipost-Address");

                //Act

                //Assert
                Assert.AreEqual(IdentificationType.DigipostAddress, identificationById.IdentificationType);
                Assert.AreEqual(IdentificationChoiceType.DigipostAddress, identificationById.IdentificationChoiceType);
                Assert.AreEqual("Digipost-Address", identificationById.Value);
                Assert.AreEqual("Digipost-Address", identificationById.Data);
            }
        }

        [TestClass]
        public class ParseIdentificationChoiceToIdentificationChoiceTypeMethod : IdentificationByIdTests
        {
            [TestMethod]
            public void ParsesAllEnumValues()
            {
                var identificationTypes = Enum.GetValues(typeof (IdentificationType)).Cast<IdentificationType>();

                foreach (var identificationType in identificationTypes)
                {
                    //Arrange
                    IdentificationById identificationById = new IdentificationById(identificationType, "DoesNotMatter");

                    //Act
                    identificationById.ParseIdentificationChoiceToIdentificationChoiceType();

                    //Assert    
                    //Will throw exception in Act if failing.
                }
            } 
        }
    }
}
