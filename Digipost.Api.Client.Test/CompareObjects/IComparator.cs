using System.Collections.Generic;

namespace Digipost.Api.Client.Test.CompareObjects
{
    public interface IComparator
    {
        bool Equal(object expected, object actual);

        bool Equal(object expected, object actual, out IEnumerable<IDifference> differences);
    }
}