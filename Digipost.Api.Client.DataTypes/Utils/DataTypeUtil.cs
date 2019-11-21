using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Xml;
using Digipost.Api.Client.Resources.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.DataTypes.Utils
{
    public static class DataTypeUtil
    {
        public static DynamicDataType GetDatatypeObject(string typeName)
        {
            ResourceUtility resourceUtility = new ResourceUtility(typeof(DataTypeUtil).GetTypeInfo().Assembly,"Digipost.Api.Client.DataTypes.Resources.XSD");
            
            var bytes = resourceUtility.ReadAllBytes("datatypes.xsd");
            var xml = XmlUtility.ToXmlDocument(Encoding.UTF8.GetString(bytes));

            return CreateObjectFromXmlNode(typeName, GetXmlNode(typeName, xml), xml);
        }

        private static DynamicDataType CreateObjectFromXmlNode(string typeName, XmlNode node, XmlDocument xmlDocument)
        {
            var obj = new DynamicDataType();

            obj.Name = typeName;
            
            foreach (XmlNode child in node.FirstChild.ChildNodes)
            {
                switch (child.Attributes["type"].Value)
                {
                    case "xs:string":
                        AddProperty<string>(obj, child);
                        break;
                    case "xs:int":
                        AddProperty<int>(obj, child);
                        break;
                    case "xs:long":    
                        AddProperty<long>(obj, child);
                        break;
                    case "xs:float":
                        AddProperty<float>(obj, child);
                        break;
                    case "xs:double":
                        AddProperty<double>(obj, child);
                        break;
                    case "xs:decimal":
                        AddProperty<decimal>(obj, child);
                        break;
                    case "xs:boolean":
                        AddProperty<bool>(obj, child);
                        break;
                    case "xs:dateTime":
                        AddProperty<DateTime>(obj, child);
                        break;
                    case "xs:anyURI":
                        AddProperty<Url>(obj, child);
                        break;
                    default:
                        string propName = child.Attributes["name"].Value.Replace("-", "_"); //Otherwise we'd have to escape '-' every time we use the property name.
                        string typePropName = child.Attributes["type"].Value.Substring(4);
                        
                        var subNode = GetXmlNode(typePropName, xmlDocument);

                        bool isList = child.Attributes["maxOccurs"] != null;

                        if (isList)
                        {
                            obj.CreateProperty(propName, new List<DynamicDataType>());
                        }
                        else
                        {
                            obj.CreateProperty(propName, CreateObjectFromXmlNode(typePropName, subNode, xmlDocument));
                        }
                        
                        break;
                }
            }

            return obj;
        }

        private static void AddProperty<T>(DynamicDataType obj, XmlNode child)
        {
            string propName = child.Attributes["name"].Value.Replace("-", "_"); //Otherwise we'd have to escape '-' every time we use the property name.
            bool isList = child.Attributes["maxOccurs"] != null;
            bool isNullable = child.Attributes["minOccurs"] != null && int.Parse(child.Attributes["minOccurs"].Value) == 0;
            T defaultVal = child.Attributes["default"] != null ? (T)Convert.ChangeType(child.Attributes["default"].Value, typeof(T)) : default(T);
            
            if (isList)
            {
                if (child.Attributes["maxOccurs"] != null && child.Attributes["maxOccurs"].Value != "unbounded")
                {
                    List<T> list = Enumerable.Range(0, int.Parse(child.Attributes["maxOccurs"].Value)).Select(s => defaultVal).ToList();
                    obj.CreateProperty(propName, list);
                }
                else
                {
                    obj.CreateProperty(propName, new List<T>());
                }
            }
            else
            {
                if (isNullable)
                {
                    obj.CreateNullableProperty(propName);
                }
                else
                {
                    obj.CreateProperty(propName, defaultVal);
                }
            }
        }
        
        private static XmlNode GetXmlNode(string typeName, XmlDocument xmlDocument)
        {
            XmlNodeList childNodes = xmlDocument.DocumentElement.ChildNodes;
            XmlNode node = null;
            foreach (XmlNode child in childNodes)
            {
                XmlAttribute nameAttribute = child.Attributes["name"];
                if (nameAttribute.Value == typeName)
                {
                    node = child;
                }
            }

            if (node == null)
            {
                throw new Exception("No object found for type name: " + typeName);
            }
            
            return node;
        }
        
        private static string JoinWithBasePath(params string[] path)
        {
            List<string> stringList = new List<string>
            {
                IncludeProjectRootNameInBasePath(typeof(DataTypeUtil).GetTypeInfo().Assembly, "Digipost.Api.Client.DataTypes.Resources.XSD")
            };
            stringList.AddRange(path);
            return string.Join(".", stringList);
        }
        
        private static string IncludeProjectRootNameInBasePath(Assembly currentAssembly, string basePathForResources)
        {
            string name = currentAssembly.GetName().Name;
            string str;
            if (basePathForResources.Contains(name))
                str = basePathForResources;
            else
                str = string.Join(".", name, basePathForResources);
            return str;
        }
    }
}
