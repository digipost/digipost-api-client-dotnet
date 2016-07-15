using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    public class DataTransferObjectConverterTests
    {
        private readonly Comparator _comparator = new Comparator();

        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void RecipientByNameAndAddress()
            {
                //Arrange
                var birthDate = DateTime.Now;

                var source = new RecipientByNameAndAddress("Ola Nordmann", "Biskop Gunnerus Gate 14", "0001", "Oslo")
                {
                    AddressLine2 = "Etasje 15",
                    BirthDate = birthDate,
                    PhoneNumber = "123456789",
                    Email = "email@test.no"
                };

                var expectedDto = new RecipientDataTransferObject(
                    new RecipientByNameAndAddressDataTranferObject("Ola Nordmann", "0001", "Oslo", "Biskop Gunnerus Gate 14"
                        )
                    {
                        AddressLine2 = "Etasje 15",
                        BirthDate = birthDate,
                        PhoneNumber = "123456789",
                        Email = "email@test.no"
                    });

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void RecipientById()
            {
                //Arrange
                var source = new RecipientById(
                    IdentificationType.DigipostAddress,
                    "ola.nordmann#2233"
                    );

                var expectedDto = new RecipientDataTransferObject(
                    IdentificationChoiceType.DigipostAddress,
                    "ola.nordmann#2233");

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Document()
            {
                //Arrange
                IDocument source = new Document("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                var expectedDto = new DocumentDataTransferObject("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotificationDataTransferObject(3));
                expectedDto.Guid = source.Guid;

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var source = DomainUtility.GetMessageWithBytesAndStaticGuidRecipientById();

                var expectedDto = DomainUtility.GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById();

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void MessageWithPrintDetailsAndRecipientById()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var source = DomainUtility.GetMessageWithBytesAndStaticGuidRecipientById();
                source.PrintDetails = printDetails;

                var expectedDto = DomainUtility.GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientById();
                expectedDto.RecipientDataTransferObject.PrintDetailsDataTransferObject =
                    DomainUtility.GetPrintDetailsDataTransferObject();

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void MessageWithPrintDetailsAndRecipientByNameAndAddress()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var source = DomainUtility.GetMessageWithBytesAndStaticGuidRecipientByNameAndAddress();
                source.PrintDetails = printDetails;

                var expectedDto = DomainUtility.GetMessageDataTransferObjectWithBytesAndStaticGuidRecipientNameAndAddress();
                expectedDto.RecipientDataTransferObject.PrintDetailsDataTransferObject =
                    DomainUtility.GetPrintDetailsDataTransferObject();

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void ForeignAddress()
            {
                //Arrange
                var source = new ForeignAddress(
                    CountryIdentifier.Country,
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4"
                    );

                var expectedDto = new ForeignAddressDataTransferObject(
                    CountryIdentifier.Country,
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4"
                    );

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void NorwegianAddress()
            {
                //Arrange
                var source = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                var expectedDto = new NorwegianAddressDataTransferObject("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintRecipientFromForeignAddress()
            {
                //Arrange
                var source = new PrintRecipient(
                    "Name",
                    new ForeignAddress(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                var expectedDto = new PrintRecipientDataTransferObject("Name", new ForeignAddressDataTransferObject(
                    CountryIdentifier.Country,
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4"
                    ));
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new PrintRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                    "0001", "Oslo", "Addr1", "Addr2", "Addr3"));
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintReturnRecipientFromForeignAddress()
            {
                //Arrange
                var source = new PrintReturnRecipient(
                    "Name",
                    new ForeignAddress(
                        CountryIdentifier.Country,
                        "NO",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                var expectedDto = new PrintReturnRecipientDataTransferObject("Name", new ForeignAddressDataTransferObject(
                    CountryIdentifier.Country,
                    "NO",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4"
                    ));
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintReturnRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintReturnRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new PrintReturnRecipientDataTransferObject("Name", new NorwegianAddressDataTransferObject(
                    "0001", "Oslo", "Addr1", "Addr2", "Addr3"));
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintDetails()
            {
                //Arrange
                var source = new PrintDetails(
                    new PrintRecipient(
                        "Name",
                        new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                    new PrintReturnRecipient(
                        "ReturnName",
                        new NorwegianAddress("0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")));

                var expectedDto = new PrintDetailsDataTransferObject(
                    new PrintRecipientDataTransferObject(
                        "Name",
                        new NorwegianAddressDataTransferObject("0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                    new PrintReturnRecipientDataTransferObject(
                        "ReturnName",
                        new NorwegianAddressDataTransferObject("0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")));

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
                Assert.Null(DataTransferObjectConverter.ToDataTransferObject((IPrintDetails) null));
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var source = new SmsNotification();
                source.NotifyAfterHours.AddRange(afterHours);
                source.NotifyAtTimes.AddRange(atTimes);

                var expectedDto = new SmsNotificationDataTransferObject();
                expectedDto.NotifyAfterHours.AddRange(afterHours);
                expectedDto.NotifyAtTimes.AddRange(atTimes.Select(a => new ListedTimeDataTransferObject(a)));

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            #region Identification

            [Fact]
            public void IdentificationByOrganizationNumber()
            {
                //Arrange
                var source = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "123456789"));
                var expectedDto = new IdentificationDataTransferObject(IdentificationChoiceType.OrganisationNumber, "123456789");

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByNameAndAddress()
            {
                //Arrange
                var source = new Identification(
                    new RecipientByNameAndAddress("Ola Nordmann", "Osloveien 22", "0001", "Oslo")
                    {
                        AddressLine2 = "Adresselinje2",
                        BirthDate = DateTime.Today,
                        PhoneNumber = "123456789",
                        Email = "tull@epost.no"
                    }
                    );

                var expectedDto = new IdentificationDataTransferObject(
                    new RecipientByNameAndAddressDataTranferObject("Ola Nordmann", "0001", "Oslo", "Osloveien 22")
                    {
                        AddressLine2 = "Adresselinje2",
                        BirthDate = DateTime.Today,
                        PhoneNumber = "123456789",
                        Email = "tull@epost.no"
                    }
                    );

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            #endregion
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Document()
            {
                //Arrange
                var source = new DocumentDataTransferObject("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotificationDataTransferObject(3));

                IDocument expected = new Document("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                expected.Guid = source.Guid;

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var deliverytime = DateTime.Now.AddDays(3);

                var sourceDto = new MessageDataTransferObject(
                    new RecipientDataTransferObject(
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
                    DeliveryTime = deliverytime
                };

                var expected = new Message(
                    new RecipientById(
                        IdentificationType.DigipostAddress,
                        "Ola.Nordmann#34JJ"
                        ),
                    new Document("TestSubject", "txt", new byte[3]))
                {
                    SenderId = "SenderId",
                    Attachments = new List<IDocument>
                    {
                        new Document("TestSubject attachment", "txt", new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = deliverytime,
                    PrimaryDocument = {Guid = sourceDto.PrimaryDocumentDataTransferObject.Guid}
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(expected);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(sourceDto, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var sourceDto = new SmsNotificationDataTransferObject();
                sourceDto.NotifyAfterHours.AddRange(afterHours);
                sourceDto.NotifyAtTimes.AddRange(atTimes.Select(a => new ListedTimeDataTransferObject(a)));

                var expected = new SmsNotification();
                expected.NotifyAfterHours.AddRange(afterHours);
                expected.NotifyAtTimes.AddRange(atTimes);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(sourceDto);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.AreEqual(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            #region Identification

            #region Personal identification number

            [Fact]
            public void IdentificationByPinReturnsDigipostResultWithNoneResultType()
            {
                //Arrange
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Digipost,
                    IdentificationValue = null,
                    IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, string.Empty);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);

                Assert.False(true);
            }

            [Fact]
            public void IdentificationByPinReturnsIdentifiedResultWithNoneResultType()
            {
                //Arrange
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Identified,
                    IdentificationValue = null,
                    IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, string.Empty);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByPinReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //This case will never happen because Digipost cannot be used to find PINs in use.
            }

            [Fact]
            public void IdentificationByPinReturnsInvalidResultWithInvalidReason()
            {
                //Arrange
                object invalidValue = InvalidReason.InvalidPersonalIdentificationNumber;
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Invalid,
                    IdentificationValue = invalidValue,
                    IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            #endregion

            #region Address

            [Fact]
            public void IdentificationByAddressReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "ola.nordmann#1234";
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Digipost,
                    IdentificationValue = digipostAddress,
                    IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByAddressReturnsIdentifiedResultWithPersonalAliasResultType()
            {
                //Arrange
                const string personAlias = "fewoinf23nio3255n32oi5n32oi5n#1234";
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Identified,
                    IdentificationValue = personAlias,
                    IdentificationResultType = IdentificationResultType.Personalias
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, personAlias);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByAddressReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //Arrange
                var reason = UnidentifiedReason.NotFound;
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Unidentified,
                    IdentificationValue = reason,
                    IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByAddressReturnsInvalidResultWithInvalidReason()
            {
                // We validate the request with the XSD, so it will fail before the request is sent.
            }

            #endregion

            #region Organization number

            [Fact]
            public void IdentificationByOrganizationNumberReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "bedriften#1234";
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Digipost,
                    IdentificationValue = digipostAddress,
                    IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsIdentifiedResultWithPersonAliasResultType()
            {
                // Will not happen since we do not have a register of organizations that does not have Digipost
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //Arrange
                var reason = UnidentifiedReason.NotFound;
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Unidentified,
                    IdentificationValue = reason,
                    IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsInvalidResultWithInvalidReason()
            {
                //Arrange
                object invalidValue = InvalidReason.InvalidOrganisationNumber;
                var source = new IdentificationResultDataTransferObject
                {
                    IdentificationResultCode = IdentificationResultCode.Invalid,
                    IdentificationValue = invalidValue,
                    IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            #endregion

            #endregion
        }
    }
}