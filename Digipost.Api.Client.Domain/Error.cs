namespace Digipost.Api.Client.Domain
{
    public class Error : IError
    {
        public string Errormessage { get; set; }

        public string Errortype { get; set; }

        public string Errorcode { get; set; }

        public override string ToString()
        {
            return $"Errorcode: {Errorcode}, Errormessage: {Errormessage}, Errortype: {Errortype}";
        }
    }
}