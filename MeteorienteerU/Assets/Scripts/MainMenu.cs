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
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Loc.Str("mainmenu_observation")))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.Button("Info");
		GUILayout.FlexibleSpace();
		GUILayout.Button("Historia");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
}
