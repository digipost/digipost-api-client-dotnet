namespace Digipost.Api.Client.Tests.CompareObjects
{
    internal class Difference : IDifference
    {
        public object WhatIsCompared { get; set; }

        public object ExpectedValue { get; set; }

        public string ActualValue { get; set; }

        public string PropertyName { get; set; }
    }
}
