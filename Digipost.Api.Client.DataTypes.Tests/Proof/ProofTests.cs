using System;
using Digipost.Api.Client.DataTypes.Proof;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.DataTypes.Tests.Proof
{
    public class ProofTests
    {
        [Fact]
        public void AsDataTransferObject()
        {
            var time = DateTime.Now;

            var source = new DataTypes.Proof.Proof("Blah & Co. Authority", new GyldighetsPeriode(new Periode {Fra = time}), new Bruker("Testbert", "Foogård"), "Proof Title");

            var expected = source.AsDataTransferObject();

            var actual = new proof
            {
                utstedervisningsnavn = "Blah & Co. Authority",
                gyldighetsperioder = new gyldighetsPeriode { Item = new periode { fra = time.ToString("O")} },
                bevisbruker = new bruker { fornavn = "Testbert", etternavn = "Foogård" },
                tittel = "Proof Title"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
