using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Print
{
    public class PrintDetailsTests
    {
        public class ConstructorMethod : PrintDetailsTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var printDetails = new PrintDetails(DomainUtility.GetPrintRecipientWithNorwegianAddress(),
                    DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PostType.A, PrintColors.Colors);

                //Act

                //Assert
                Comparator.AssertEqual(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient);

                Comparator.AssertEqual(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient);

                Assert.Equal(PostType.A, printDetails.PostType);
                Assert.Equal(PrintColors.Colors, printDetails.PrintColors);
                Assert.Equal(NondeliverableHandling.ReturnToSender, printDetails.NondeliverableHandling);
            }
        }
    }
}
