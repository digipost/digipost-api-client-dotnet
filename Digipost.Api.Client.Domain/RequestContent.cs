using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Domain
{
    public interface IRequestContent
    {
        object DataTransferObject { get;}
    }
}