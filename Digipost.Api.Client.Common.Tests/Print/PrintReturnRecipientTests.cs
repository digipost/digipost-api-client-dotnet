using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Print
{
    public class PrintReturnRecipientTests
    {
        public class ConstructorMethod : PrintReturnRecipientTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                const string name = "name";

                //Act
                var printRecipient = new PrintReturnRecipient(name, DomainUtility.GetNorwegianAddress());

                //Assert
                Assert.Equal(name, printRecipient.Name);

                Comparator.AssertEqual(DomainUtility.GetNorwegianAddress(), printRecipient.Address);
            }
        }
    }
}
