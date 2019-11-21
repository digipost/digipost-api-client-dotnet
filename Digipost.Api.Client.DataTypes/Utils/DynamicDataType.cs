using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Digipost.Api.Client.DataTypes.Utils
{
    [Serializable]
    [XmlRoot(ElementName = "datatype", Namespace = "http://api.digipost.no/schema/datatypes")]
    [XmlTypeAttribute("datatype", Namespace="http://api.digipost.no/schema/datatypes")]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class DynamicDataType : DynamicObject, IXmlSerializable
    {
        public string Name { get; set; }
        
        Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }

            result = "";
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (properties.ContainsKey(binder.Name))
            {
                properties[binder.Name] = value;
                return true;
            }

            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }

        public void CreateNullableProperty(string propName)
        {
            properties[propName] = null;
        }

        public void CreateProperty<T>(string propName, T propValue)
        {
            properties[propName] = propValue;
        }

        public XmlSchema GetSchema()
        {
            return(null);
        }

        public void ReadXml(XmlReader reader)
        {
            return;
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var kvp in properties)
            {
                if (kvp.Value != null)
                {
                    XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(kvp.Key);
                    xmlRootAttribute.Namespace = "http://api.digipost.no/schema/datatypes";
                    
                    var serializer = new XmlSerializer(kvp.Value.GetType(), xmlRootAttribute);
                    serializer.Serialize(writer, kvp.Value);
                }
            }
        }
    }
}
