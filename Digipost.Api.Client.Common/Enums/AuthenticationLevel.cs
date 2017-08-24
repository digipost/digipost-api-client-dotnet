namespace Digipost.Api.Client.Common.Enums
{
    public enum AuthenticationLevel
    {
        /// <summary>
        ///     Default. Social security number and password is required to open the letter.
        /// </summary>
        Password,

        /// <summary>
        ///     Two factor authentication will be required to open the letter.
        /// </summary>
        TwoFactor
    }
}
