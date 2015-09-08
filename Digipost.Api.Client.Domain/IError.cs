using System.Collections.Generic;

namespace Digipost.Api.Client.Domain
{
    public interface IError
    {
        string Errorcode { get; set; }
        string Errormessage { get; set; }
        string Errortype { get; set; }
        string ToString();
    }
}