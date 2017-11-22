namespace Digipost.Api.Client.Tests.CompareObjects
{
    internal class Difference
    {
        public string WhatIsCompared { get; set; }

        public object ExpectedValue { get; set; }

        public string ActualValue { get; set; }

        public string PropertyName { get; set; }

        public override string ToString()
        {
            return $"Difference in property '{PropertyName}'! Expected '{ExpectedValue}' but was '{ActualValue}'";
        }
    }
}
