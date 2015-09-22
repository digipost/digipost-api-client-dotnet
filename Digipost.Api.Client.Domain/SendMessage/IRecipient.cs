using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IRecipient
    {
        IPrintDetails PrintDetails{ get; set; }
    }
}