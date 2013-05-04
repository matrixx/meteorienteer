using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SensorCaptureView))]
public class MainMenu : MonoBehaviour
{
	public Texture bgImage;
	private SensorCaptureView sensorCaptureView;
	private InfoView infoView;
	
	void Awake()
	{
		sensorCaptureView = GetComponent<SensorCaptureView>();
		infoView = GetComponent<InfoView>();
	}
	
	void OnGUI()
	{
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y), bgImage, ScaleMode.StretchToFill);
		
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Meteorienteer");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Loc.Str("mainmenu_observation")))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUI.enabled = false;
		GUILayout.Button(Loc.Str ("mainmenu_info"));
		GUILayout.FlexibleSpace();
		GUILayout.Button(Loc.Str ("mainmenu_history"));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
}
