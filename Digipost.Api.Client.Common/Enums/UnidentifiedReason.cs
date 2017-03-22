namespace Digipost.Api.Client.Common.Enums
{
    public enum UnidentifiedReason
    {
        /// <summary>
        ///     When more than one possible subject. Try narrow down the search with more information about the subject.
        /// </summary>
        MultipleMatches,

        /// <summary>
        ///     Subject not found based on search criteria.
        /// </summary>
        NotFound
    }
}