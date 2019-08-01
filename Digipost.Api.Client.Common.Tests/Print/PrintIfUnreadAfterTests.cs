using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Tests;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Print
{
    public class PrintIfUnreadAfterTests
    {
        public class ConstructorMethod : PrintIfUnreadAfterTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                DateTime deadline = DateTime.Now.AddDays(3);
                PrintDetails printDetails = new PrintDetails(DomainUtility.GetPrintRecipientWithNorwegianAddress(),
                        DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PrintColors.Colors);
                
                //Arrange
                var printIfUnreadAfter = new PrintIfUnread(
                    deadline, 
                    printDetails);
                
                //Act

                //Assert
                Assert.Equal(deadline, printIfUnreadAfter.PrintIfUnreadAfter);

                Comparator.AssertEqual(printDetails, printIfUnreadAfter.PrintDetails);
            }
        }
    }
}
