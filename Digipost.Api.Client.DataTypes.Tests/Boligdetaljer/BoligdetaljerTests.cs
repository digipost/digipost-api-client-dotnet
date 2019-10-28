using System;
using Digipost.Api.Client.DataTypes.Boligdetaljer;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Boligdetaljer
{
    public class BoligdetaljerTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var source = new DataTypes.Boligdetaljer.Boligdetaljer(new Residence(new ResidenceAddress {UnitNumber = "H1234", HouseNumber = "A4", StreetName = "Test Gate", City = "Oslo", PostalCode = "1234"} ));

            var expected = source.AsDataTransferObject();

            var actual = new boligdetaljer
            {
                residence = new residence { address = new residenceAddress { unitnumber = "H1234", housenumber = "A4", streetname = "Test Gate", city = "Oslo", postalcode = "1234"} }
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
