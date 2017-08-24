namespace Digipost.Api.Client.Common.Enums
{
    public enum InvalidReason
    {
        /// <summary>
        ///     Invalid Social Security Number (SSN). Check the number and try again.
        /// </summary>
        InvalidPersonalIdentificationNumber,

        /// <summary>
        ///     Invalid organisation number. Check the number and try again.
        /// </summary>
        InvalidOrganisationNumber,

        /// <summary>
        ///     Subject is unknown.
        /// </summary>
        Unknown
    }
}
