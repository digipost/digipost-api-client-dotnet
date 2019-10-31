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

            var source = new DataTypes.Proof.Proof("Blah & Co. Authority", new ValidPeriod(new Period {From = time}), new ProofHolder("Testbert", "Foogård"), "Proof Title");

            var expected = source.AsDataTransferObject();

            var actual = new proof
            {
                authorizername = "Blah & Co. Authority",
                validperiod = new validPeriod { Item = new period { from = time.ToString("O")} },
                proofholder = new proofHolder { firstname = "Testbert", surname = "Foogård" },
                title = "Proof Title"
            };

            Comparator.AssertEqual(expected, actual);
        }
    }
}
