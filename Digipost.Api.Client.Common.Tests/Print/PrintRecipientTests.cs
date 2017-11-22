using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Print
{
    public class PrintRecipientTests
    {
        public class ConstructorMethod : PrintRecipientTests

        {
            private readonly Comparator _comparator = new Comparator();

            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                const string name = "name";

                var printRecipient = new PrintRecipient(name, DomainUtility.GetNorwegianAddress());

                //Act

                //Assert
                Assert.Equal(name, printRecipient.Name);

                _comparator.AssertEqual(DomainUtility.GetNorwegianAddress(), printRecipient.Address);
            }
        }
    }
}
