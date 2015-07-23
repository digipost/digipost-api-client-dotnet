using System.Xml;
using Digipost.Api.Client.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Api.Client.Tests.Unittest
{
    [TestClass]
    public class DigipostApiTests
    {
        [TestClass]
        public class ValidateMethod
        {
            [TestMethod]
            public void EmptyXmlDocumentIsAccepted()
            {
                DigipostApi.ValidateXml(new XmlDocument());
            }

            private const string ValidMessageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Kristian Sæther Enge</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Kristian Sæther Enge</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>0f8fad5b-d9cb-469f-a165-70867728950e</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment><uuid>0f8fad5b-d9cb-469f-a165-70867728951f</uuid><subject>attachment</subject><file-type>txt</file-type><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></attachment></message>";

            [TestMethod]
            public void ValidMessageXmlIsAccepted()
            {
                var messageXmlDocument = new XmlDocument();
                messageXmlDocument.LoadXml(ValidMessageBlueprint);
                DigipostApi.ValidateXml(messageXmlDocument);       
            }

            private const string InvalidMessageBlueprint = @"<?xml version=""1.0"" encoding=""utf-8""?><message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.digipost.no/schema/v6""><recipient><name-and-address><fullname>Kristian Sæther Enge</fullname><addressline1>Colletts gate 68</addressline1><postalcode>0460</postalcode><city>Oslo</city></name-and-address><print-details><recipient><name>Kristian Sæther Enge</name><norwegian-address><addressline1>Collettsgate 68</addressline1><addressline2>Leil h401</addressline2><addressline3>dør 2</addressline3><zip-code>0460</zip-code><city>Oslo</city></norwegian-address></recipient><return-address><name>Ola Digipost</name><foreign-address><addressline1>svenskegatan 1</addressline1><addressline2> leil h101</addressline2><addressline3>pb 12</addressline3><addressline4>skuff 3</addressline4><country>SE</country></foreign-address></return-address><post-type>B</post-type><color>MONOCHROME</color></print-details></recipient><primary-document><uuid>0f8fad5b-d9cb-469f-a165-70867728950e</uuid><subject>Subject</subject><file-type>txt</file-type><sms-notification><after-hours>2</after-hours></sms-notification><authentication-level>COSMIC(TOP SECRET)</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></primary-document><attachment><uuid>123456</uuid><subject>attachment</subject><file-type>txt</file-type><authentication-level>TWO_FACTOR</authentication-level><sensitivity-level>SENSITIVE</sensitivity-level></attachment></message>";

            [TestMethod]
            [ExpectedException(typeof(XmlException))]
            public void InvalidMessageXmlThrowsException()
            {
                var messageXmlDocument = new XmlDocument();
                messageXmlDocument.LoadXml(InvalidMessageBlueprint);
                DigipostApi.ValidateXml(messageXmlDocument);  
            }

            private const string ValidIdentificationBlueprint = "<?xml version=\"1.0\" encoding=\"utf-8\"?><identification xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://api.digipost.no/schema/v6\"><personal-identification-number>31108446911</personal-identification-number></identification>";

            [TestMethod]
            public void ValidIdentificationXmlIsAccepted()
            {
                var identificationXmlDocument = new XmlDocument();
                identificationXmlDocument.LoadXml(ValidIdentificationBlueprint);
                DigipostApi.ValidateXml(identificationXmlDocument);
            }

            private const string InvalidIdentificationBlueprint = "<?xml version=\"1.0\" encoding=\"utf-8\"?><identification xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://api.digipost.no/schema/v6\"><personal-identification-number>134</personal-identification-number></identification>";

            [TestMethod]
            [ExpectedException(typeof(XmlException))]
            public void InvalidIdentificationXmlThrowsException()
            {
                var identificationXmlDocument = new XmlDocument();
                identificationXmlDocument.LoadXml(InvalidIdentificationBlueprint);
                DigipostApi.ValidateXml(identificationXmlDocument);
            }
        }
    }
}
