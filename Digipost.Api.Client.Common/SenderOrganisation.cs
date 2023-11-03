namespace Digipost.Api.Client.Common
{
    /// <summary>
    ///     A SenderOrganisation is the same as a @See Sender. But the identification
    ///     is by OrganisationNumber instead of the id in Digipost.
    ///     A organisation can have several accounts and to separate different accounts
    ///     a PartId can be given the account in Digipost.
    /// </summary>
    public class SenderOrganisation
    {
        public string OrganisationNumber { get; }
        public string PartId { get; }

        public SenderOrganisation(string organisationNumber, string partId)
        {
            OrganisationNumber = organisationNumber;
            PartId = partId;
        }
    }
}
