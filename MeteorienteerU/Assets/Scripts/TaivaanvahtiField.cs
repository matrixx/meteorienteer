using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class TaivaanvahtiField
{
 	public enum FieldType {
        TYPE_NOT_SET,
        TYPE_TEXT,
        TYPE_CHECKBOX,
        TYPE_SELECTION,
        TYPE_DATE,
        TYPE_COORDINATE,
        TYPE_TIME
    };
	
	public string id {get; set;}
	public string label {get; private set;}
	public string info {get; private set;}
	public string infoUrl {get; private set;}
	public bool isMandatory {get; private set;}
	public FieldType type {get; private set;}
	public Dictionary<string, string> values {get; private set;}
		
	public TaivaanvahtiField(TaivaanvahtiForm form)
	{
	}
	
	public void ParseFieldElement(XmlNode node)
	{
		id = node["field_id"].InnerText;
	    label = node["field_label"].InnerText;
		isMandatory = node["field_mandatory"].InnerText == "1";
		info = node["field_info"].InnerText;
		infoUrl = node["field_info_url"].InnerText;
		
		type = FieldType.TYPE_NOT_SET;
	    string fieldTypeString = node["field_type"].InnerText;
	    if(fieldTypeString=="text") type = FieldType.TYPE_TEXT;
	    if(fieldTypeString.ToLower()=="date") type = FieldType.TYPE_DATE;
	    if(fieldTypeString.ToLower()=="time") type = FieldType.TYPE_TIME;
	    if(fieldTypeString=="coordinate") type = FieldType.TYPE_COORDINATE;
	    if(fieldTypeString=="checkbox") type = FieldType.TYPE_CHECKBOX;
	    if(fieldTypeString=="select") type = FieldType.TYPE_SELECTION;
	    if(fieldTypeString=="specific_tulipallon_kirkkaus") type = FieldType.TYPE_SELECTION;
	    if(type == FieldType.TYPE_NOT_SET) {
       	 Debug.Log("Warning: field type " + fieldTypeString + " not handled!");
    	}
		
		XmlNode valuesNode = node["values"];
		if (valuesNode != null)
		{
			values = new Dictionary<string, string>();
			XmlNode valueNode = valuesNode["value"];
			while (valueNode != null)
			{
				values.Add(valueNode["value_id"].InnerText, valueNode["value_name"].InnerText);
				do
				{
					valueNode = valueNode.NextSibling;
				}
				while (valueNode != null && valueNode.Name != "value");
			}
		}
		
		if (values != null && values.Count > 0) type = FieldType.TYPE_SELECTION;
	}
	
    public void CreateFieldElement(XmlElement elem, string valuestr, XmlDocument doc)
	{
	    string idString = id;
    	// Workarounds:
	    if(idString == "observation_start_hours") idString = "start_hours";
	    if(idString == "observation_end_hours") idString = "end_hours";
	    CreateFieldElement(elem, idString, valuestr, doc);
	}
    
	public static void CreateFieldElement(XmlElement elem, string name, string valuestr, XmlDocument doc)
	{
	    XmlElement fieldElement = doc.CreateElement("field");
	    elem.AppendChild(fieldElement);
	
	    XmlElement fieldIdElement = doc.CreateElement("field_id");
	    fieldElement.AppendChild(fieldIdElement);
	
	    XmlText fieldIdText = doc.CreateTextNode(name);
	    fieldIdElement.AppendChild(fieldIdText);
	
	    XmlElement fieldValueElement = doc.CreateElement("field_value");
	    fieldElement.AppendChild(fieldValueElement);
	    XmlText fieldValueText = doc.CreateTextNode(valuestr);
	    fieldValueElement.AppendChild(fieldValueText);
	}
}
