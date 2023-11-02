using System.Collections.Generic;

namespace Digipost.Api.Client.Common.SenderInfo
{
    public class SenderInformation
    {
        public Sender Sender { get; }

        public SenderStatus SenderStatus { get; }

        public IEnumerable<SenderFeature> SenderFeatures { get; }

        public bool IsValidSender => SenderStatus == SenderStatus.ValidSender && Sender.Id > 0;

        public SenderInformation(SenderStatus senderStatus)
        {
            SenderStatus = senderStatus;
        }

        public SenderInformation(Sender sender, SenderStatus senderStatus, IEnumerable<SenderFeature> senderFeatures)
        {
            Sender = sender;
            SenderStatus = senderStatus;
            SenderFeatures = senderFeatures;
        }
    }

    public class SenderFeature
    {
        public string Identificator { get; }

        public string Param { get; }

        public SenderFeature(string identificator, string param)
        {
            Identificator = identificator;
            Param = param;
        }
    }

}
