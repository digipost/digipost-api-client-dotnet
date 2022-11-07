using System.Net.Http;
using System.Xml;

namespace Digipost.Api.Client.Common.Actions
{
    public abstract class DigipostAction<T> where T: IRequestContent
    {
        protected DigipostAction(T requestContent)
        {
            InitializeRequestXmlContent(requestContent);
        }

        public XmlDocument RequestContent { get; internal set; }

        internal abstract HttpContent Content(T requestContent);

        protected abstract string Serialize(T requestContent);

        private void InitializeRequestXmlContent(T requestContent)
        {
            if (requestContent == null) return;

            var document = new XmlDocument();
            var serialized = Serialize(requestContent);
            document.LoadXml(serialized);
            RequestContent = document;
        }
    }
}
