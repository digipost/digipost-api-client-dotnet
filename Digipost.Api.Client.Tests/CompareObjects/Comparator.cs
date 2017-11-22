using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace Digipost.Api.Client.Tests.CompareObjects
{
    internal class Comparator
    {
        public ComparisonConfiguration ComparisonConfiguration { get; set; } = new ComparisonConfiguration();

        private static void Equal(object expected, object actual, out IEnumerable<Difference> differences)
        {
            var compareLogic = new CompareLogic(
                new ComparisonConfig
                {
                    MaxDifferences = 5,
                    IgnoreObjectTypes = true
                });

            var compareResult = compareLogic.Compare(expected, actual);

            differences = compareResult.Differences.Select(d => new Difference
            {
                PropertyName = d.PropertyName,
                WhatIsCompared = d.GetWhatIsCompared(),
                ExpectedValue = d.Object1Value,
                ActualValue = d.Object2Value
            }).ToList();
        }

        public void AssertEqual(object expected, object actual)
        {
            IEnumerable<Difference> differences;
            Equal(expected, actual, out differences);
            Assert.Equal(new List<Difference>(), differences);
        }
    }
}
