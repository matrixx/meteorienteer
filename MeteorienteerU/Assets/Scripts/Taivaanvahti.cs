using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Taivaanvahti : MonoBehaviour
{
	public string URL = "https://www.taivaanvahti.fi/api";
	public int category = 1;
	
	private string gotFormString;
	private string sendString;
	
	public TaivaanvahtiForm Form {get; private set;}
	public bool FormReady {get; private set;}
	
	private Dictionary<TaivaanvahtiField, string> sendForm;
	
	private string sendResponse = "";
	
	public void GetForm()
	{
		FormReady = false;
		Form = null;
		StartCoroutine("GetFormCR");
	}
	
	public void SubmitForm(Dictionary<TaivaanvahtiField, string> sendForm)
	{
		sendResponse = "";
		this.sendForm = sendForm;
		StartCoroutine("SubmitFormCR");
	}
	
	IEnumerator GetFormCR()
	{
		string request = "<Request><Action>FormTemplateRequest</Action>" +
                                    "<Category>" + category + "</Category>" +
                                    "</Request>";
		Hashtable headers = new Hashtable();
		headers["Content-Type"] = "text/xml";
		WWW www = new WWW(URL, System.Text.Encoding.UTF8.GetBytes(request), headers);
		yield return www;
		gotFormString = www.text;
		Debug.Log(gotFormString);
		
		
	    TaivaanvahtiForm form = new TaivaanvahtiForm();
		List<TaivaanvahtiField> fields = new List<TaivaanvahtiField>();
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(gotFormString);
		XmlElement docElem = xmlDoc.DocumentElement;
		XmlNode categoryNode = docElem.FirstChild;
		if (categoryNode != null)
		{
			while (categoryNode != null)
			{
				if (categoryNode.Name == "observation" || categoryNode.Name == "category")
				{
					HandleCategory(categoryNode, fields, form);
				}
				categoryNode = categoryNode.NextSibling;
			}
		}
		form.SetFields(fields);
		Form = form;
		FormReady = true;
	}
	
	void HandleCategory(XmlNode categoryNode, List<TaivaanvahtiField> fields, TaivaanvahtiForm form)
	{
	    XmlNode e = categoryNode.FirstChild;
	    while(e != null)
		{
	        if (e.Name == "field" || e.Name == "specific")
			{
	            TaivaanvahtiField field = new TaivaanvahtiField(form);
	            field.ParseFieldElement(e);
	            fields.Add(field);
	        }
	        e = e.NextSibling;
	    }
	}
	
	IEnumerator SubmitFormCR()
	{
		Hashtable headers = new Hashtable();
		headers["Content-Type"] = "text/xml";
		
		string outString;
		XmlDocument doc = new XmlDocument();
		XmlElement requestElement = doc.CreateElement("Request");
		doc.AppendChild(requestElement);
		XmlElement actionElement = doc.CreateElement("Action");
		requestElement.AppendChild(actionElement);
		XmlText actionText = doc.CreateTextNode("ObservationAddRequest");
	    actionElement.AppendChild(actionText);
	    XmlElement observationElement = doc.CreateElement("observation");
	    requestElement.AppendChild(observationElement);
	    foreach(TaivaanvahtiField field in sendForm.Keys)
		{
	//        if(field->isMandatory())
	            field.CreateFieldElement(observationElement, sendForm[field], doc);
	    }
	    // Create category id
	    XmlElement fieldElement = doc.CreateElement("field");
	    observationElement.AppendChild(fieldElement);
	
	    XmlElement fieldIdElement = doc.CreateElement("field_id");
	    fieldElement.AppendChild(fieldIdElement);
	
	    XmlText fieldIdText = doc.CreateTextNode("category_id");
	    fieldIdElement.AppendChild(fieldIdText);
	
	    XmlElement fieldValueElement = doc.CreateElement("field_value");
	    fieldElement.AppendChild(fieldValueElement);
	    XmlText fieldValueText = doc.CreateTextNode(category.ToString());
	    fieldValueElement.AppendChild(fieldValueText);
	    // end category id
	    // add category
	    XmlElement categoryElement = doc.CreateElement("category");
	    observationElement.AppendChild(categoryElement);
	    TaivaanvahtiField.CreateFieldElement(categoryElement, "category_id", "tulipallo", doc);
	    TaivaanvahtiField.CreateFieldElement(categoryElement, "specific_havaintokategoria", "Tulipallo", doc);
	    // end category
	    outString = doc.InnerXml;
		
		//Debug.Log(outString);
		
		WWW www = new WWW(URL, System.Text.Encoding.UTF8.GetBytes(outString), headers);
		yield return www;
		sendResponse = www.text;
	}
	
	public enum SendResult {None, Success, MissingFields, OtherError};
	
	public SendResult OngoingSendResult()
	{
		if (sendResponse == "")
		{
			return SendResult.None;
		}
		else if (sendResponse.Contains("<response_type>Success</response_type>"))
		{
			return SendResult.Success;
		}
		else
		{
			if (sendResponse.Contains("field is required"))
			{
				return SendResult.MissingFields;
			}
			else
			{
				return SendResult.OtherError;
			}
		}
	}
}


