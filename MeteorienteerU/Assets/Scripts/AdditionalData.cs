using UnityEngine;
using System.Collections.Generic;

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

	void Awake()
	{
		directionView = GetComponent<DirectionView>();
		taivaanVahti = GetComponent<Taivaanvahti>();
		sentView = GetComponent<SentView>();
	}
	
	void OnEnable()
	{
		sendResult = Taivaanvahti.SendResult.None;
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
		
		float angle = Vector3.Angle(Vector3.down, SensorData.Acceleration) - 90f;
		
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
		
		fieldValues.Add("observation_date", System.DateTime.Now.ToShortDateString());
		fieldValues.Add("observation_start_hours", System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString());
		
		fieldValues.Add("observation_coordinates", SensorData.Latitude + ", " + SensorData.Longitude);
	}	
	
	void OnGUI()
	{
		GUISkin origSkin = guiskin;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgImage, ScaleMode.StretchToFill);
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Box("Lisää tietoja havainnosta:");
		if (sendResult == Taivaanvahti.SendResult.MissingFields)
		{
			GUILayout.Box("Error: Some fields are missing");
		}
		else if (sendResult == Taivaanvahti.SendResult.OtherError)
		{
			GUILayout.Box("Error: Error sending data to server.");
		}
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Takaisin"))
		{
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUI.skin = origSkin;
		GUIDrawFormOptions();
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Lähetä"))
		{
			SubmitForm();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	private Taivaanvahti.SendResult sendResult;
	
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
			}
			if (taivaanVahti.OngoingSendResult() == Taivaanvahti.SendResult.Success)
			{
				this.enabled = false;
				sentView.enabled = true;
				sendResult = Taivaanvahti.SendResult.None;
			}
			else if (taivaanVahti.OngoingSendResult() != Taivaanvahti.SendResult.None)
			{
				sendResult = taivaanVahti.OngoingSendResult();
			}
		}
	}
	
	private Dictionary<int, bool> showPopup = new Dictionary<int, bool>();
	private Dictionary<int, int> listEntry = new Dictionary<int, int>();
	private Dictionary<string, string> fieldValues = new Dictionary<string, string>();
	
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
							GUILayout.Box(field.label);
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
							GUILayout.Box(field.label);
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
				else if (field.id == "start_minutes")
				{
					if (fieldValues.ContainsKey("observation_start_minutes"))
					{
						submitData.Add(field, System.DateTime.Parse(fieldValues["observation_start_minutes"]).Minute.ToString());
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
			
			if (taivaanVahti.Form.Fields.Find(ff => ff.id == "start_minutes") == null)
			{
				TaivaanvahtiField field = new TaivaanvahtiField(null);
				field.id = "start_minutes";
				if (fieldValues.ContainsKey("observation_start_hours"))
				{
					submitData.Add(field, System.DateTime.Parse(fieldValues["observation_start_hours"]).Minute.ToString());
				}
			}
			
			taivaanVahti.SubmitForm(submitData);
		}	
	}
}
