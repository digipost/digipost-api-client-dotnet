using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Event
{
    public class Event : IDataType
    {
        /// <summary>
        ///     The name of the event.
        ///     Example: Kommunestyre- og fylkestingvalg
        ///     150 characters or less.
        /// </summary>
        public string SubTitle { get; set; }
        
        /// <summary>
        ///     A List of the time intervals of the event.
        /// </summary>
        public List<EventTimeSpan> Time { get; set; }
        
        /// <summary>
        ///     Optional label for time. null yield default in gui, eg. 'Opening hours'
        /// </summary>
        public string TimeLabel { get; set; }
        
        /// <summary>
        ///     Free text but can contain a ISO8601 DateTime.
        ///     Example: 'Please use entrance from street'
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        ///     The name of the place.
        ///     Example: 'Sagene skole'
        /// </summary>
        public string Place { get; set; }
        
        /// <summary>
        ///     Optional label for place. null yield default in gui, eg. 'Venue location'
        /// </summary>
        public string PlaceLabel { get; set; }
        
        /// <summary>
        ///     Address where the event takes place.
        /// </summary>
        public EventAddress Address { get; set; }
        
        /// <summary>
        ///     Additional sections of information (max 10) with a title and text.
        /// </summary>
        public List<Info> Info { get; set; }
        
        /// <summary>
        ///     Optional label for barcode. null yield default in gui, eg. ''
        /// </summary>
        public string BarcodeLabel { get; set; }
        
        /// <summary>
        ///     The Barcode to be scanned for validation.
        /// </summary>
        public EventBarcode Barcode { get; set; }
        
        /// <summary>
        ///     Links for related information to the event.
        /// </summary>
        public List<ExternalLink> Links { get; set; }
        
        public Event(List<EventTimeSpan> time)
        {
            Time = time;
        }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal @event AsDataTransferObject()
        {
            var dto = new @event
            {
                subTitle = SubTitle,
                time = Time?.Select(i => i.AsDataTransferObject()).ToArray(),
                timeLabel = TimeLabel,
                description = Description,
                place = Place,
                placeLabel = PlaceLabel,
                address = Address?.AsDataTransferObject(),
                info = Info?.Select(i => i.AsDataTransferObject()).ToArray(),
                barcodeLabel = BarcodeLabel,
                barcode = Barcode?.AsDataTransferObject(),
                links = Links?.Select(i => i.AsDataTransferObject()).ToArray()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"Times: '{(Time != null ? string.Join(", ", Time.Select(x => x.ToString())) : "<none>")}', " +
                   $"Label: '{TimeLabel}', " +
                   $"Subtitle: '{SubTitle}', " +
                   $"Description: '{Description}', " +
                   $"Place: '{Place}', " +
                   $"PlaceLabel: '{PlaceLabel}', " +
                   $"Address: '{Address?.ToString() ?? "Address: <none>"}', " +
                   $"Info: '{(Info != null ? string.Join(", ", Info.Select(x => x.ToString())) : "no additional info")}', " +
                   $"BarcodeLabel: '{BarcodeLabel}', " +
                   $"Barcode: '{Barcode?.ToString() ?? "Barcode: <none>"}', " +
                   $"Links: '{(Links != null ? string.Join(", ", Links.Select(x => x.ToString())) : "<none>")}'.";
        }
    }
}
