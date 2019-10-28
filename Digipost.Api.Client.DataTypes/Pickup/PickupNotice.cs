using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Pickup
{
    public class PickupNotice : IDataType
    {
        public PickupNotice(string parcelId, Barcode barcode, DateTime arrivalDateTime, DateTime returnDateTime, Recipient recipient, PickupPlace pickupPlace)
        {
            ParcelId = parcelId;
            Barcode = barcode;
            ArrivalDateTime = arrivalDateTime;
            ReturnDateTime = returnDateTime;
            Recipient = recipient;
            PickupPlace = pickupPlace;
        }

        /// <summary>
        ///     The id of the parcel in posten
        /// </summary>
        public string ParcelId { get; set; }
        
        /// <summary>
        ///     The uuid of the parcel in posten
        /// </summary>
        public string ParcelUuid { get; set; }
        
        /// <summary>
        ///     Barcode
        /// </summary>
        public Barcode Barcode { get; set; }
        
        /// <summary>
        ///     Mail Service product name
        /// </summary>
        public string ProductName { get; set; }
        
        /// <summary>
        ///     ISO8601 full DateTime for arrival at pickup place
        /// </summary>
        public DateTime ArrivalDateTime { get; set; }
        
        /// <summary>
        ///     ISO8601 full DateTime for return back to sender
        /// </summary>
        public DateTime ReturnDateTime { get; set; }
        
        /// <summary>
        ///     The recipient of the parcel
        /// </summary>
        public Recipient Recipient { get; set; }
        
        /// <summary>
        ///     The sender of the parcel
        /// </summary>
        public Sender Sender { get; set; }
        
        /// <summary>
        ///     Where the parcel can be retrieved 
        /// </summary>
        public PickupPlace PickupPlace { get; set; }
        
        /// <summary>
        ///     Package information
        /// </summary>
        public Package Package { get; set; }
        
        /// <summary>
        ///     Information about value, mva, customs processing and more
        /// </summary>
        public Cost Cost { get; set; }
        
        /// <summary>
        ///     The state the package is at present time
        /// </summary>
        public Status Status { get; set; }
        
        /// <summary>
        ///     Tags to describe the document
        /// </summary>
        public List<Tag> Tags { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal pickupNotice AsDataTransferObject()
        {
            var dto = new pickupNotice
            {
                parcelid = ParcelId,
                parceluuid = ParcelUuid,
                barcode = Barcode.AsDataTransferObject(),
                productname = ProductName,
                arrivaldatetime = ArrivalDateTime.ToString("O"),
                returndatetime = ReturnDateTime.ToString("O"),
                recipient = Recipient.AsDataTransferObject(),
                sender = Sender?.AsDataTransferObject(),
                pickupplace = PickupPlace.AsDataTransferObject(),
                package = Package?.AsDataTransferObject(),
                cost = Cost?.AsDataTransferObject(),
                status = (status) Enum.Parse(typeof(status), Status.ToString()),
                tags = Tags?.Select(i => (tag) Enum.Parse(typeof(tag), i.ToString())).ToArray()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"ParcelId: '{ParcelId}', " +
                   $"ParcelUuid: '{ParcelUuid}', " +
                   $"Barcode: '{Barcode?.ToString() ?? "<none>"}', " +
                   $"ProductName: '{ProductName}', " +
                   $"ArrivalDateTime: '{ArrivalDateTime}', " +
                   $"ReturnDateTime: '{ReturnDateTime}', " +
                   $"Recipient: '{Recipient?.ToString() ?? "<none>"}', " +
                   $"Sender: '{Sender?.ToString() ?? "<none>"}', " +
                   $"PickupPlace: '{PickupPlace?.ToString() ?? "<none>"}', " +
                   $"Package: '{Package?.ToString() ?? "<none>"}', " +
                   $"Cost: '{Cost?.ToString() ?? "<none>"}', " +
                   $"Status: '{Status.ToString()}', " +
                   $"Tags: '{(Tags != null ? string.Join(", ", Tags.Select(x => x.ToString())) : "<none>")}'";
        }
    }
}
