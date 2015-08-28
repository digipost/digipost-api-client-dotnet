using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class DtoConverterTests
    {
        readonly Comparator _comparator = new Comparator();

        [TestClass]
        public class ToDataTransferObjectMethod : DtoConverterTests
        {
            [TestMethod]
            public void Identification()
            {
                //Arrange
                Identification source = new Identification(IdentificationChoiceType.DigipostAddress, "Ola.Nordmann#244BB2");
                IdentificationDataTransferObject expectedDto = new IdentificationDataTransferObject(IdentificationChoiceType.DigipostAddress, "Ola.Nordmann#244BB2");

                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void RecipientFromNameAndAddress()
            {
                //Arrange
                var recipientByNameAndAddress = new RecipientByNameAndAddress("Ola Nordmann", "0001", "Oslo", "Osloveien 22");
                IRecipient source = new Recipient(recipientByNameAndAddress);
                Domain.RecipientDataTransferObject expectedDto = new Domain.RecipientDataTransferObject(recipientByNameAndAddress);
                
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);
                
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void RecipientFromPrintDetails()
            {
                //Arrange
                IRecipient source = new Recipient(
                    new PrintDetails(
                     new PrintRecipient("Name",
                         new NorwegianAddress(
                            "0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                         new PrintReturnRecipient("Name", new NorwegianAddress(
                            "0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret"))));
                
                var expectedDto = new RecipientDataTransferObject( 
                    new PrintDetailsDataTransferObject(
                     new PrintRecipientDataTransferObject("Name", 
                         new NorwegianAddressDataTransferObject(
                            "0001", "Oslo", "Addr1", "Addr2", "Addr3")),  
                         new PrintReturnRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                            "0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret"))));
                
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void RecipientFromIdentification()
            {
                //Arrange
                IRecipient source = new Recipient(IdentificationChoiceType.DigipostAddress, "ola.nordmann#23FF");
                Domain.RecipientDataTransferObject expectedDto = new Domain.RecipientDataTransferObject(IdentificationChoiceType.DigipostAddress, "ola.nordmann#23FF");
                
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void Document()
            {
                //Arrange
                IDocument source = new Document("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                DocumentDataTransferObject expectedDto = new DocumentDataTransferObject("TestSubject","txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                expectedDto.Guid = source.Guid;

                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void Message()
            {
                //Arrange
                var deliverytime = DateTime.Now.AddDays(3);
                Message source = new Message(
                    new Recipient(
                        IdentificationChoiceType.DigipostAddress,
                        "Ola.Nordmann#34JJ"
                        ),
                    new Document("TestSubject", "txt", new byte[3]), "SenderId")
                {
                    Attachments = new List<IDocument>()
                    {
                        new Document("TestSubject attachment", "txt",  new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = deliverytime
                };

                MessageDataTransferObject expectedDto = new MessageDataTransferObject(
                    new Domain.RecipientDataTransferObject(
                        IdentificationChoiceType.DigipostAddress,
                        "Ola.Nordmann#34JJ"
                        ),
                    new DocumentDataTransferObject("TestSubject", "txt", new byte[3]), "SenderId")
                {
                    Attachments = new List<DocumentDataTransferObject>
                    {
                        new DocumentDataTransferObject("TestSubject attachment", "txt", new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = deliverytime,
                    PrimaryDocumentDataTransferObject = { Guid = source.PrimaryDocument.Guid }
                };


                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ForeignAddress()
            {
                //Arrange
                ForeignAddress source = new ForeignAddress(
                   CountryIdentifier.Country,
                   "NO",
                   "Adresselinje1",
                   "Adresselinje2",
                   "Adresselinje3",
                   "Adresselinje4"
                   );

                ForeignAddressDataTransferObject expectedDto = new ForeignAddressDataTransferObject(
                   CountryIdentifier.Country,
                   "NO",
                   "Adresselinje1",
                   "Adresselinje2",
                   "Adresselinje3",
                   "Adresselinje4"
                   );

                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void NorwegianAddress()
            {
                //Arrange
                NorwegianAddress source = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                NorwegianAddressDataTransferObject expectedDto = new NorwegianAddressDataTransferObject("0001", "Oslo", "Addr1", "Addr2", "Addr3");
                
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void PrintRecipientFromForeignAddress()
            {
                //Arrange
                PrintRecipient source = new PrintRecipient(
                    "Name",
                    new ForeignAddress(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                PrintRecipientDataTransferObject expectedDto = new PrintRecipientDataTransferObject("Name", new ForeignAddressDataTransferObject(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);
                
                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void PrintRecipientFromNorwegianAddress()
            {
                //Arrange
                PrintRecipient source = new PrintRecipient(
                    "Name",
                 new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                PrintRecipientDataTransferObject expectedDto = new PrintRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                        "0001", "Oslo", "Addr1", "Addr2", "Addr3"));
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void PrintReturnRecipientFromForeignAddress()
            {
                //Arrange
                PrintReturnRecipient source = new PrintReturnRecipient(
                    "Name",
                    new ForeignAddress(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                PrintReturnRecipientDataTransferObject expectedDto = new PrintReturnRecipientDataTransferObject("Name", new ForeignAddressDataTransferObject(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void PrintReturnRecipientFromNorwegianAddress()
            {
                //Arrange
                PrintReturnRecipient source = new PrintReturnRecipient(
                    "Name",
                 new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                PrintReturnRecipientDataTransferObject expectedDto = new PrintReturnRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                        "0001", "Oslo", "Addr1", "Addr2", "Addr3"));
                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);

               //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void PrintDetails()
            {
                //Arrange
                PrintDetails source = new PrintDetails(
                    new PrintRecipient("Name", new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                    new PrintReturnRecipient("Name", new NorwegianAddress("0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")));

                var expectedDto = new PrintDetailsDataTransferObject(
                     new PrintRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                        "0001", "Oslo", "Addr1", "Addr2", "Addr3")),  new PrintReturnRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                        "0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")));

                //Act
                var actualDto = DtoConverter.ToDataTransferObject(source);
                
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.AreEqual(0, differences.Count());
                Assert.IsNull(DtoConverter.ToDataTransferObject((IPrintDetails) null));
            }
        }

        [TestClass]
        public class FromDataTransferObjectMethod : DtoConverterTests
        {
            [TestMethod]
            public void IdentificationResultFromPersonalIdentificationNumber()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Digipost;
                source.IdentificationValue = null;
                source.IdentificationResultType = IdentificationResultType.DigipostAddress;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.DigipostAddress, "");

                //Act
                var actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                Assert.AreEqual(source.IdentificationResultType, expected.ResultType);
                Assert.AreEqual("", actual.Data);
                Assert.AreEqual(null, actual.Error);
            }

            [TestMethod]
            public void IdentificationResultFromPersonByNameAndAddress()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Digipost;
                source.IdentificationValue = "jarand.bjarte.t.k.grindheim#8DVE";
                source.IdentificationResultType = IdentificationResultType.DigipostAddress;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.DigipostAddress, "jarand.bjarte.t.k.grindheim#8DVE");

                //Act
                IdentificationResult actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());

                Assert.AreEqual(source.IdentificationValue, actual.Data);
                Assert.AreEqual(source.IdentificationResultType, actual.ResultType);
                Assert.AreEqual(null, actual.Error);
            }

            [TestMethod]
            public void IdentificationResultFromUnknownDigipostAddress()
            {
                //Arrange
                IdentificationResultDataTransferObject source = new IdentificationResultDataTransferObject();
                source.IdentificationResultCode = IdentificationResultCode.Unidentified;
                source.IdentificationValue = "NotFound";
                source.IdentificationResultType = IdentificationResultType.UnidentifiedReason;

                IdentificationResult expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, "NotFound");

                //Act
                var actual = DtoConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());

                Assert.AreEqual(source.IdentificationResultType, actual.ResultType);
                Assert.AreEqual(null, actual.Data);
                Assert.AreEqual(source.IdentificationValue.ToString(),actual.Error.ToString());
            }
        }

    }
}
