namespace Digipost.Api.Client.Tests.CompareObjects
{
    internal class Difference : IDifference
    {
        public string WhatIsCompared { get; set; }

        public object ExpectedValue { get; set; }

        public string ActualValue { get; set; }

        public string PropertyName { get; set; }
    }
}