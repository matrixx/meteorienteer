using UnityEngine;
using System.Collections;

public class DirectionView : MonoBehaviour {
	
	private SensorCaptureView sensorCaptureView;
	private AdditionalData additionalData;

	void Awake(){
		sensorCaptureView = GetComponent<SensorCaptureView>();
		additionalData = GetComponent<AdditionalData>();
	}
	
	void OnGUI(){
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str("direction_guide"));
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Loc.Str("direction_back")))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(Loc.Str("direction_next"))){
			this.enabled = false;
			additionalData.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
