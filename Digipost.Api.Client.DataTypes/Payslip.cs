using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public class Payslip : IDataType
    {
        public XmlElement Serialize()
        {
            return DataTypeSerialization.Serialize(AsDataTransferObject());
        }
        
        internal payslip AsDataTransferObject()
        {
            var dto = new payslip {};
            return dto;
        }
        
        public override string ToString()
        {
            return $"Payslip";
        }
    }
}
