using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Extensions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Tests.CompareObjects;
using V8;
using Xunit;
using Identification = Digipost.Api.Client.Common.Identify.Identification;

namespace Digipost.Api.Client.Common.Tests
{
    public class DataTransferObjectConverterTests
    {
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
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

                var expectedDto = new V8.Foreign_Address()
                {
                    Country_Code = "SE",
                    Addressline1 = source.AddressLine1,
                    Addressline2 = source.AddressLine2,
                    Addressline3 = source.AddressLine3,
                    Addressline4 = source.AddressLine4
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void IdentificationByAddressReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "ola.nordmann#1234";
                var source = new V8.Identification_Result()
                {
                    Result = Identification_Result_Code.DIGIPOST,
                    Digipost_Address = digipostAddress

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByAddressReturnsIdentifiedResultWithPersonalAliasResultType()
            {
                //Arrange
                const string personAlias = "fewoinf23nio3255n32oi5n32oi5n#1234";
                var source = new V8.Identification_Result()
                {
                    Result = Identification_Result_Code.IDENTIFIED,
                    Person_Alias = personAlias

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = personAlias,
                    //IdentificationResultType = IdentificationResultType.Personalias
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, personAlias);

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByAddressReturnsInvalidResultWithInvalidReason()
            {
                // We validate the request with the XSD, so it will fail before the request is sent.
            }

            [Fact]
            public void IdentificationByAddressReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //Arrange
                var reason = V8.Unidentified_Reason.NOT_FOUND;
                var source = new V8.Identification_Result()
                {
                    Result = Identification_Result_Code.UNIDENTIFIED,
                    Unidentified_Reason = V8.Unidentified_Reason.NOT_FOUND,
                    Unidentified_ReasonSpecified = true

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToIdentificationError());

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
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

                var sourceRecipient = (RecipientByNameAndAddress) source.DigipostRecipient;

                var expectedDto = new V8.Identification()
                {
                    Name_And_Address =  new Name_And_Address()
                    {
                        Fullname = sourceRecipient.FullName,
                        Addressline1 = sourceRecipient.AddressLine1,
                        Addressline2 = sourceRecipient.AddressLine2,
                        Postalcode = sourceRecipient.PostalCode,
                        City = sourceRecipient.City,
                        Birth_Date = sourceRecipient.BirthDate.Value,
                        Birth_DateSpecified = true,
                        Phone_Number = sourceRecipient.PhoneNumber,
                        Email_Address = sourceRecipient.Email
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void IdentificationByNameAndAddressAcceptsNoBirthDate()
            {
                //Arrange
                var source = new Identification(
                    new RecipientByNameAndAddress("Ola Nordmann", "Osloveien 22", "0001", "Oslo")
                    {
                        AddressLine2 = "Adresselinje2",
                        PhoneNumber = "123456789",
                        Email = "tull@epost.no"
                    }
                );

                var sourceRecipient = (RecipientByNameAndAddress) source.DigipostRecipient;

                var expectedDto = new V8.Identification()
                {
                    Name_And_Address = new Name_And_Address()
                    {
                        Fullname = sourceRecipient.FullName,
                        Addressline1 = sourceRecipient.AddressLine1,
                        Addressline2 = sourceRecipient.AddressLine2,
                        Postalcode = sourceRecipient.PostalCode,
                        City = sourceRecipient.City,
                        Birth_DateSpecified = false,
                        Phone_Number = sourceRecipient.PhoneNumber,
                        Email_Address = sourceRecipient.Email
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void IdentificationByOrganizationNumber()
            {
                //Arrange
                var source = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "123456789"));
                var expectedDto = new V8.Identification()
                {
                    Organisation_Number = ((RecipientById) source.DigipostRecipient).Id
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "bedriften#1234";
                var source = new V8.Identification_Result()
                {
                    Result = Identification_Result_Code.DIGIPOST,
                    Digipost_Address = digipostAddress

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsIdentifiedResultWithPersonAliasResultType()
            {
                // Will not happen since we do not have a register of organizations that does not have Digipost
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsInvalidResultWithInvalidReason()
            {
                //Arrange
                var invalidValue = Invalid_Reason.INVALID_ORGANISATION_NUMBER;
                var source = new Identification_Result()
                {
                    Result = Identification_Result_Code.INVALID,
                    Invalid_Reason = (Invalid_Reason) invalidValue,
                    Invalid_ReasonSpecified = true

                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToIdentificationError());

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //Arrange
                var reason = Unidentified_Reason.NOT_FOUND;
                var source = new Identification_Result()
                {
                    Result = Identification_Result_Code.UNIDENTIFIED,
                    Unidentified_Reason = reason,
                    Unidentified_ReasonSpecified = true

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToIdentificationError());

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByPinReturnsDigipostResultWithNoneResultType()
            {
                //Arrange
                var source = new Identification_Result()
                {
                    Result = Identification_Result_Code.DIGIPOST,
                    //ItemsElementName = new [] { },

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, string.Empty);

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByPinReturnsIdentifiedResultWithNoneResultType()
            {
                //Arrange

                var source = new Identification_Result()
                {
                    Result = Identification_Result_Code.IDENTIFIED,

                    //ItemsElementName = new [] { },

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, string.Empty);

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByPinReturnsInvalidResultWithInvalidReason()
            {
                //Arrange
                var invalidValue = Invalid_Reason.INVALID_PERSONAL_IDENTIFICATION_NUMBER;
                var source = new Identification_Result()
                {
                    Result = Identification_Result_Code.INVALID,
                    Invalid_Reason = (Invalid_Reason) invalidValue,
                    Invalid_ReasonSpecified = true

                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToIdentificationError());

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void IdentificationByPinReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //This case will never happen because Digipost cannot be used to find PINs in use.
            }

            [Fact]
            public void NorwegianAddress()
            {
                //Arrange
                var source = new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3");

                var expectedDto = new Norwegian_Address()
                {
                    Zip_Code = source.PostalCode,
                    City = source.City,
                    Addressline1 = source.AddressLine1,
                    Addressline2 = source.AddressLine2,
                    Addressline3 = source.AddressLine3
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
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

                List<PrintInstruction> printinstruction = new List<PrintInstruction>();
                printinstruction.Add(new PrintInstruction("test", "testing"));
                source.PrintInstructions = new PrintInstructions(printinstruction);

                var sourceAddress = source.PrintRecipient.Address;
                var returnAddress = source.PrintReturnRecipient.Address;

                var expectedDto = new Print_Details()
                {
                    Recipient = new Print_Recipient()
                    {
                        Name = source.PrintRecipient.Name,
                        Norwegian_Address = new Norwegian_Address()
                        {
                            Zip_Code = ((NorwegianAddress) sourceAddress).PostalCode,
                            City = ((NorwegianAddress) sourceAddress).City,
                            Addressline1 = sourceAddress.AddressLine1,
                            Addressline2 = sourceAddress.AddressLine2,
                            Addressline3 = sourceAddress.AddressLine3
                        }
                    },
                    Return_Address = new Print_Recipient()
                    {
                        Name = source.PrintReturnRecipient.Name,
                        Norwegian_Address = new Norwegian_Address()
                        {
                            Zip_Code = ((NorwegianAddress) returnAddress).PostalCode,
                            City = ((NorwegianAddress) returnAddress).City,
                            Addressline1 = returnAddress.AddressLine1,
                            Addressline2 = returnAddress.AddressLine2,
                            Addressline3 = returnAddress.AddressLine3
                        }
                    }
                };
                expectedDto.Print_Instructions.Add(new Print_Instruction(){Key = "test", Value = "testing"});


                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);

                Assert.Null(DataTransferObjectConverter.ToDataTransferObject((IPrintDetails) null));
            }

            [Fact]
            public void PrintIfUnread()
            {
                //Arrange
                DateTime printifunreadafter = DateTime.Now.AddDays(3);
                PrintDetails printDetails = new PrintDetails(
                    new PrintRecipient(
                        "Name",
                        new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                    new PrintReturnRecipient(
                        "ReturnName",
                        new NorwegianAddress("0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")));

                var source = new PrintIfUnread(printifunreadafter, printDetails);
                var sourceAddress = source.PrintDetails.PrintRecipient.Address;
                var returnAddress = source.PrintDetails.PrintReturnRecipient.Address;

                var expectedDtoPrintDetails = new V8.Print_Details()
                {
                    Recipient = new Print_Recipient()
                    {
                        Name = source.PrintDetails.PrintRecipient.Name,
                        Norwegian_Address = new Norwegian_Address()
                        {
                            Zip_Code = ((NorwegianAddress) sourceAddress).PostalCode,
                            City = ((NorwegianAddress) sourceAddress).City,
                            Addressline1 = sourceAddress.AddressLine1,
                            Addressline2 = sourceAddress.AddressLine2,
                            Addressline3 = sourceAddress.AddressLine3
                        }
                    },
                    Return_Address = new Print_Recipient()
                    {
                        Name = source.PrintDetails.PrintReturnRecipient.Name,
                        Norwegian_Address = new Norwegian_Address()
                        {
                            Zip_Code = ((NorwegianAddress) returnAddress).PostalCode,
                            City = ((NorwegianAddress) returnAddress).City,
                            Addressline1 = returnAddress.AddressLine1,
                            Addressline2 = returnAddress.AddressLine2,
                            Addressline3 = returnAddress.AddressLine3
                        }
                    }
                };

                var expectedDto = new V8.Print_If_Unread()
                {
                    Print_If_Unread_After = printifunreadafter,
                    Print_Details = expectedDtoPrintDetails
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);

                Assert.Null(DataTransferObjectConverter.ToDataTransferObject((IPrintIfUnread) null));
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

                var expectedDto = new V8.Print_Recipient()
                {
                    Name = source.Name,
                    Foreign_Address = new Foreign_Address()
                    {
                        Country = "NORGE",
                        Addressline1 = source.Address.AddressLine1,
                        Addressline2 = source.Address.AddressLine2,
                        Addressline3 = source.Address.AddressLine3,
                        Addressline4 = ((ForeignAddress) source.Address).AddressLine4
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void PrintRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new V8.Print_Recipient()
                {
                    Name = source.Name,
                    Norwegian_Address = new Norwegian_Address()
                    {
                        Zip_Code = ((NorwegianAddress) source.Address).PostalCode,
                        City = ((NorwegianAddress) source.Address).City,
                        Addressline1 = source.Address.AddressLine1,
                        Addressline2 = source.Address.AddressLine2,
                        Addressline3 = source.Address.AddressLine3
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
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

                var expectedDto = new V8.Print_Recipient()
                {
                    Name = source.Name,
                    Foreign_Address = new V8.Foreign_Address()
                    {
                        Country = ((ForeignAddress) source.Address).CountryIdentifierValue,
                        Addressline1 = source.Address.AddressLine1,
                        Addressline2 = source.Address.AddressLine2,
                        Addressline3 = source.Address.AddressLine3,
                        Addressline4 = ((ForeignAddress) source.Address).AddressLine4
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void PrintReturnRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintReturnRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new V8.Print_Recipient()
                {
                    Name = source.Name,
                    Norwegian_Address = new Norwegian_Address()
                    {
                        Zip_Code = ((NorwegianAddress) source.Address).PostalCode,
                        City = ((NorwegianAddress) source.Address).City,
                        Addressline1 = source.Address.AddressLine1,
                        Addressline2 = source.Address.AddressLine2,
                        Addressline3 = source.Address.AddressLine3
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void RecipientById()
            {
                //Arrange
                var source = new RecipientById(
                    IdentificationType.DigipostAddress,
                    "ola.nordmann#2233"
                );

                var expectedDto = new V8.Message_Recipient()
                {
                    Digipost_Address = "ola.nordmann#2233",
                    Print_Details = null //TODO: Implementer print!
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

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

                var expectedDto = new V8.Message_Recipient()
                {
                    Name_And_Address = new V8.Name_And_Address()
                    {
                        Fullname = source.FullName,
                        Addressline1 = source.AddressLine1,
                        Addressline2 = source.AddressLine2,
                        Postalcode = source.PostalCode,
                        City = source.City,
                        Birth_Date = birthDate,
                        Birth_DateSpecified = true,
                        Phone_Number = source.PhoneNumber,
                        Email_Address = source.Email
                    }
                };

                //Act
                var actualDto = source.ToDataTransferObject();

                //Assert
                Comparator.AssertEqual(expectedDto, actualDto);
            }

            [Fact]
            public void SearchResult()
            {
                //Arrange
                var recipient0 = new V8.Recipient()
                {
                    Firstname = "Stian Jarand",
                    Middlename = "Jani",
                    Lastname = "Larsen",
                    Digipost_Address = "Stian.Jani.Larsen#22DB",
                    Mobile_Number = "12345678",
                    Organisation_Name = "Organisatorisk Landsforbund",
                    Organisation_Number = "1234567689",
                };
                recipient0.Address.Add(new V8.Address()
                {
                    Street = "Bakkeveien",
                    House_Number = "3",
                    House_Letter = "C",
                    Additional_Addressline = "Underetasjen",
                    Zip_Code = "3453",
                    City = "Konjakken"
                });
                recipient0.Address.Add(new V8.Address()
                {
                    Street = "Komleveien",
                    House_Number = "33",
                    Zip_Code = "3453",
                    City = "Konjakken"
                });

                var recipient1 = new V8.Recipient()
                {
                    Firstname = "Bentoni Jarandsen",
                    Lastname = "Larsen",
                    Mobile_Number = "02345638",
                    Digipost_Address = "Bentoni.Jarandsen.Larsen#KG33",
                };
                recipient1.Address.Add(new V8.Address()
                {
                    Street = "Mudleveien",
                    House_Number = "45",
                    Zip_Code = "4046",
                    City = "Hafrsfjell"
                });

                var source = new V8.Recipients();
                source.Recipient.Add(recipient0);
                source.Recipient.Add(recipient1);

                var expected = new SearchDetailsResult
                {
                    PersonDetails = new List<SearchDetails>
                    {
                        new SearchDetails
                        {
                            FirstName = recipient0.Firstname,
                            MiddleName = recipient0.Middlename,
                            LastName = recipient0.Lastname,
                            MobileNumber = recipient0.Mobile_Number,
                            OrganizationName = recipient0.Organisation_Name,
                            OrganizationNumber = recipient0.Organisation_Number,
                            DigipostAddress = recipient0.Digipost_Address,
                            SearchDetailsAddress = new List<SearchDetailsAddress>
                            {
                                new SearchDetailsAddress
                                {
                                    Street = recipient0.Address[0].Street,
                                    HouseNumber = recipient0.Address[0].House_Number,
                                    HouseLetter = recipient0.Address[0].House_Letter,
                                    AdditionalAddressLine = recipient0.Address[0].Additional_Addressline,
                                    PostalCode = recipient0.Address[0].Zip_Code,
                                    City = recipient0.Address[0].City
                                },
                                new SearchDetailsAddress
                                {
                                    Street = recipient0.Address[1].Street,
                                    HouseNumber = recipient0.Address[1].House_Number,
                                    HouseLetter = recipient0.Address[1].House_Letter,
                                    AdditionalAddressLine = recipient0.Address[1].Additional_Addressline,
                                    PostalCode = recipient0.Address[1].Zip_Code,
                                    City = recipient0.Address[1].City
                                }
                            }
                        },
                        new SearchDetails
                        {
                            FirstName = recipient1.Firstname,
                            MiddleName = recipient1.Middlename,
                            LastName = recipient1.Lastname,
                            MobileNumber = recipient1.Mobile_Number,
                            OrganizationName = recipient1.Organisation_Name,
                            OrganizationNumber = recipient1.Organisation_Number,
                            DigipostAddress = recipient1.Digipost_Address,
                            SearchDetailsAddress = new List<SearchDetailsAddress>
                            {
                                new SearchDetailsAddress
                                {
                                    Street = recipient1.Address[0].Street,
                                    HouseNumber = recipient1.Address[0].House_Number,
                                    HouseLetter = recipient1.Address[0].House_Letter,
                                    AdditionalAddressLine = recipient1.Address[0].Additional_Addressline,
                                    PostalCode = recipient1.Address[0].Zip_Code,
                                    City = recipient1.Address[0].City
                                }
                            }
                        }
                    }
                };

                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Error()
            {
                //Arrange
                var source = new V8.Error()
                {
                    Error_Code = "UNKNOWN_RECIPIENT",
                    Error_Message = "The recipient does not have a Digipost account.",
                    Error_Type = "CLIENT_DATA"
                };

                var expected = new Error
                {
                    Errorcode = source.Error_Code,
                    Errormessage = source.Error_Message,
                    Errortype = source.Error_Type
                };

                //Act
                var actual = source.FromDataTransferObject();

                //Assert
                Comparator.AssertEqual(expected, actual);
            }
        }
    }
}
