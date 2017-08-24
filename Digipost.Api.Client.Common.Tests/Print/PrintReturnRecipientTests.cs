﻿using System.Collections.Generic;
using System.Linq;
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
            private readonly Comparator _comparator = new Comparator();

            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                const string name = "name";

                //Act
                var printRecipient = new PrintReturnRecipient(name, DomainUtility.GetNorwegianAddress());

                //Assert
                Assert.Equal(name, printRecipient.Name);

                IEnumerable<IDifference> differences;
                _comparator.Equal(DomainUtility.GetNorwegianAddress(), printRecipient.Address, out differences);

                Assert.Equal(0, differences.Count());
            }
        }
    }
}
