namespace Digipost.Api.Client.Tests.CompareObjects
{
    public interface IDifference
    {
        object WhatIsCompared { get; set; }

        object ExpectedValue { get; set; }

        string ActualValue { get; set; }

        string PropertyName { get; set; }
    }
}
