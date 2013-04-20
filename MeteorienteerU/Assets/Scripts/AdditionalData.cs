using UnityEngine;
using System.Collections;

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
	}
	
	void OnGUI()
	{
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.Box(new GUIContent(bgImage));
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
		GUIDrawFormOptions();
		GUILayout.FlexibleSpace();
		GUILayout.Button("Lähetä");
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void GUIDrawFormOptions()
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		if (taivaanVahti)
		{
			if (taivaanVahti.FormReady)
			{
				foreach (TaivaanvahtiField field in taivaanVahti.Form.Fields)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(field.id);
					GUILayout.TextField("");
					GUILayout.Label(field.values != null ? field.values.Count.ToString() : "0");
					GUILayout.EndHorizontal();
				}
			}
		}
		GUILayout.EndScrollView();
	}
}
