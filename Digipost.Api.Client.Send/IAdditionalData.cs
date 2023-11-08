using Digipost.Api.Client.Common;
using Digipost.Api.Client.DataTypes.Core;

namespace Digipost.Api.Client.Send
{
    public interface IAdditionalData : IRequestContent
    {
        /// <summary>
        ///     The sender of the message, i.e. what the receiver of the message sees as the sender of the message.
        ///     If you are delivering a message on behalf of an organization with id 5555, set this property
        ///     to 5555. If you are delivering on behalf of yourself, set this to your organization`s sender id.
        ///     The id is created by Digipost.
        /// </summary>
        Sender Sender { get; }

        /// <summary>
        ///     Optional metadata to enrich the document in Digipost. See https://github.com/digipost/digipost-data-types for valid data-types.
        /// </summary>
        IDigipostDataType DataType { get; }
    }

    public class AdditionalData : IAdditionalData
    {
        public Sender Sender { get; }

        public IDigipostDataType DataType { get; }

        public AdditionalData(Sender sender, IDigipostDataType dataType)
        {
            Sender = sender;
            DataType = dataType;
        }
    }
}
