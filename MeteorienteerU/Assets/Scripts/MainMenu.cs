using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SensorCaptureView))]
public class MainMenu : MonoBehaviour
{
	//public GUIStyle buttonStyle;
	private SensorCaptureView sensorCaptureView;
	
	void Awake()
	{
		sensorCaptureView = GetComponent<SensorCaptureView>();
	}
	
	void OnGUI()
	{
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
		GUILayout.Button(Loc.Str("mainmenu_info"));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
}
