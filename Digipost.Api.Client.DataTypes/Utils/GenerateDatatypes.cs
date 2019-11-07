using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Api.Client.DataTypes.Utils
{
    public class GenerateDatatypes
    {
        
        static void Main(string[] args)
        {
            GetXmlNode("proof");
        }

        private static Object GetXmlNode(string typeName)
        {
            FileStream fileStream = new FileStream("/Users/aaronzachariaharrick/digipost/digipost-api-client-dotnet/Digipost.Api.Client.DataTypes/Resources/XSD/datatypes.xsd", FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            XmlReader reader = XmlReader.Create(streamReader);
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(reader);

            XmlNodeList childNodes = xmlDocument.DocumentElement.ChildNodes;
            XmlNode node = null;
            foreach (XmlNode child in childNodes)
            {
                XmlAttribute nameAttribute = child.Attributes["name"];
                if (nameAttribute.Value == typeName)
                {
                    node = child; //Why no break? Because we have the second node of this name, the first one is just a definition.
                }
            }

            var obj = new ExpandoObject() as IDictionary<string, object>;

            if (node != null)
            {
                foreach (XmlNode child in node.FirstChild.ChildNodes)
                {
                    string propName = child.Attributes["name"].Value;
                    switch (child.Attributes["type"].Value)
                    {
                        case "xs:string":
                            obj.Add(propName, "");
                            break;
                        case "xs:int":
                            obj.Add(propName, 0);
                            break;
                    }
                }
            }

            return obj;
        }
    }
}
