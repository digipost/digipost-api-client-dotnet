using System;
using System.Xml;

namespace Digipost.Api.Client.DataTypes.Receipt
{
    public class TaxiDetails : IDataType
    {
        /// <summary>
        ///     Car plate number
        /// </summary>
        public string CarPlateNumber { get; set; }
        
        /// <summary>
        ///     License
        /// </summary>
        public string License { get; set; }
        
        /// <summary>
        ///     Organisation Number License Holder
        /// </summary>
        public string OrgNumberLicenseHolder { get; set; }
        
        /// <summary>
        ///     Start time
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        ///     Stop time
        /// </summary>
        public DateTime StopTime { get; set; }
        
        /// <summary>
        ///     Tips
        /// </summary>
        public Decimal Tips { get; set; }
        
        /// <summary>
        ///     Total meter price
        /// </summary>
        public Decimal TotalMeterPrice { get; set; }
        
        /// <summary>
        ///     Total distance before boarding in meters
        /// </summary>
        public int TotalDistanceBeforeBoarding { get; set; }
        
        /// <summary>
        ///     Total distance in meters
        /// </summary>
        public int TotalDistance { get; set; }
        
        /// <summary>
        ///     Total distance with passenger in meters
        /// </summary>
        public int TotalDistanceWithPassenger { get; set; }
        
        /// <summary>
        ///     Total time before boarding in seconds
        /// </summary>
        public int TotalTimeBeforeBoarding { get; set; }
        
        /// <summary>
        ///     Total time in seconds
        /// </summary>
        public int TotalTime { get; set; }
        
        /// <summary>
        ///     Total time with passenger in seconds
        /// </summary>
        public int TotalTimeWithPassenger { get; set; }
        
        /// <summary>
        ///     VAT
        /// </summary>
        public VatDetails Vat { get; set; }
        
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal taxiDetails AsDataTransferObject()
        {
            var dto = new taxiDetails
            {
                carPlateNumber = CarPlateNumber,
                license = License,
                orgNumberLicenseHolder = OrgNumberLicenseHolder,
                startTime = StartTime.ToString("O"),
                stopTime = StopTime.ToString("O"),
                tips = Tips.ToString("C"),
                totalMeterPrice = TotalMeterPrice.ToString("C"),
                totalDistanceBeforeBoardingInMeters = TotalDistanceBeforeBoarding,
                totalDistanceInMeters = TotalDistance,
                totalDistanceWithPassengerInMeters = TotalDistanceWithPassenger,
                totalTimeBeforeBoardingInSeconds = TotalTimeBeforeBoarding,
                totalTimeInSeconds = TotalTime,
                totalTimeWithPassengerInSeconds = TotalTimeWithPassenger,
                vat = Vat.AsDataTransferObject()
            };
            return dto;
        }
        
        public override string ToString()
        {
            return $"CarPlateNumber: '{CarPlateNumber}', " +
                   $"License: '{License}', " +
                   $"OrgNumberLicenseHolder: '{OrgNumberLicenseHolder}', " +
                   $"StartTime: '{StartTime}', " +
                   $"StopTime: '{StopTime}', " +
                   $"Tips: '{Tips}', " +
                   $"TotalMeterPrice: '{TotalMeterPrice}', " +
                   $"TotalDistanceBeforeBoarding: '{TotalDistanceBeforeBoarding}', " +
                   $"TotalDistance: '{TotalDistance}', " +
                   $"TotalDistanceWithPassenger: '{TotalDistanceWithPassenger}', " +
                   $"TotalTimeBeforeBoarding: '{TotalTimeBeforeBoarding}', " +
                   $"TotalTime: '{TotalTime}', " +
                   $"TotalTimeWithPassenger: '{TotalTimeWithPassenger}', " +
                   $"Vat: '{Vat}'";
        }
    }
}
