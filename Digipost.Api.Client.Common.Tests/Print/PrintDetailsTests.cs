using System.Collections.Generic;
using System.Linq;
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
            private readonly Comparator _comparator = new Comparator();

            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var printDetails = new PrintDetails(DomainUtility.GetPrintRecipientWithNorwegianAddress(),
                    DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PostType.A, PrintColors.Colors);

                //Act

                //Assert
                _comparator.AssertEqual(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient);

                _comparator.AssertEqual(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient);

                Assert.Equal(PostType.A, printDetails.PostType);
                Assert.Equal(PrintColors.Colors, printDetails.PrintColors);
                Assert.Equal(NondeliverableHandling.ReturnToSender, printDetails.NondeliverableHandling);
            }
        }
    }
}
