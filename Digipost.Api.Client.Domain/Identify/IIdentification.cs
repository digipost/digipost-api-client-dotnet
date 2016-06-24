namespace Digipost.Api.Client.Domain.Identify
{
    public interface IIdentification : IRequestContent
    {
        DigipostRecipient DigipostRecipient { get; set; }
    }
}