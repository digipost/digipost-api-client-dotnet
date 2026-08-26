namespace Digipost.Api.Client.Common.Enums
{
    public enum DocumentEventType
    {
        EmailNotificationFailed,
        EmailMessageSent,
        EmailMessageFailed,
        SmsNotificationFailed,
        SmsNotificationDelivered,
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
