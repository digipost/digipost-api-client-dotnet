using Digipost.Api.Client.Internal;
using Xunit;

namespace Digipost.Api.Client.Tests.Internal
{
    public class RequestHeaderUtilityTests
    {
        public class GetAssemblyVersionMethod
        {
            [Fact]
            public void DescribesTheActualRuntime_InsteadOfTheNetCoreOnlyFallback()
            {
                var userAgent = RequestHeaderUtility.GetAssemblyVersion();

                Assert.StartsWith("digipost-api-client-dotnet/", userAgent);
                Assert.DoesNotContain("AssemblyVersionNotFound", userAgent);
                Assert.DoesNotContain("netcore/", userAgent);
            }
        }
    }
}
