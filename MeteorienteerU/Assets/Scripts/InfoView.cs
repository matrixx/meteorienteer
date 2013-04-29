using UnityEngine;
using System.Collections;

public class InfoView : MonoBehaviour {
	
	public Texture bgImage;
	private MainMenu mainMenu;
	
	void Awake()
	{
		mainMenu = GetComponent<MainMenu>();
	}
	
	void OnGUI()
	{
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y), bgImage, ScaleMode.StretchToFill);
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str("info_text"));
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Loc.Str("infoview_back")))
		{
			this.enabled = false;
			mainMenu.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.Button(Loc.Str("infoview_"));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
}
