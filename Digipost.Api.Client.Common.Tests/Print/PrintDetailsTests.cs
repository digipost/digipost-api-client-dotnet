using System.Collections.Generic;
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
                    DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PrintColors.Colors);
                
                List<PrintInstruction> printinstruction = new List<PrintInstruction>();
                printinstruction.Add(new PrintInstruction("test", "testing"));
                printDetails.PrintInstructions = new PrintInstructions(printinstruction);

                //Act

                //Assert
                Comparator.AssertEqual(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient);

                Comparator.AssertEqual(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient);

                Assert.Equal(PrintColors.Colors, printDetails.PrintColors);
                Assert.Equal(NondeliverableHandling.ReturnToSender, printDetails.NondeliverableHandling);
                Comparator.AssertEqual(printinstruction, printDetails.PrintInstructions);
            }

            [Fact]
            public void CanSetShreddedDeliverableHandling()
            {
                //Arrange
                List<PrintInstruction> printinstruction = new List<PrintInstruction>();
                printinstruction.Add(new PrintInstruction("test", "testing"));
                var printDetails = new PrintDetails(DomainUtility.GetPrintRecipientWithNorwegianAddress(),
                    DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PrintColors.Colors)
                {
                    NondeliverableHandling = NondeliverableHandling.Shred, PrintInstructions = new PrintInstructions(printinstruction)
                };

                //Act

                //Assert
                Comparator.AssertEqual(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient);

                Comparator.AssertEqual(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient);

                Assert.Equal(PrintColors.Colors, printDetails.PrintColors);
                Assert.Equal(NondeliverableHandling.Shred, printDetails.NondeliverableHandling);
                Comparator.AssertEqual(printinstruction, printDetails.PrintInstructions);
            }
        }
    }
}
