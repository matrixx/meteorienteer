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
	
	public void GetForm()
	{
		FormReady = false;
		Form = null;
		StartCoroutine("GetFormCR");
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
				Debug.Log("categoryNode.Name: " + categoryNode.Name);
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
			Debug.Log(e.Name);
			Debug.Log(e.LocalName);
	        if (e.Name == "field" || e.Name == "specific")
			{
	            TaivaanvahtiField field = new TaivaanvahtiField(form);
	            field.ParseFieldElement(e);
	            fields.Add(field);
	        }
	        e = e.NextSibling;
	    }
	}
}


