using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class SignedDocument : IDataType
    {
        public SignedDocument(string documentIssuer, string documentSubject, DateTime signingTime)
        {
            DocumentIssuer = documentIssuer;
            DocumentSubject = documentSubject;
            SigningTime = signingTime;
        }

        /// <summary>
        ///     The original issuer of the document to be signed
        /// </summary>
        public string DocumentIssuer { get; set; }
        
        /// <summary>
        ///     The original subject of the document to be signed
        /// </summary>
        public string DocumentSubject { get; set; }
        
        /// <summary>
        ///     When the recipient signed the document. ISO8601 full DateTime
        /// </summary>
        public DateTime SigningTime { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal signedDocument AsDataTransferObject()
        {
            var dto = new signedDocument
            {
                documentissuer = DocumentIssuer,
                documentsubject = DocumentSubject,
                signingtime = SigningTime.ToString("O")
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"DocumentIssuer: '{DocumentIssuer}', " +
                   $"DocumentSubject: '{DocumentSubject}', " +
                   $"SigningTime: '{SigningTime}'";
        }
    }
}
