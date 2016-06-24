using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Digipost.Api.Client.Domain.Exceptions;

namespace Digipost.Api.Client.Domain.DataTransferObjects
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(TypeName = "print-recipient", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-recipient", Namespace = "http://api.digipost.no/schema/v6", IsNullable = true)]
    public abstract class PrintDataTransferObject
    {
        private object _address;

        protected PrintDataTransferObject(string name,
            ForeignAddressDataTransferObject foreignAddressDataTransferObject)
        {
            Name = name;
            Address = foreignAddressDataTransferObject;
        }

        protected PrintDataTransferObject(string name,
            NorwegianAddressDataTransferObject norwegianAddressDataTransferObject)
        {
            Name = name;
            Address = norwegianAddressDataTransferObject;
        }

        protected PrintDataTransferObject()
        {
            /* Must exist for serialization */
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("foreign-address", typeof (ForeignAddressDataTransferObject))]
        [XmlElement("norwegian-address", typeof (NorwegianAddressDataTransferObject))]
        public object Address
        {
            get { return _address; }
            set
            {
                if (!(value is ForeignAddressDataTransferObject) && !(value is NorwegianAddressDataTransferObject))
                    throw new ApiException("Invalid type of Address! Valid types are [NorwegianAddress,Foreignaddress]");

                _address = value;
            }
        }
    }
}