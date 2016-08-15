using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    public class PrintReturnRecipientTests
    {
        public class ConstructorMethod : PrintReturnRecipientTests
        {
            private readonly Comparator _comparator = new Comparator();

            [Fact]
            public void SimpleConstructor()
            {
                //Arrange

                //Act
                var printRecipient = new PrintReturnRecipient("name", DomainUtility.GetNorwegianAddress());

                //Assert
                Assert.Equal("name", printRecipient.Name);

                IEnumerable<IDifference> differences;
                _comparator.AreEqual(DomainUtility.GetNorwegianAddress(), printRecipient.Address, out differences);

                Assert.Equal(0, differences.Count());
            }
        }
    }

}