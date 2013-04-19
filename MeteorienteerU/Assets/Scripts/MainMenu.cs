using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SensorCaptureView))]
public class MainMenu : MonoBehaviour
{
	//public GUIStyle buttonStyle;
	private SensorCaptureView sensorCaptureView;
	private InfoView infoView;
	
	void Awake()
	{
		sensorCaptureView = GetComponent<SensorCaptureView>();
		infoView = GetComponent<InfoView>();
	}
	
	void OnGUI()
	{
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
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
		if(GUILayout.Button(Loc.Str("mainmenu_info"))){
			this.enabled = false;
			infoView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
}
