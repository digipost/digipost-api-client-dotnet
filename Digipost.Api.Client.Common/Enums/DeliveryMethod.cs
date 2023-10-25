namespace Digipost.Api.Client.Common.Enums
{
    public enum DeliveryMethod
    {
        /// <summary>
        ///     Delivered through physical print and postal service.
        /// </summary>
        Print,

        /// <summary>
        ///     Delivered digitally in Digipost
        /// </summary>
        Digipost,

        /// <summary>
        ///     Delivered through physical peppol.
        /// </summary>
        PEPPOL,

        /// <summary>
        ///     Delivered to email
        /// </summary>
        EPOST,

        /// <summary>
        ///     Pending delivery
        /// </summary>
        PENDING
    }
}
