using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.DataTypes;

namespace Digipost.Api.Client.Send
{
    public interface IDocument
    {
        /// <summary>
        ///     Unique identification of document. Is set automatically using System.Guid.NewGuid(), and is
        ///     necessary to change.
        /// </summary>
        string Guid { get; set; }

        /// <summary>
        ///     The subject of the message
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        ///     The file type of the document, indicated by file-type.
        /// </summary>
        string FileType { get; set; }

        /// <summary>
        ///     Optional SMS notification to Recipient.
        ///     Additional charges apply.
        /// </summary>
        ISmsNotification SmsNotification { get; set; }

        /// <summary>
        ///     The level of authentication for the document.
        /// </summary>
        AuthenticationLevel AuthenticationLevel { get; set; }

        /// <summary>
        ///     Sets the sensitivity level for the document.
        /// </summary>
        SensitivityLevel SensitivityLevel { get; set; }

        /// <summary>
        ///     The document encoded as a byte array.
        /// </summary>
        byte[] ContentBytes { get; set; }

        /// <summary>
        ///     Optional metadata to enrich the document in Digipost. See https://github.com/digipost/digipost-data-types for valid data-types.
        /// </summary>
        IDataType DataType { get; set; }
    }
}
