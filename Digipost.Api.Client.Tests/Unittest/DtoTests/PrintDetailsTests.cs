using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;
using Digipost.Api.Client.Tests.CompareObjects;
using Digipost.Api.Client.Tests.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest.DtoTests
{
    [TestClass]
    public class PrintDetailsTests
    {
        [TestClass]
        public class ConstructorMethod : PrintDetailsTests
        {
            Comparator _comparator = new Comparator();
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                PrintDetails printDetails = new PrintDetails(DomainUtility.GetPrintRecipientWithNorwegianAddress(),
                    DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), PostType.A, PrintColors.Colors);

                //Act

                //Assert
                IEnumerable<IDifference> printDifference;
                _comparator.AreEqual(DomainUtility.GetPrintRecipientWithNorwegianAddress(), printDetails.PrintRecipient, out printDifference);
                Assert.AreEqual(0, printDifference.Count());

                IEnumerable<IDifference> printReturnDifference;
                _comparator.AreEqual(DomainUtility.GetPrintReturnRecipientWithNorwegianAddress(), printDetails.PrintReturnRecipient, out printReturnDifference);
                Assert.AreEqual(0, printReturnDifference.Count());

                Assert.AreEqual(PostType.A,printDetails.PostType);
                Assert.AreEqual(PrintColors.Colors, printDetails.PrintColors);
                Assert.AreEqual(NondeliverableHandling.ReturnToSender, printDetails.NondeliverableHandling);


            } 
        }
    }
}
