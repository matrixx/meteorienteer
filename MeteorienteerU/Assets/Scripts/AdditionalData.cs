using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AdditionalData : MonoBehaviour
{
	public Texture bgImage;
	public GUISkin guiskin;
	public GUIStyle dropdownBackgroudStyle;
	
	public float inputWidth = 320f;
	public float menuWidth = 732f;
	
	public float spaceReservedForDropdown = 250f;
	
	public List<string> usedFields;
	
	private DirectionView directionView;
	private SentView sentView;
	private Taivaanvahti taivaanVahti;
	
	private Vector2 scrollPosition;
	private bool formWasReady = false;
	
	private int currentField = -1;
	private int firstField = -1;
	private int lastField = -1;
	
//	private Dictionary<string, string> fieldIdsToTranslations = new Dictionary<string, string>();
	private Dictionary<string, string> valuesToTranslations = new Dictionary<string, string>();

	void Awake()
	{
		directionView = GetComponent<DirectionView>();
		taivaanVahti = GetComponent<Taivaanvahti>();
		sentView = GetComponent<SentView>();
		
		TextAsset mappingFile = (TextAsset)Resources.Load("valuesToTranslations", typeof(TextAsset));
		Match linem = Regex.Match(mappingFile.text, ".+;\\s*(\\n|\\r\\n)");
		while (linem.Success)
		{
			string line = linem.Value.Trim();
			line = line.Remove(line.Length - 1);
			string[] separator = {"=>"};
			string[] keyAndValue = line.Split(separator, System.StringSplitOptions.None);
			valuesToTranslations.Add(keyAndValue[0].Replace("==>", "=>"), keyAndValue[1].Replace("==>", "=>"));
			linem = linem.NextMatch();
		}
		
//		mappingFile = (TextAsset)Resources.Load("valuesToTranslations.txt", typeof(TextAsset));
//		linem = Regex.Match(mappingFile.text, ".+;\\s*(\\n|\\r\\n)");
//		while (linem.Success)
//		{
//			string line = linem.Value.Trim();
//			line = line.Remove(line.Length - 1);
//			string[] separator = {"=>"};
//			string[] keyAndValue = line.Split(separator, System.StringSplitOptions.None);
//			valuesToTranslations.Add(keyAndValue[0].Replace("==>", "=>"), keyAndValue[1].Replace("==>", "=>"));
//			linem = linem.NextMatch();
//		}
		
//		Match keym = Regex.Match(source.text, ".+[^\\],?");
//		while (keym.Success)
//		{
//			string key = keym.Value.Trim();
//			key = key.Remove(key.Length - 1);
//			Match next = keym.NextMatch();
//			string trstr = "";
//			if (next.Success)
//			{
//				trstr = source.text.Substring(keym.Index + keym.Length, next.Index - (keym.Index + keym.Length)).Trim().Replace("\\,", ",");
//			}
//			else
//			{
//				trstr = source.text.Substring(keym.Index + keym.Length).Trim().Replace("\\,", ",");
//			}
//			translation.Add(key, trstr);
//			keym = next;
//		}
	}
	
	void OnEnable()
	{
		currentField = -1;
		firstField = -1;
		lastField = -1;
	//	sendResult = Taivaanvahti.SendResult.None;
		Debug.Log("AdditionalData::OnEnable");
		Debug.Log(fieldValues.Count);
		formWasReady = false;
		if (taivaanVahti) taivaanVahti.GetForm();
		fieldValues = new Dictionary<string, string>();
		
		float angleDelta = 360f/8f;
		
		string heading = "Pohjoinen";
		if (SensorData.TrueHeading >= 360 - angleDelta * .5f && SensorData.TrueHeading < angleDelta * .5f)
		{
			heading = "Pohjoinen";
		}
		else if (SensorData.TrueHeading >= angleDelta * .5f && SensorData.TrueHeading < angleDelta * 1.5f)
		{
			heading = "Koillinen";
		}
		else if (SensorData.TrueHeading >= angleDelta * 1.5f && SensorData.TrueHeading < angleDelta * 2.5f)
		{
			heading = "Itä";
		}
		else if (SensorData.TrueHeading >= angleDelta * 2.5f && SensorData.TrueHeading < angleDelta * 3.5f)
		{
			heading = "Kaakko";
		}
		else if (SensorData.TrueHeading >= angleDelta * 3.5f && SensorData.TrueHeading < angleDelta * 4.5f)
		{
			heading = "Etelä";
		}
		else if (SensorData.TrueHeading >= angleDelta * 4.5f && SensorData.TrueHeading < angleDelta * 5.5f)
		{
			heading = "Lounas";
		}
		else if (SensorData.TrueHeading >= angleDelta * 5.5f && SensorData.TrueHeading < angleDelta * 6.5f)
		{
			heading = "Länsi";
		}
		else if (SensorData.TrueHeading >= angleDelta * 6.5f && SensorData.TrueHeading < angleDelta * 7.5f)
		{
			heading = "Luode";
		}
		fieldValues.Add("specific_ilmansuunta_katoamishetkellä", heading);
		
		float angle = -Vector3.Angle(Vector3.forward, SensorData.Acceleration) + 90f;
		
		angleDelta = 90f / 7f;
		
		string korkeus =  "";
		
		if (angle < angleDelta)
		{
			korkeus = "Erittäin matalalla horisontin luona";
		}
		else if (angle < angleDelta * 2f)
		{
			korkeus = "Melko matalalla lähellä horisonttia";
		}
		else if (angle < angleDelta * 3f)
		{
			korkeus = "Noin 1/3:n päässä horisontista kohti taivaanlakea";
		}
		else if (angle < angleDelta * 4f)
		{
			korkeus = "Noin puolivälissä taivasta";
		}
		else if (angle < angleDelta * 5f)
		{
			korkeus = "Noin 1/3:n päässä taivaanlaesta kohti horisonttia";
		}
		else if (angle < angleDelta * 6f)
		{
			korkeus = "Lähes suoraan pääni yläpuolella";
		}
		else if (angle < angleDelta * 7f)
		{
			korkeus = "Suoraan pääni yläpuolella";
		}
		fieldValues.Add("specific_korkeus_katoamishetkellä", korkeus);
		
		Vector3 directionLineVec = DirectionLine.Singleton.wentTo.position - DirectionLine.Singleton.startedFrom.position;
		float travelAngle = Vector3.Angle(Vector3.up,directionLineVec);
		if (directionLineVec.x < 0) travelAngle = 360f - travelAngle;
		fieldValues.Add("specific_lentokulma", Mathf.Round(travelAngle).ToString());
		
		System.DateTime nowTime = System.DateTime.Now;
		
		fieldValues.Add("observation_date", nowTime.ToShortDateString());
		fieldValues.Add("observation_start_hours", nowTime.Hour.ToString() + ":" +
			(nowTime.Minute < 10 ? "0" : "") + nowTime.Minute.ToString());
		
		fieldValues.Add("observation_coordinates", SensorData.Latitude + ", " + SensorData.Longitude);
	}	
	
	void OnGUI()
	{
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUISkin origSkin = guiskin;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y), bgImage, ScaleMode.StretchToFill);
		GUILayout.BeginArea(new Rect(0,0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str("additionaldata_title"));
		if (taivaanVahti.CurrentSendStatus() == Taivaanvahti.SendStatus.MissingFields)
		{
			GUILayout.Label(Loc.Str("additionaldata_missing_fields"));
		}
		else if (taivaanVahti.CurrentSendStatus() == Taivaanvahti.SendStatus.OtherError)
		{
			GUILayout.Label(Loc.Str("additionaldata_other_error"));
		}
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUI.skin = origSkin;
		GUIDrawCurrentField();
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (currentField == firstField)
		{
			if (GUILayout.Button(Loc.Str("additionaldata_back")))
			{
				if (taivaanVahti.CurrentSendStatus() != Taivaanvahti.SendStatus.Sending)
				{
					this.enabled = false;
					directionView.enabled = true;
				}
			}
		}
		else
		{
			if (GUILayout.Button(Loc.Str("additionaldata_previous")))
			{
				PrevField();
			}
		}
		
		GUILayout.FlexibleSpace();
		
		if (taivaanVahti && taivaanVahti.FormReady && formWasReady)
		{
			if (currentField == lastField)
			{
				if (GUILayout.Button(Loc.Str("additionaldata_send")))
				{
					if (taivaanVahti.CurrentSendStatus() != Taivaanvahti.SendStatus.Sending)
					{
						SubmitForm();
					}
				}
			}
			else
			{
				if (GUILayout.Button(Loc.Str("additionaldata_next")))
				{
					NextField();
				}
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
//	private Taivaanvahti.SendStatus sendResult;
	
	private Dictionary<int, bool> showPopup = new Dictionary<int, bool>();
	private Dictionary<int, int> listEntry = new Dictionary<int, int>();
	private Dictionary<string, string> fieldValues = new Dictionary<string, string>();
	
	void Update()
	{
		if (taivaanVahti)
		{
			if (taivaanVahti.FormReady && !formWasReady)
			{
				int fieldNumber = 0;
				foreach (TaivaanvahtiField field in taivaanVahti.Form.Fields)
				{
					if (usedFields.Contains(field.id))
					{
						if (!showPopup.ContainsKey(fieldNumber)) showPopup.Add(fieldNumber, false);
						if (!listEntry.ContainsKey(fieldNumber)) listEntry.Add(fieldNumber, 0);
						if (!fieldValues.ContainsKey(field.id))
						{
							Debug.Log("Field was not set: " + field.id);
							fieldValues.Add(field.id, "");
						}
						else
						{
							Debug.Log("Field was already set: " + field.id);
						}
					}
					++fieldNumber;
				}
				formWasReady = true;
				NextField();
				firstField = currentField;
				for (int i = 0; i < taivaanVahti.Form.Fields.Count; ++i)
				{
					if (usedFields.Contains(taivaanVahti.Form.Fields[i].id))
					{
						lastField = i;
					}
				}
			}
			if (taivaanVahti.CurrentSendStatus() == Taivaanvahti.SendStatus.Success ||
				taivaanVahti.CurrentSendStatus() == Taivaanvahti.SendStatus.ImageSendFailed)
			{
				this.enabled = false;
				sentView.enabled = true;
			}
//			else if (taivaanVahti.SendStatus() != Taivaanvahti.SendResult.None)
//			{
//				sendResult = taivaanVahti.SendStatus();
//			}
		}
	}
		
	void GUIDrawFormOptions()
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(menuWidth));
		if (taivaanVahti)
		{
			if (taivaanVahti.FormReady && formWasReady)
			{
				Dictionary<int, Rect> popupRects = new Dictionary<int, Rect>();
				int fieldNumber = 0;
				foreach (TaivaanvahtiField field in taivaanVahti.Form.Fields)
				{
					if (usedFields.Contains(field.id))
					{
						GUILayout.BeginHorizontal();
						if (field.type == TaivaanvahtiField.FieldType.TYPE_CHECKBOX)
						{
							fieldValues[field.id] = GUILayout.Toggle(fieldValues[field.id] != "0", field.label) ? "1" : "0";
						}
						if (field.type == TaivaanvahtiField.FieldType.TYPE_SELECTION && field.values != null && field.values.Count > 1)
						{
							GUILayout.Label(field.label);
							GUILayout.FlexibleSpace();
							GUIContent[] listContent = new GUIContent[field.values.Count];
							int i = 0;
							foreach (string str in field.values.Values)
							{
								listContent[i] = new GUIContent(str);
								if (str == fieldValues[field.id])
								{
									listEntry[fieldNumber] = i;
								}
								++i;
							}
							Rect rc = GUILayoutUtility.GetRect(inputWidth, GUI.skin.box.CalcSize(listContent[listEntry[fieldNumber]]).y);
							popupRects.Add(fieldNumber, rc);
						}
						else
						{
							GUILayout.Label(field.label);
							GUILayout.FlexibleSpace();
							fieldValues[field.id] = GUILayout.TextField(fieldValues[field.id], GUILayout.Width(inputWidth));
						}
						GUILayout.EndHorizontal();
					}
					++fieldNumber;
				}
				
				for (fieldNumber = taivaanVahti.Form.Fields.Count - 1; fieldNumber >= 0; --fieldNumber)
				{
					TaivaanvahtiField field =  taivaanVahti.Form.Fields[fieldNumber];
					if (usedFields.Contains(field.id) && field.type == TaivaanvahtiField.FieldType.TYPE_SELECTION && field.values != null && field.values.Count > 1)
					{
						GUIContent[] listContent = new GUIContent[field.values.Count];
						int i = 0;
						foreach (string str in field.values.Values)
						{
							listContent[i] = new GUIContent(str);
							if (str == fieldValues[field.id])
							{
								listEntry[fieldNumber] = i;
							}
							++i;
						}
						bool show = showPopup[fieldNumber];
						int entry = listEntry[fieldNumber];
						Popup.List(popupRects[fieldNumber], ref show, ref entry, listContent[listEntry[fieldNumber]],
							listContent, GUI.skin.button, dropdownBackgroudStyle, GUI.skin.button);
						GUI.depth = 10;
						showPopup[fieldNumber] = show;
						listEntry[fieldNumber] = entry;
						fieldValues[field.id] = listContent[entry].text;
					}
				}
				GUILayout.Space(spaceReservedForDropdown);
			}
		}
		GUILayout.EndScrollView();
	}
	
	void NextField()
	{
		int prevField = currentField;
		do
		{
			++currentField;
		}
		while (currentField < taivaanVahti.Form.Fields.Count && !usedFields.Contains(taivaanVahti.Form.Fields[currentField].id));
		if (currentField >= taivaanVahti.Form.Fields.Count)
		{
			currentField = prevField;
		}
	}
	
	void PrevField()
	{
		int lastField = currentField;
		do
		{
			--currentField;
		}
		while (currentField >= 0 && !usedFields.Contains(taivaanVahti.Form.Fields[currentField].id));
		if (currentField < 0)
		{
			currentField = lastField;
		}
	}
	
	void GUIDrawCurrentField()
	{
		if (taivaanVahti && taivaanVahti.FormReady && formWasReady)
		{
			if (currentField >= 0 && currentField < taivaanVahti.Form.Fields.Count)
			{
				TaivaanvahtiField field =  taivaanVahti.Form.Fields[currentField];
				
				GUILayout.BeginVertical();
				if (field.type == TaivaanvahtiField.FieldType.TYPE_CHECKBOX)
				{
					fieldValues[field.id] = GUILayout.Toggle(fieldValues[field.id] != "0", Loc.Str(field.id)) ? "1" : "0";
				}
				if (field.type == TaivaanvahtiField.FieldType.TYPE_SELECTION && field.values != null && field.values.Count > 1)
				{
					GUILayout.Label(Loc.Str(field.id));
					GUIContent[] listContent = new GUIContent[field.values.Count];
					string[] values = new string[field.values.Count];
					int i = 0;
					foreach (string str in field.values.Values)
					{
						string loc_id;
						if (!valuesToTranslations.TryGetValue(str, out loc_id))
						{
							loc_id = str;
						}
						listContent[i] = new GUIContent(Loc.Str(loc_id));
						if (str == fieldValues[field.id])
						{
							listEntry[currentField] = i;
						}
						values[i] = str;
						++i;
					}
					int entry = listEntry[currentField];
					entry = GUILayout.SelectionGrid(entry, listContent, 2);
					listEntry[currentField] = entry;
					fieldValues[field.id] = values[entry];
				}
				else
				{
					GUILayout.Label(Loc.Str(field.id));
					fieldValues[field.id] = GUILayout.TextField(fieldValues[field.id], GUILayout.Width(inputWidth));
				}
				GUILayout.EndVertical();
			}
		}
		else
		{
			GUILayout.Label(Loc.Str("additionaldata_connectingtoserver"));
		}
	}
	
	void SubmitForm()
	{
		if (taivaanVahti && taivaanVahti.FormReady && formWasReady)
		{
			Dictionary<TaivaanvahtiField, string> submitData = new Dictionary<TaivaanvahtiField, string>();
			foreach (TaivaanvahtiField field in taivaanVahti.Form.Fields)
			{
				if (usedFields.Contains(field.id) && fieldValues.ContainsKey(field.id))
				{
					submitData.Add(field, fieldValues[field.id]);
				}
				else if (field.id == "start_day")
				{
					if (fieldValues.ContainsKey("observation_date"))
					{
						submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Day.ToString());
					}
				}
				else if (field.id == "start_month")
				{
					if (fieldValues.ContainsKey("observation_date"))
					{
						submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Month.ToString());
					}
				}
				else if (field.id == "start_year")
				{
					if (fieldValues.ContainsKey("observation_date"))
					{
						submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Year.ToString());
					}
				}
				
				if (field.id == "start_hours")
				{
					if (fieldValues.ContainsKey("observation_start_hours"))
					{
						string hours = System.DateTime.Parse(fieldValues["observation_start_hours"]).Hour.ToString();
						Debug.Log(hours);
						submitData.Add(field, hours);
					}
				}
				else if (field.id == "start_minutes")
				{
					if (fieldValues.ContainsKey("observation_start_hours"))
					{
						submitData.Add(field, System.DateTime.Parse(fieldValues["observation_start_hours"]).Minute.ToString());
					}
				}
			}
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_day") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_day";
				if (fieldValues.ContainsKey("observation_date"))
				{
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Day.ToString());
				}
			}
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_month") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_month";
				if (fieldValues.ContainsKey("observation_date"))
				{
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Month.ToString());
				}
			}
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_year") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_year";
				if (fieldValues.ContainsKey("observation_date"))
				{
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_date"]).Year.ToString());
				}
			}
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_hours") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_hours";
				if (fieldValues.ContainsKey("observation_start_hours"))
				{
					Debug.Log("Hour2: " + System.DateTime.Parse(fieldValues["observation_start_hours"]).Hour.ToString());
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_start_hours"]).Hour.ToString());
				}
			}
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_minutes") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_minutes";
				if (fieldValues.ContainsKey("observation_start_hours"))
				{
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_start_hours"]).Minute.ToString());
				}
			}
			
			taivaanVahti.SubmitForm(submitData, SensorData.image);
		}	
	}
	
	void OnDisable()
	{
		taivaanVahti.Reset();
	}
}
