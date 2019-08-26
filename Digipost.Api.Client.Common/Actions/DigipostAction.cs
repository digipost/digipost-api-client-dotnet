using System.Net.Http;
using System.Xml;

namespace Digipost.Api.Client.Common.Actions
{
    public abstract class DigipostAction
    {
        protected DigipostAction(IRequestContent requestContent)
        {
            InitializeRequestXmlContent(requestContent);
        }

        public XmlDocument RequestContent { get; internal set; }

        internal abstract HttpContent Content(IRequestContent requestContent);

        protected abstract string Serialize(IRequestContent requestContent);

        private void InitializeRequestXmlContent(IRequestContent requestContent)
        {
            if (requestContent == null) return;

            var document = new XmlDocument();
            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);
            RequestContent = document;
        }
    }
}
