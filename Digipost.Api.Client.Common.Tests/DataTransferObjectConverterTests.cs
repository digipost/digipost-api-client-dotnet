using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Common.Tests
{
    public class DataTransferObjectConverterTests
    {
        private readonly Comparator _comparator = new Comparator();

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

                var expectedDto = new foreignaddress
                {
                    ItemElementName = ItemChoiceType2.countrycode,
                    Item = "SE",
                    addressline1 = source.AddressLine1,
                    addressline2 = source.AddressLine2,
                    addressline3 = source.AddressLine3,
                    addressline4 = source.Addressline4
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByAddressReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "ola.nordmann#1234";
                var source = new identificationresult
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] {digipostAddress},
                    ItemsElementName = new[] {ItemsChoiceType.digipostaddress}

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByAddressReturnsIdentifiedResultWithPersonalAliasResultType()
            {
                //Arrange
                const string personAlias = "fewoinf23nio3255n32oi5n32oi5n#1234";
                var source = new identificationresult
                {
                    result = identificationresultcode.IDENTIFIED,
                    Items = new object[] {personAlias},
                    ItemsElementName = new[] {ItemsChoiceType.personalias}

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = personAlias,
                    //IdentificationResultType = IdentificationResultType.Personalias
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, personAlias);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
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
                var reason = unidentifiedreason.NOT_FOUND;
                var source = new identificationresult
                {
                    result = identificationresultcode.UNIDENTIFIED,
                    Items = new object[] {unidentifiedreason.NOT_FOUND},
                    ItemsElementName = new[] {ItemsChoiceType.unidentifiedreason}

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
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

                var sourceRecipient = (RecipientByNameAndAddress) source.DigipostRecipient;

                var expectedDto = new identification
                {
                    ItemElementName = ItemChoiceType.nameandaddress,
                    Item = new nameandaddress
                    {
                        fullname = sourceRecipient.FullName,
                        addressline1 = sourceRecipient.AddressLine1,
                        addressline2 = sourceRecipient.AddressLine2,
                        postalcode = sourceRecipient.PostalCode,
                        city = sourceRecipient.City,
                        birthdate = sourceRecipient.BirthDate.Value,
                        birthdateSpecified = true,
                        phonenumber = sourceRecipient.PhoneNumber,
                        emailaddress = sourceRecipient.Email
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

                var expectedDto = new identification
                {
                    ItemElementName = ItemChoiceType.nameandaddress,
                    Item = new nameandaddress
                    {
                        fullname = sourceRecipient.FullName,
                        addressline1 = sourceRecipient.AddressLine1,
                        addressline2 = sourceRecipient.AddressLine2,
                        postalcode = sourceRecipient.PostalCode,
                        city = sourceRecipient.City,
                        birthdateSpecified = false,
                        phonenumber = sourceRecipient.PhoneNumber,
                        emailaddress = sourceRecipient.Email
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
            public void IdentificationByOrganizationNumber()
            {
                //Arrange
                var source = new Identification(new RecipientById(IdentificationType.OrganizationNumber, "123456789"));
                var expectedDto = new identification
                {
                    ItemElementName = ItemChoiceType.organisationnumber,
                    Item = ((RecipientById) source.DigipostRecipient).Id
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsDigipostResultWithDigipostAddressResultType()
            {
                //Arrange
                const string digipostAddress = "bedriften#1234";
                var source = new identificationresult
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] {digipostAddress},
                    ItemsElementName = new[] {ItemsChoiceType.digipostaddress}

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = digipostAddress,
                    //IdentificationResultType = IdentificationResultType.DigipostAddress
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
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
                object invalidValue = invalidreason.INVALID_ORGANISATION_NUMBER;
                var source = new identificationresult
                {
                    result = identificationresultcode.INVALID,
                    Items = new[] {invalidValue},
                    ItemsElementName = new[] {ItemsChoiceType.invalidreason}

                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByOrganizationNumberReturnsUnidentifiedResultWithUnidentifiedReason()
            {
                //Arrange
                var reason = unidentifiedreason.NOT_FOUND;
                var source = new identificationresult
                {
                    result = identificationresultcode.UNIDENTIFIED,
                    Items = new object[] {reason},
                    ItemsElementName = new[] {ItemsChoiceType.unidentifiedreason}

                    //IdentificationResultCode = IdentificationResultCode.Unidentified,
                    //IdentificationValue = reason,
                    //IdentificationResultType = IdentificationResultType.UnidentifiedReason
                };

                var expected = new IdentificationResult(IdentificationResultType.UnidentifiedReason, reason.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByPinReturnsDigipostResultWithNoneResultType()
            {
                //Arrange
                var source = new identificationresult
                {
                    result = identificationresultcode.DIGIPOST,
                    Items = new object[] {null}
                    //ItemsElementName = new [] { },

                    //IdentificationResultCode = IdentificationResultCode.Digipost,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.DigipostAddress, string.Empty);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByPinReturnsIdentifiedResultWithNoneResultType()
            {
                //Arrange

                var source = new identificationresult
                {
                    result = identificationresultcode.IDENTIFIED,
                    Items = new object[] {null}
                    //ItemsElementName = new [] { },

                    //IdentificationResultCode = IdentificationResultCode.Identified,
                    //IdentificationValue = null,
                    //IdentificationResultType = IdentificationResultType.None
                };

                var expected = new IdentificationResult(IdentificationResultType.Personalias, string.Empty);

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void IdentificationByPinReturnsInvalidResultWithInvalidReason()
            {
                //Arrange
                object invalidValue = invalidreason.INVALID_PERSONAL_IDENTIFICATION_NUMBER;
                var source = new identificationresult
                {
                    result = identificationresultcode.INVALID,
                    Items = new[] {invalidValue},
                    ItemsElementName = new[] {ItemsChoiceType.invalidreason}

                    //IdentificationResultCode = IdentificationResultCode.Invalid,
                    //IdentificationValue = invalidValue,
                    //IdentificationResultType = IdentificationResultType.InvalidReason
                };

                var expected = new IdentificationResult(IdentificationResultType.InvalidReason, invalidValue.ToString());

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
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

                var expectedDto = new norwegianaddress
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
            public void PrintDetails()
            {
                //Arrange
                var source = new PrintDetails(
                    new PrintRecipient(
                        "Name",
                        new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3")),
                    new PrintReturnRecipient(
                        "ReturnName",
                        new NorwegianAddress("0001", "OsloRet", "Addr1Ret", "Addr2Ret", "Addr3Ret")))
                {
                    PostType = PostType.A
                };

                var sourceAddress = source.PrintRecipient.Address;
                var returnAddress = source.PrintReturnRecipient.Address;

                var expectedDto = new printdetails
                {
                    posttype = posttype.A,
                    recipient = new printrecipient
                    {
                        name = source.PrintRecipient.Name,
                        Item = new norwegianaddress
                        {
                            zipcode = ((NorwegianAddress) sourceAddress).PostalCode,
                            city = ((NorwegianAddress) sourceAddress).City,
                            addressline1 = sourceAddress.AddressLine1,
                            addressline2 = sourceAddress.AddressLine2,
                            addressline3 = sourceAddress.AddressLine3
                        }
                    },
                    returnaddress = new printrecipient
                    {
                        name = source.PrintReturnRecipient.Name,
                        Item = new norwegianaddress
                        {
                            zipcode = ((NorwegianAddress) returnAddress).PostalCode,
                            city = ((NorwegianAddress) returnAddress).City,
                            addressline1 = returnAddress.AddressLine1,
                            addressline2 = returnAddress.AddressLine2,
                            addressline3 = returnAddress.AddressLine3
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

                var expectedDto = new printrecipient
                {
                    name = source.Name,
                    Item = new foreignaddress
                    {
                        ItemElementName = ItemChoiceType2.country,
                        Item = "NORGE",
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                        addressline4 = ((ForeignAddress) source.Address).Addressline4
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
            public void PrintRecipientFromNorwegianAddress()
            {
                //Arrange
                var source = new PrintRecipient(
                    "Name",
                    new NorwegianAddress("0001", "Oslo", "Addr1", "Addr2", "Addr3"));

                var expectedDto = new printrecipient
                {
                    name = source.Name,
                    Item = new norwegianaddress
                    {
                        zipcode = ((NorwegianAddress) source.Address).PostalCode,
                        city = ((NorwegianAddress) source.Address).City,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3
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

                var expectedDto = new printrecipient
                {
                    name = source.Name,
                    Item = new foreignaddress
                    {
                        ItemElementName = ItemChoiceType2.country,
                        Item = ((ForeignAddress) source.Address).CountryIdentifierValue,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3,
                        addressline4 = ((ForeignAddress) source.Address).Addressline4
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

                var expectedDto = new printrecipient
                {
                    name = source.Name,
                    Item = new norwegianaddress
                    {
                        zipcode = ((NorwegianAddress) source.Address).PostalCode,
                        city = ((NorwegianAddress) source.Address).City,
                        addressline1 = source.Address.AddressLine1,
                        addressline2 = source.Address.AddressLine2,
                        addressline3 = source.Address.AddressLine3
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
            public void RecipientById()
            {
                //Arrange
                var source = new RecipientById(
                    IdentificationType.DigipostAddress,
                    "ola.nordmann#2233"
                );

                var expectedDto = new messagerecipient
                {
                    ItemElementName = ItemChoiceType1.digipostaddress,
                    Item = "ola.nordmann#2233",
                    printdetails = null //TODO: Implementer print!
                };

                //Act
                var actualDto = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actualDto, out differences);
                Assert.Equal(0, differences.Count());
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

                var expectedDto = new messagerecipient
                {
                    ItemElementName = ItemChoiceType1.nameandaddress,
                    Item = new nameandaddress
                    {
                        fullname = source.FullName,
                        addressline1 = source.AddressLine1,
                        addressline2 = source.AddressLine2,
                        postalcode = source.PostalCode,
                        city = source.City,
                        birthdate = birthDate,
                        birthdateSpecified = true,
                        phonenumber = source.PhoneNumber,
                        emailaddress = source.Email
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
            public void SearchResult()
            {
                //Arrange
                var source = new recipients
                {
                    recipient = new[]
                    {
                        new recipient
                        {
                            firstname = "Stian Jarand",
                            middlename = "Jani",
                            lastname = "Larsen",
                            digipostaddress = "Stian.Jani.Larsen#22DB",
                            mobilenumber = "12345678",
                            organisationname = "Organisatorisk Landsforbund",
                            organisationnumber = "1234567689",
                            address = new[]
                            {
                                new address
                                {
                                    street = "Bakkeveien",
                                    housenumber = "3",
                                    houseletter = "C",
                                    additionaladdressline = "Underetasjen",
                                    zipcode = "3453",
                                    city = "Konjakken"
                                },
                                new address
                                {
                                    street = "Komleveien",
                                    housenumber = "33",
                                    zipcode = "3453",
                                    city = "Konjakken"
                                }
                            }
                        },
                        new recipient
                        {
                            firstname = "Bentoni Jarandsen",
                            lastname = "Larsen",
                            mobilenumber = "02345638",
                            digipostaddress = "Bentoni.Jarandsen.Larsen#KG33",
                            address = new[]
                            {
                                new address
                                {
                                    street = "Mudleveien",
                                    housenumber = "45",
                                    zipcode = "4046",
                                    city = "Hafrsfjell"
                                }
                            }
                        }
                    }
                };

                var recipient0 = source.recipient.ElementAt(0);
                var recipient1 = source.recipient.ElementAt(1);

                var expected = new SearchDetailsResult
                {
                    PersonDetails = new List<SearchDetails>
                    {
                        new SearchDetails
                        {
                            FirstName = recipient0.firstname,
                            MiddleName = recipient0.middlename,
                            LastName = recipient0.lastname,
                            MobileNumber = recipient0.mobilenumber,
                            OrganizationName = recipient0.organisationname,
                            OrganizationNumber = recipient0.organisationnumber,
                            DigipostAddress = recipient0.digipostaddress,
                            SearchDetailsAddress = new List<SearchDetailsAddress>
                            {
                                new SearchDetailsAddress
                                {
                                    Street = recipient0.address[0].street,
                                    HouseNumber = recipient0.address[0].housenumber,
                                    HouseLetter = recipient0.address[0].houseletter,
                                    AdditionalAddressLine = recipient0.address[0].additionaladdressline,
                                    PostalCode = recipient0.address[0].zipcode,
                                    City = recipient0.address[0].city
                                },
                                new SearchDetailsAddress
                                {
                                    Street = recipient0.address[1].street,
                                    HouseNumber = recipient0.address[1].housenumber,
                                    HouseLetter = recipient0.address[1].houseletter,
                                    AdditionalAddressLine = recipient0.address[1].additionaladdressline,
                                    PostalCode = recipient0.address[1].zipcode,
                                    City = recipient0.address[1].city
                                }
                            }
                        },
                        new SearchDetails
                        {
                            FirstName = recipient1.firstname,
                            MiddleName = recipient1.middlename,
                            LastName = recipient1.lastname,
                            MobileNumber = recipient1.mobilenumber,
                            OrganizationName = recipient1.organisationname,
                            OrganizationNumber = recipient1.organisationnumber,
                            DigipostAddress = recipient1.digipostaddress,
                            SearchDetailsAddress = new List<SearchDetailsAddress>
                            {
                                new SearchDetailsAddress
                                {
                                    Street = recipient1.address[0].street,
                                    HouseNumber = recipient1.address[0].housenumber,
                                    HouseLetter = recipient1.address[0].houseletter,
                                    AdditionalAddressLine = recipient1.address[0].additionaladdressline,
                                    PostalCode = recipient1.address[0].zipcode,
                                    City = recipient1.address[0].city
                                }
                            }
                        }
                    }
                };

                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                var comparator = new Comparator {ComparisonConfiguration = new ComparisonConfiguration {IgnoreObjectTypes = true}};
                comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
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

                var expectedDto = new smsnotification
                {
                    afterhours = afterHours.ToArray(),
                    at = atTimes.Select(a => new listedtime {timeSpecified = true, time = a}).ToArray()
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expectedDto, actual, out differences);
                Assert.Equal(0, differences.Count());
            }
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Error()
            {
                //Arrange
                var source = new error
                {
                    errorcode = "UNKNOWN_RECIPIENT",
                    errormessage = "The recipient does not have a Digipost account.",
                    errortype = "CLIENT_DATA"
                };

                var expected = new Error
                {
                    Errorcode = source.errorcode,
                    Errormessage = source.errormessage,
                    Errortype = source.errortype
                };

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                IEnumerable<IDifference> differences;
                _comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }
        }
    }
}
