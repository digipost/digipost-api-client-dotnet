using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName = "sms-notification", Namespace = "http://api.digipost.no/schema/v6")]
    public class Smsnotification
    {
        public Smsnotification(int afterhours)
        {
            Afterhours = new List<int> {afterhours};
        }

        public Smsnotification(DateTime sendingTime)
        {
            At = new List<Listedtime> {new Listedtime(sendingTime)};
        }

        private Smsnotification()
        {
            Afterhours = new List<int>();
            At = new List<Listedtime>();
        }

        [XmlElement("at")]
        public List<Listedtime> At { get; set; }

        [XmlElement("after-hours")]
        public List<int> Afterhours { get; set; }
    }

    public class Listedtime
    {
        private Listedtime()
        {
            /**Must exist for serialization.**/ 
        }

        public Listedtime(DateTime time)
        {
            Time = time;
        }

        [XmlAttribute("time")]
        public DateTime Time { get; set; }
    }
}