﻿using UnityEngine;
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
	public enum SendStatus {Idle, Sending, Success, MissingFields, ImageSendFailed, OtherError};
	
	private SendStatus sendStatus = SendStatus.Idle;
	
	private Dictionary<TaivaanvahtiField, string> sendForm;
	
	
	
	private Texture2D sendImage;
	
	public void GetForm()
	{
		FormReady = false;
		Form = null;
		StartCoroutine("GetFormCR");
	}
	
	public void SubmitForm(Dictionary<TaivaanvahtiField, string> sendForm, Texture2D sendImage)
	{
		sendStatus = SendStatus.Sending;
		this.sendForm = sendForm;
		this.sendImage = sendImage;
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
		
		Debug.Log(outString);
		
		WWW www = new WWW(URL, System.Text.Encoding.UTF8.GetBytes(outString), headers);
		yield return www;
		
		Debug.Log(www.text);
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(www.text);
		XmlElement responseElement = xmlDoc.DocumentElement;
		sendStatus = SendStatus.OtherError;
		if (responseElement.Name == "response")
		{
			string response_type = responseElement["response_type"].InnerText;
			if (response_type == "Success")
			{
				string observation_id = responseElement["observation_id"].InnerText;
				string observation_modification_key = responseElement["observation_modification_key"].InnerText;
				
								
				XmlDocument imageSendDoc = new XmlDocument();
				
				XmlElement imageRequestElement = imageSendDoc.CreateElement("Request");
				imageSendDoc.AppendChild(imageRequestElement);
				XmlElement imageActionElement = imageSendDoc.CreateElement("Action");
				imageRequestElement.AppendChild(imageActionElement);
				XmlText imageActionText = imageSendDoc.CreateTextNode("ObservationAddImageRequest");
			    imageActionElement.AppendChild(imageActionText);
				
				XmlElement imageElement = imageSendDoc.CreateElement("image");
				imageRequestElement.AppendChild(imageElement);
				
				XmlElement obsIdElement = imageSendDoc.CreateElement("observation_id");
				XmlText obIdText = imageSendDoc.CreateTextNode(observation_id);
				obsIdElement.AppendChild(obIdText);
				imageElement.AppendChild(obsIdElement);
				
				XmlElement obsModKeyElement = imageSendDoc.CreateElement("observation_modification_key");
				XmlText obModKeyText = imageSendDoc.CreateTextNode(observation_modification_key);
				obsModKeyElement.AppendChild(obModKeyText);
				imageElement.AppendChild(obsModKeyElement);
				
				XmlElement obsImageElement = imageSendDoc.CreateElement("observation_image");
				XmlText obImageText = imageSendDoc.CreateTextNode("1");
				obsImageElement.AppendChild(obImageText);
				imageElement.AppendChild(obsImageElement);
			
				XmlElement obsImageNameElement = imageSendDoc.CreateElement("image_name");
				XmlText obImageNameText = imageSendDoc.CreateTextNode("meteorienteer_fireball.jpg");
				obsImageNameElement.AppendChild(obImageNameText);
				imageElement.AppendChild(obsImageNameElement);
				
				XmlElement obsMimetypeElement = imageSendDoc.CreateElement("mimetype");
				XmlText obMimetypeText = imageSendDoc.CreateTextNode("image/jpeg");
				obsMimetypeElement.AppendChild(obMimetypeText);
				imageElement.AppendChild(obsMimetypeElement);
							
				XmlElement obsEncodingElement = imageSendDoc.CreateElement("encoding");
				XmlText obEncodingText = imageSendDoc.CreateTextNode("Base64");
				obsEncodingElement.AppendChild(obEncodingText);
				imageElement.AppendChild(obsEncodingElement);
				
				JPGEncoder jpgEncoder = new JPGEncoder(sendImage.GetPixels(), sendImage.width, sendImage.height, 90f);
				jpgEncoder.doEncoding();
				byte[] imageBytes = jpgEncoder.GetBytes();
				
				XmlElement obsImageDataElement = imageSendDoc.CreateElement("image_data");
				XmlText obImageDataText = imageSendDoc.CreateTextNode(System.Convert.ToBase64String(imageBytes));
				obsImageDataElement.AppendChild(obImageDataText);
				imageElement.AppendChild(obsImageDataElement);
				
				string imageOutString = imageSendDoc.InnerXml;
				Debug.Log(imageOutString);
				
				sendStatus = SendStatus.Sending;
				
				www = new WWW(URL, System.Text.Encoding.UTF8.GetBytes(imageOutString), headers);
				yield return www;
				Debug.Log(www.text);
				xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(www.text);
				responseElement = xmlDoc.DocumentElement;
				sendStatus = SendStatus.ImageSendFailed;
				if (responseElement.Name == "response")
				{
					response_type = responseElement["response_type"].InnerText;
					if (response_type == "Success")
					{
						sendStatus = SendStatus.Success;
					}	
				}
			}
			else
			{
				if (responseElement["field_id"] != null)
				{
					sendStatus = SendStatus.MissingFields;
				}
			}
		}
		
		
//		if (categoryNode != null)
//		{
//			while (categoryNode != null)
//			{
//				if (categoryNode.Name == "observation" || categoryNode.Name == "category")
//				{
//					HandleCategory(categoryNode, fields, form);
//				}
//				categoryNode = categoryNode.NextSibling;
//			}
//		}
	}
	
	public SendStatus CurrentSendStatus()
	{
		/*if (sendResponse == "")
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
		}*/
		return sendStatus;
	}
	
	public void Reset()
	{
		sendStatus = SendStatus.Idle;
		FormReady = false;
		Form = null;
	}
}


