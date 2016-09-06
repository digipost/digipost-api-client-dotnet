using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Extensions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Test.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Test.DataTransferObjects
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

                var expectedDto = new nameandaddress
                {
                    fullname = source.FullName,
                    addressline1 = source.AddressLine1,
                    addressline2 = source.AddressLine2,
                    postalcode = source.PostalCode,
                    city = source.City,
                    birthdate = birthDate,
                    phonenumber = source.PhoneNumber,
                    emailaddress = source.Email
                };
                  
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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

                var expectedDto = new identification()
                {
                    ItemElementName = ItemChoiceType.digipostaddress,
                    Item = "ola.nordmann#2233"
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Document()
            {
                //Arrange
                IDocument source = new Document("TestSubject", "txt", new byte[2], AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                var expectedDto = new document()
                {
                    subject = source.Subject,
                    filetype = source.FileType,
                    authenticationlevel = source.AuthenticationLevel.ToAuthenticationLevel(),
                    sensitivitylevel = source.SensitivityLevel.ToSensitivityLevel(),
                    smsnotification = new smsnotification() {afterhours = source.SmsNotification.NotifyAfterHours.ToArray()},
                    uuid = source.Guid
                };
                
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Invoice()
            {
                //Arrange
                var contentBytes = new byte[] {0xb2};
                var smsNotification = new SmsNotification(DateTime.Now);

                var source = new Invoice(
                    "subject", 
                    "txt", 
                    contentBytes, 
                    100,
                    "8902438456", 
                    DateTime.Today, 
                    "123123123",
                    AuthenticationLevel.TwoFactor, 
                    SensitivityLevel.Sensitive, 
                    smsNotification);

                var expectedDto = new
                    invoice()
                {
                    subject = source.Subject,
                    filetype = source.FileType,
                    authenticationlevel = source.AuthenticationLevel.ToAuthenticationLevel(),
                    sensitivitylevel = source.SensitivityLevel.ToSensitivityLevel(),
                    smsnotification = new smsnotification() { afterhours = source.SmsNotification.NotifyAfterHours.ToArray() },
                    uuid = source.Guid,
                    amount = source.Amount,
                    account = source.Account,
                    duedate = source.Duedate
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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
                _comparator.Equal(expectedDto, actualDto, out differences);
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
                expectedDto.recipient.printdetails =
                    DomainUtility.GetPrintDetailsDataTransferObject();

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void MessageWithPrintDetailsAndRecipientByNameAndAddress()
            {
                //Arrange
                var printDetails = DomainUtility.GetPrintDetails();
                var source = new Message(
                    DomainUtility.GetRecipientByNameAndAddress(),
                    new Document("TestSubject", "txt", new byte[3])
                    )
                {
                    SenderId = "SenderId",
                    Attachments = new List<IDocument>
                    {
                        new Document("TestSubject attachment", "txt", new byte[3])
                        {
                            Guid = "attachmentGuid"
                        }
                    },
                    DeliveryTime = DateTime.Today.AddDays(3),
                    PrimaryDocument = { Guid = "attachmentGuidPrimary" }
                };
                source.PrintDetails = printDetails;

                var expectedDto =
                    new message
                    {
                        recipient = new messagerecipient
                        {
                            Item = new nameandaddress
                            {
                                fullname = "Ola Nordmann",
                                postalcode = "0001",
                                city = "Oslo",
                                addressline1 = "Biskop Gunnerus Gate 14"
                            },
                            ItemElementName = ItemChoiceType1.nameandaddress,
                            printdetails = DomainUtility.GetPrintDetailsDataTransferObject()
                        },
                        primarydocument = new document { subject = "PrimaryDocument", filetype = "txt", uuid = "primaryDocumentGuid" },
                        attachment = new[]
                        {
                            new document
                            {
                                subject = "testSubject",
                                filetype = "txt",
                                uuid = "attachmentGuid"
                            }
                        },
                        deliverytime = DateTime.Today.AddDays(3)
                    };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());




            }

            [Fact]
            public void ForeignAddress()
            {
                //Arrange
                var source = new ForeignAddress(
                    CountryIdentifier.Countrycode,
                    "SE",
                    "Adresselinje1",
                    "Adresselinje2",
                    "Adresselinje3",
                    "Adresselinje4"
                    );

                var expectedDto = new foreignaddress
                {
                    ItemElementName = ItemChoiceType2.countrycode,
                    Item = "SE",
                    addressline1 = source.AddressLine1,
                    addressline2 = source.AddressLine2,
                    addressline3 = source.AddressLine3,
                    addressline4 = source.Addressline4,
                };
                    
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void NorwegianAddress()
            {
                //Arrange
                var source = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                var expectedDto = new norwegianaddress()
                {
                    zipcode = source.PostalCode,
                    city = source.City,
                    addressline1 = source.AddressLine1,
                    addressline2 = source.AddressLine2,
                    addressline3 = source.AddressLine3
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

               var expectedDto = new printrecipient()
                {
                    name = source.Name,
                    Item = new norwegianaddress
                    {
                        zipcode = ((NorwegianAddress)source.Address).PostalCode,
                        city = ((NorwegianAddress)source.Address).City,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                    }
                };
                    
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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
                        "NORGE",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                var expectedDto = new printrecipient()
                {
                    name = source.Name,
                    Item = new foreignaddress
                    {
                        ItemElementName = ItemChoiceType2.country,
                        Item = "NORGE",
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                        addressline4 = ((ForeignAddress)source.Address).Addressline4,
                    }
                };
                    
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void PrintReturnRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintReturnRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new printrecipient()
                {
                    name = source.Name,
                    Item = new norwegianaddress
                    {
                        zipcode = ((NorwegianAddress)source.Address).PostalCode,
                        city = ((NorwegianAddress)source.Address).City,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                    }
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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
                        "NORGE",
                        "Adresselinje1",
                        "Adresselinje2",
                        "Adresselinje3",
                        "Adresselinje4"
                        ));

                var expectedDto = new printrecipient()
                {
                    name = source.Name,
                    Item = new foreignaddress
                    {
                        ItemElementName = ItemChoiceType2.country,
                        Item = ((ForeignAddress)source.Address).CountryIdentifierValue,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                        addressline4 = ((ForeignAddress)source.Address).Addressline4,
                    }
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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

                var sourceAddress = source.PrintRecipient.Address;
                var returnAddress = source.PrintReturnRecipient.Address;

                var expectedDto = new printdetails()
                {
                    recipient = new printrecipient()
                    {
                        name = source.PrintRecipient.Name,
                        Item = new norwegianaddress
                        {
                            zipcode = ((NorwegianAddress) sourceAddress).PostalCode,
                            city = ((NorwegianAddress) sourceAddress).City,
                            addressline1 = sourceAddress.AddressLine1,
                            addressline2 = sourceAddress.AddressLine2,
                            addressline3 = sourceAddress.AddressLine3,
                        }
                    },
                    returnaddress = new printrecipient()
                    {
                        name = source.PrintRecipient.Name,
                        Item = new norwegianaddress
                        {
                            zipcode = ((NorwegianAddress) returnAddress).PostalCode,
                            city = ((NorwegianAddress) returnAddress).City,
                            addressline1 = returnAddress.AddressLine1,
                            addressline2 = returnAddress.AddressLine2,
                            addressline3 = returnAddress.AddressLine3,
                        }
                    }
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);

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

                var expectedDto = new smsnotification()
                {
                    afterhours = afterHours.ToArray(),
                    at = atTimes.Select(a => new listedtime { timeSpecified = true, time = a }).ToArray()
                };
                
                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByOrganizationNumber()
            {
                //Arrange
                var source = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "123456789"));
                var expectedDto = new identification()
                {
                    ItemElementName = ItemChoiceType.organisationnumber,
                    Item = ((RecipientById)source.DigipostRecipient).Id
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
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

                var sourceRecipient = ((RecipientByNameAndAddress)source.DigipostRecipient);
                var expectedDto = new identification()
                {
                    ItemElementName = ItemChoiceType.nameandaddress,
                    Item = new nameandaddress()
                    {
                        fullname = sourceRecipient.FullName,
                        addressline1 = sourceRecipient.AddressLine1,
                        addressline2 = sourceRecipient.AddressLine2,
                        birthdate = sourceRecipient.BirthDate.Value,
                        phonenumber = sourceRecipient.PhoneNumber,
                        emailaddress = sourceRecipient.Email,
                    }
                };
                
                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByNameAndAddressAcceptsNoBirthDate()
            {
                Assert.False(true);
            }
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Document()
            {
                //Arrange
                var source = new document()
                {
                    subject = "testSubject",
                    filetype = "txt",
                    authenticationlevel = authenticationlevel.PASSWORD,
                    sensitivitylevel = sensitivitylevel.SENSITIVE,
                    smsnotification = new smsnotification() { afterhours = new []{3}}
                };
                    
                IDocument expected = new Document(source.subject, source.filetype, AuthenticationLevel.Password, SensitivityLevel.Sensitive, new SmsNotification(3));
                expected.Guid = source.uuid;

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);

                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Message()
            {
                //Arrange
                var deliverytime = DateTime.Now.AddDays(3);

                var source = new message()
                {
                    recipient = new messagerecipient()
                    {
                        ItemElementName = ItemChoiceType1.digipostaddress,
                        Item = "Ola.Nordmann#34JJ"
                    },
                    primarydocument = new document()
                    {
                        subject = "TestSubject",
                        filetype = "txt",
                    },
                    attachment = new document[]
                    {
                        new document()
                        {
                            subject = "TestSubject Attachment",
                            filetype = "txt",
                            uuid = "attachmentGuid"

                        }
                    }
                    //Todo: Missing "SenderId". Wassup?? Where to put?
                };
                
                var expected = new Message(
                    new RecipientById(
                        IdentificationType.DigipostAddress, 
                        (string)source.recipient.Item
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
                    PrimaryDocument = {Guid = source.primarydocument.uuid}
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(expected);

                //Assert

                IEnumerable<IDifference> differences;
                _comparator.Equal(source, actual, out differences);

                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void SmsNotification()
            {
                //Arrange
                var atTimes = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(3)};
                var afterHours = new List<int> {4, 5};

                var sourceDto = new smsnotification()
                {
                    afterhours = afterHours.ToArray(),
                    at = atTimes.Select(a => new listedtime { timeSpecified = true, time = a }).ToArray()
                };

                var expected = new SmsNotification();
                expected.NotifyAfterHours.AddRange(afterHours);
                expected.NotifyAtTimes.AddRange(atTimes);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(sourceDto);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);

                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByPinReturnsDigipostResultWithNoneResultType()
            {
                //Arrange
                var source = new identificationresult()
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] {null},
                    //ItemsElementName = new [] { },
                    
                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, string.Empty);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByPinReturnsIdentifiedResultWithNoneResultType()
            {
                //Arrange



                var source = new identificationresult
                {
                    result = identificationresultcode.IDENTIFIED,
                    Items = new object[] { null },
                    //ItemsElementName = new [] { },

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
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
                var source = new identificationresult
                {
                    result = identificationresultcode.INVALID,
                    Items = new object[] { invalidreason.INVALID_PERSONAL_IDENTIFICATION_NUMBER },
                    ItemsElementName = new[] { ItemsChoiceType.invalidreason, },

                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByAddressReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "ola.nordmann#1234";
                var source = new identificationresult()
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] { digipostAddress },
                    ItemsElementName = new[] { ItemsChoiceType.digipostaddress, },
                    
                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
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
                var source = new identificationresult()
                {
                    result = identificationresultcode.IDENTIFIED,
                    Items = new object[] { personAlias },
                    ItemsElementName = new[] { ItemsChoiceType.personalias, },

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = personAlias,
                    //IdentificationResultType = IdentificationResultType.Personalias
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
                var source = new identificationresult
                {
                    result = identificationresultcode.UNIDENTIFIED,
                    Items = new object[] { unidentifiedreason.NOT_FOUND},
                    ItemsElementName = new[] { ItemsChoiceType.unidentifiedreason, },

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }

            [Fact]
            public void IdentificationByAddressReturnsInvalidResultWithInvalidReason()
            {
                // We validate the request with the XSD, so it will fail before the request is sent.
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "bedriften#1234";
                var source = new identificationresult
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] { digipostAddress},
                    ItemsElementName = new[] { ItemsChoiceType.digipostaddress, },

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
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
                var source = new identificationresult
                {
                    result = identificationresultcode.UNIDENTIFIED,
                    Items = new object[] { unidentifiedreason.NOT_FOUND },
                    ItemsElementName = new[] { ItemsChoiceType.unidentifiedreason},

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
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
                var source = new identificationresult
                {
                    result = identificationresultcode.INVALID,
                    Items = new object[] { invalidreason.INVALID_ORGANISATION_NUMBER },
                    ItemsElementName = new[] { ItemsChoiceType.invalidreason, },
                    
                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Assert.Equal(expected.ResultType, actual.ResultType);
                Assert.Equal(expected.Data, actual.Data);
                Assert.Equal(expected.Error, actual.Error);
            }
        }
    }
}