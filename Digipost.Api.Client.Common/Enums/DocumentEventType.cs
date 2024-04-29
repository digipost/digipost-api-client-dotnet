namespace Digipost.Api.Client.Common.Enums
{
    public enum DocumentEventType
    {
        EmailNotificationFailed,
        EmailMessageSent,
        EmailMessageFailed,
        SmsNotificationFailed,
        Opened,
        MoveFilesFromPublicSector,
        Postmarked,
        PrintFailed,
        Shredded,
        PeppolDelivered,
        PeppolFailed,
        RequestForRegistrationExpired,
        RequestForRegistrationDeliveredDigipost,
        RequestForRegistrationFailed,
        ShareDocumentsRequestDocumentsShared,
        ShareDocumentsRequestSharingWithdrawn,
    }
}
