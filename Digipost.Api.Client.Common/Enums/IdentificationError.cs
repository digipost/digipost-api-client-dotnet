namespace Digipost.Api.Client.Common.Enums
{
    /// <summary>
    /// `Unknown` is never actually returned from Digipost. `NotFound` is. But since `Unknown` has
    /// been the de facto response for `NotFound` since for ever in this client library
    /// we should just keep it that way.
    /// </summary>
    public enum IdentificationError
    {
        Unidentified,

        Invalid,

        InvalidPersonalIdentificationNumber,

        InvalidOrganisationNumber,

        Unknown,

        NotFound,

        MultipleMatches
    }
}
