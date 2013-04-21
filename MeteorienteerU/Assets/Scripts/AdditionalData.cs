using UnityEngine;
using System.Collections.Generic;

public class AdditionalData : MonoBehaviour {
	
	public Texture bgImage;
	private DirectionView directionView;
	private Taivaanvahti taivaanVahti;
	
	private Vector2 scrollPosition;

	void Awake()
	{
		directionView = GetComponent<DirectionView>();
		taivaanVahti = GetComponent<Taivaanvahti>();
	}
	
	void OnEnable()
	{
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
		fieldValues.Add("specific_ilmansuunta_katoamishetkellä", Mathf.Round(SensorData.TrueHeading).ToString());
		
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
		fieldValues.Add("specific_korkeus_katoamishetkellä", Mathf.Round(angle).ToString());
		
		Vector3 directionLineVec = DirectionLine.Singleton.wentTo.position - DirectionLine.Singleton.startedFrom.position;
		float travelAngle = Vector3.Angle(Vector3.up,directionLineVec);
		if (directionLineVec.x < 0) travelAngle = 360f - travelAngle;
		fieldValues.Add("specific_lentokulma", Mathf.Round(travelAngle).ToString());
	}
	
	void OnGUI()
	{
		GUISkin origSkin = GUI.skin;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgImage, ScaleMode.StretchToFill);
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label("Lisää tietoja havainnosta:");
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Takaisin"))
		{
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUI.skin = origSkin;
		GUIDrawFormOptions();
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.FlexibleSpace();
		GUILayout.Button("Lähetä");
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	private Dictionary<int, bool> showPopup = new Dictionary<int, bool>();
	private Dictionary<int, int> listEntry = new Dictionary<int, int>();
	private Dictionary<string, string> fieldValues = new Dictionary<string, string>();
	
	void GUIDrawFormOptions()
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		if (taivaanVahti)
		{
			if (taivaanVahti.FormReady)
			{
				int fieldNumber = 0;
				foreach (TaivaanvahtiField field in taivaanVahti.Form.Fields)
				{
					if (!showPopup.ContainsKey(fieldNumber)) showPopup.Add(fieldNumber, false);
					if (!listEntry.ContainsKey(fieldNumber)) listEntry.Add(fieldNumber, 0);
					if (!fieldValues.ContainsKey(field.id)) fieldValues.Add(field.id, "");
					GUILayout.BeginHorizontal();
//					if (field.type == TaivaanvahtiField.FieldType.TYPE_CHECKBOX)
//					{
//						fieldValues[field.id] = GUILayout.Toggle(fieldValues[field.id] != "0", field.label) ? "1" : "0";
//					}
//					if (field.type == TaivaanvahtiField.FieldType.TYPE_SELECTION && field.values != null && field.values.Count > 1)
//					{
//						GUILayout.Label(field.label);
//						GUIContent[] listContent = new GUIContent[field.values.Count];
//						int i = 0;
//						foreach (string str in field.values.Values)
//						{
//							listContent[i] = new GUIContent(str);
//							if (str == fieldValues[field.id])
//							{
//								listEntry[fieldNumber] = i;
//							}
//							++i;
//						}
//						Rect rc = GUILayoutUtility.GetRect(listContent[listEntry[fieldNumber]], GUI.skin.box);
//						bool show = showPopup[fieldNumber];
//						int entry = listEntry[fieldNumber];
//						Popup.List(rc, ref show, ref entry, listContent[listEntry[fieldNumber]], listContent, GUI.skin.button);
//						showPopup[fieldNumber] = show;
//						listEntry[fieldNumber] = entry;
//						fieldValues[field.id] = listContent[entry].text;
//					}
//					else
//					{
						GUILayout.Label(field.label);
						fieldValues[field.id] = GUILayout.TextField(fieldValues[field.id]);
//					}
					GUILayout.EndHorizontal();
					++fieldNumber;
				}
			}
		}
		GUILayout.EndScrollView();
	}
}
