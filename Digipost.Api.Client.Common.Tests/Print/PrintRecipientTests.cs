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
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                const string name = "name";

                var printRecipient = new PrintRecipient(name, DomainUtility.GetNorwegianAddress());

                //Act

                //Assert
                Assert.Equal(name, printRecipient.Name);

                Comparator.AssertEqual(DomainUtility.GetNorwegianAddress(), printRecipient.Address);
            }
        }
    }
}
