using UnityEngine;
using System.Collections;

public class DirectionView : MonoBehaviour
{	
	private SensorCaptureView sensorCaptureView;
	private AdditionalData additionalData;

	void Awake()
	{
		sensorCaptureView = GetComponent<SensorCaptureView>();
		additionalData = GetComponent<AdditionalData>();
	}
	
	void OnEnable()
	{
		DirectionLine.Singleton.enabled = true;
	}
	
	void OnGUI()
	{
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label("Aseta nuoli kuvaamaan tulipallon lentorataa.");
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Takaisin"))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Seuraava"))
		{
			this.enabled = false;
			additionalData.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		if (SensorData.LocationAvailable)
		{
			GUILayout.Label("Latitude: " + SensorData.Latitude);
			GUILayout.Label("Longitude: " + SensorData.Longitude);
		}
		else
		{
			GUILayout.Label("Location services unavailable");
		}
		GUILayout.Label("---");
		GUILayout.Label("Heading: " + Mathf.Round(SensorData.TrueHeading));
		GUILayout.Label("---");
		float angle = -Vector3.Angle(Vector3.forward, SensorData.Acceleration) + 90f;
		GUILayout.Label("Vertical angle: " + Mathf.Round(angle));
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void OnDisable()
	{
		DirectionLine.Singleton.enabled = false;
		/*if(SensorCaptureView.deviceCamera){
			SensorCaptureView.deviceCamera.gameObject.SetActiveRecursively(false);
		}*/
	}
}
