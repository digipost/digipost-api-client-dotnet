﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1047.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://api.digipost.no/schema/datatypes", IsNullable=false)]
public partial class @event {
    
    private eventTimeSpan[] timeField;
    
    private string subTitleField;
    
    private string timeLabelField;
    
    private string descriptionField;
    
    private string placeField;
    
    private string placeLabelField;
    
    private eventAddress addressField;
    
    private info[] infoField;
    
    private string barcodeLabelField;
    
    private barcode barcodeField;
    
    private externalLink[] linksField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("time")]
    public eventTimeSpan[] time {
        get {
            return this.timeField;
        }
        set {
            this.timeField = value;
        }
    }
    
    /// <remarks/>
    public string subTitle {
        get {
            return this.subTitleField;
        }
        set {
            this.subTitleField = value;
        }
    }
    
    /// <remarks/>
    public string timeLabel {
        get {
            return this.timeLabelField;
        }
        set {
            this.timeLabelField = value;
        }
    }
    
    /// <remarks/>
    public string description {
        get {
            return this.descriptionField;
        }
        set {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    public string place {
        get {
            return this.placeField;
        }
        set {
            this.placeField = value;
        }
    }
    
    /// <remarks/>
    public string placeLabel {
        get {
            return this.placeLabelField;
        }
        set {
            this.placeLabelField = value;
        }
    }
    
    /// <remarks/>
    public eventAddress address {
        get {
            return this.addressField;
        }
        set {
            this.addressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("info")]
    public info[] info {
        get {
            return this.infoField;
        }
        set {
            this.infoField = value;
        }
    }
    
    /// <remarks/>
    public string barcodeLabel {
        get {
            return this.barcodeLabelField;
        }
        set {
            this.barcodeLabelField = value;
        }
    }
    
    /// <remarks/>
    public barcode barcode {
        get {
            return this.barcodeField;
        }
        set {
            this.barcodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("links")]
    public externalLink[] links {
        get {
            return this.linksField;
        }
        set {
            this.linksField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class eventTimeSpan {
    
    private System.DateTime starttimeField;
    
    private System.DateTime endtimeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("start-time")]
    public System.DateTime starttime {
        get {
            return this.starttimeField;
        }
        set {
            this.starttimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("end-time")]
    public System.DateTime endtime {
        get {
            return this.endtimeField;
        }
        set {
            this.endtimeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class heftelse {
    
    private string panthaverField;
    
    private string typepantField;
    
    private string beloepField;
    
    /// <remarks/>
    public string panthaver {
        get {
            return this.panthaverField;
        }
        set {
            this.panthaverField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("type-pant")]
    public string typepant {
        get {
            return this.typepantField;
        }
        set {
            this.typepantField = value;
        }
    }
    
    /// <remarks/>
    public string beloep {
        get {
            return this.beloepField;
        }
        set {
            this.beloepField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class omsetningshistorikk {
    
    private string datoField;
    
    private string beskrivelseField;
    
    private string selgerField;
    
    private string kjoeperField;
    
    private long beloepField;
    
    private bool beloepFieldSpecified;
    
    /// <remarks/>
    public string dato {
        get {
            return this.datoField;
        }
        set {
            this.datoField = value;
        }
    }
    
    /// <remarks/>
    public string beskrivelse {
        get {
            return this.beskrivelseField;
        }
        set {
            this.beskrivelseField = value;
        }
    }
    
    /// <remarks/>
    public string selger {
        get {
            return this.selgerField;
        }
        set {
            this.selgerField = value;
        }
    }
    
    /// <remarks/>
    public string kjoeper {
        get {
            return this.kjoeperField;
        }
        set {
            this.kjoeperField = value;
        }
    }
    
    /// <remarks/>
    public long beloep {
        get {
            return this.beloepField;
        }
        set {
            this.beloepField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool beloepSpecified {
        get {
            return this.beloepFieldSpecified;
        }
        set {
            this.beloepFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class hjemmelshaver {
    
    private string nameField;
    
    private string emailField;
    
    /// <remarks/>
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    public string email {
        get {
            return this.emailField;
        }
        set {
            this.emailField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class matrikkel {
    
    private string kommunenummerField;
    
    private string gaardsnummerField;
    
    private string bruksnummerField;
    
    private string festenummerField;
    
    private string seksjonsnummerField;
    
    /// <remarks/>
    public string kommunenummer {
        get {
            return this.kommunenummerField;
        }
        set {
            this.kommunenummerField = value;
        }
    }
    
    /// <remarks/>
    public string gaardsnummer {
        get {
            return this.gaardsnummerField;
        }
        set {
            this.gaardsnummerField = value;
        }
    }
    
    /// <remarks/>
    public string bruksnummer {
        get {
            return this.bruksnummerField;
        }
        set {
            this.bruksnummerField = value;
        }
    }
    
    /// <remarks/>
    public string festenummer {
        get {
            return this.festenummerField;
        }
        set {
            this.festenummerField = value;
        }
    }
    
    /// <remarks/>
    public string seksjonsnummer {
        get {
            return this.seksjonsnummerField;
        }
        set {
            this.seksjonsnummerField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class residenceAddress {
    
    private string unitnumberField;
    
    private string housenumberField;
    
    private string streetnameField;
    
    private string postalcodeField;
    
    private string cityField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("unit-number")]
    public string unitnumber {
        get {
            return this.unitnumberField;
        }
        set {
            this.unitnumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("house-number")]
    public string housenumber {
        get {
            return this.housenumberField;
        }
        set {
            this.housenumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("street-name")]
    public string streetname {
        get {
            return this.streetnameField;
        }
        set {
            this.streetnameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("postal-code")]
    public string postalcode {
        get {
            return this.postalcodeField;
        }
        set {
            this.postalcodeField = value;
        }
    }
    
    /// <remarks/>
    public string city {
        get {
            return this.cityField;
        }
        set {
            this.cityField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class appointmentAddress {
    
    private string streetaddressField;
    
    private string postalcodeField;
    
    private string cityField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("street-address")]
    public string streetaddress {
        get {
            return this.streetaddressField;
        }
        set {
            this.streetaddressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("postal-code")]
    public string postalcode {
        get {
            return this.postalcodeField;
        }
        set {
            this.postalcodeField = value;
        }
    }
    
    /// <remarks/>
    public string city {
        get {
            return this.cityField;
        }
        set {
            this.cityField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class barcode {
    
    private string barcodeValueField;
    
    private string barcodeTypeField;
    
    private string barcodeTextField;
    
    private bool showValueInBarcodeField;
    
    private bool showValueInBarcodeFieldSpecified;
    
    /// <remarks/>
    public string barcodeValue {
        get {
            return this.barcodeValueField;
        }
        set {
            this.barcodeValueField = value;
        }
    }
    
    /// <remarks/>
    public string barcodeType {
        get {
            return this.barcodeTypeField;
        }
        set {
            this.barcodeTypeField = value;
        }
    }
    
    /// <remarks/>
    public string barcodeText {
        get {
            return this.barcodeTextField;
        }
        set {
            this.barcodeTextField = value;
        }
    }
    
    /// <remarks/>
    public bool showValueInBarcode {
        get {
            return this.showValueInBarcodeField;
        }
        set {
            this.showValueInBarcodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool showValueInBarcodeSpecified {
        get {
            return this.showValueInBarcodeFieldSpecified;
        }
        set {
            this.showValueInBarcodeFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class info {
    
    private string titleField;
    
    private string textField;
    
    /// <remarks/>
    public string title {
        get {
            return this.titleField;
        }
        set {
            this.titleField = value;
        }
    }
    
    /// <remarks/>
    public string text {
        get {
            return this.textField;
        }
        set {
            this.textField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
public partial class eventAddress {
    
    private string streetaddressField;
    
    private string postalcodeField;
    
    private string cityField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("street-address")]
    public string streetaddress {
        get {
            return this.streetaddressField;
        }
        set {
            this.streetaddressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("postal-code")]
    public string postalcode {
        get {
            return this.postalcodeField;
        }
        set {
            this.postalcodeField = value;
        }
    }
    
    /// <remarks/>
    public string city {
        get {
            return this.cityField;
        }
        set {
            this.cityField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://api.digipost.no/schema/datatypes", IsNullable=false)]
public partial class externalLink {
    
    private string urlField;
    
    private System.DateTime deadlineField;
    
    private bool deadlineFieldSpecified;
    
    private string descriptionField;
    
    private string buttontextField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
    public string url {
        get {
            return this.urlField;
        }
        set {
            this.urlField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime deadline {
        get {
            return this.deadlineField;
        }
        set {
            this.deadlineField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool deadlineSpecified {
        get {
            return this.deadlineFieldSpecified;
        }
        set {
            this.deadlineFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string description {
        get {
            return this.descriptionField;
        }
        set {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("button-text")]
    public string buttontext {
        get {
            return this.buttontextField;
        }
        set {
            this.buttontextField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://api.digipost.no/schema/datatypes", IsNullable=false)]
public partial class appointment {
    
    private string starttimeField;
    
    private string endtimeField;
    
    private string arrivaltimeField;
    
    private string placeField;
    
    private appointmentAddress addressField;
    
    private string subtitleField;
    
    private info[] infoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("start-time")]
    public string starttime {
        get {
            return this.starttimeField;
        }
        set {
            this.starttimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("end-time")]
    public string endtime {
        get {
            return this.endtimeField;
        }
        set {
            this.endtimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("arrival-time")]
    public string arrivaltime {
        get {
            return this.arrivaltimeField;
        }
        set {
            this.arrivaltimeField = value;
        }
    }
    
    /// <remarks/>
    public string place {
        get {
            return this.placeField;
        }
        set {
            this.placeField = value;
        }
    }
    
    /// <remarks/>
    public appointmentAddress address {
        get {
            return this.addressField;
        }
        set {
            this.addressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("sub-title")]
    public string subtitle {
        get {
            return this.subtitleField;
        }
        set {
            this.subtitleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("info")]
    public info[] info {
        get {
            return this.infoField;
        }
        set {
            this.infoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://api.digipost.no/schema/datatypes", IsNullable=false)]
public partial class boligdetaljer {
    
    private residence residenceField;
    
    private hjemmelshaver[] hjemmelshavereField;
    
    private int bruksarealField;
    
    private bool bruksarealFieldSpecified;
    
    private int antalloppholdsromField;
    
    private bool antalloppholdsromFieldSpecified;
    
    private int antallbaderomField;
    
    private bool antallbaderomFieldSpecified;
    
    private omsetningshistorikk[] omsetningshistorikkField;
    
    private string organisasjonsnummerField;
    
    private string bruksenhetField;
    
    private string andelsnummerField;
    
    private heftelse[] heftelserField;
    
    /// <remarks/>
    public residence residence {
        get {
            return this.residenceField;
        }
        set {
            this.residenceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("hjemmelshavere")]
    public hjemmelshaver[] hjemmelshavere {
        get {
            return this.hjemmelshavereField;
        }
        set {
            this.hjemmelshavereField = value;
        }
    }
    
    /// <remarks/>
    public int bruksareal {
        get {
            return this.bruksarealField;
        }
        set {
            this.bruksarealField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool bruksarealSpecified {
        get {
            return this.bruksarealFieldSpecified;
        }
        set {
            this.bruksarealFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("antall-oppholdsrom")]
    public int antalloppholdsrom {
        get {
            return this.antalloppholdsromField;
        }
        set {
            this.antalloppholdsromField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool antalloppholdsromSpecified {
        get {
            return this.antalloppholdsromFieldSpecified;
        }
        set {
            this.antalloppholdsromFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("antall-baderom")]
    public int antallbaderom {
        get {
            return this.antallbaderomField;
        }
        set {
            this.antallbaderomField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool antallbaderomSpecified {
        get {
            return this.antallbaderomFieldSpecified;
        }
        set {
            this.antallbaderomFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("omsetningshistorikk")]
    public omsetningshistorikk[] omsetningshistorikk {
        get {
            return this.omsetningshistorikkField;
        }
        set {
            this.omsetningshistorikkField = value;
        }
    }
    
    /// <remarks/>
    public string organisasjonsnummer {
        get {
            return this.organisasjonsnummerField;
        }
        set {
            this.organisasjonsnummerField = value;
        }
    }
    
    /// <remarks/>
    public string bruksenhet {
        get {
            return this.bruksenhetField;
        }
        set {
            this.bruksenhetField = value;
        }
    }
    
    /// <remarks/>
    public string andelsnummer {
        get {
            return this.andelsnummerField;
        }
        set {
            this.andelsnummerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("heftelser")]
    public heftelse[] heftelser {
        get {
            return this.heftelserField;
        }
        set {
            this.heftelserField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1047.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.digipost.no/schema/datatypes")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://api.digipost.no/schema/datatypes", IsNullable=false)]
public partial class residence {
    
    private residenceAddress addressField;
    
    private matrikkel matrikkelField;
    
    private string sourceField;
    
    private string externalidField;
    
    /// <remarks/>
    public residenceAddress address {
        get {
            return this.addressField;
        }
        set {
            this.addressField = value;
        }
    }
    
    /// <remarks/>
    public matrikkel matrikkel {
        get {
            return this.matrikkelField;
        }
        set {
            this.matrikkelField = value;
        }
    }
    
    /// <remarks/>
    public string source {
        get {
            return this.sourceField;
        }
        set {
            this.sourceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("external-id")]
    public string externalid {
        get {
            return this.externalidField;
        }
        set {
            this.externalidField = value;
        }
    }
}
