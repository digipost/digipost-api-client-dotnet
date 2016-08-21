using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Xunit;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
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
                IEnumerable<IDifference> printDifference;
                _comparator.Equal(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient, out printDifference);
                Assert.Equal(0, printDifference.Count());

                IEnumerable<IDifference> printReturnDifference;
                _comparator.Equal(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient, out printReturnDifference);
                Assert.Equal(0, printReturnDifference.Count());

                Assert.Equal(PostType.A, printDetails.PostType);
                Assert.Equal(PrintColors.Colors, printDetails.PrintColors);
                Assert.Equal(NondeliverableHandling.ReturnToSender, printDetails.NondeliverableHandling);
            }
        }
    }
}