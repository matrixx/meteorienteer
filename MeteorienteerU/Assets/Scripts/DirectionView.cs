using UnityEngine;
using System.Collections;

public class DirectionView : MonoBehaviour
{	
	public GUISkin transparentGuiSkin;
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
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = transparentGuiSkin;
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.Box("Aseta nuoli kuvaamaan tulipallon lentorataa.");
		if (SensorData.LocationAvailable)
		{
			GUILayout.Box("");
			GUILayout.Box("");
		}
		else
		{
			GUILayout.Box("");
		}
		GUILayout.Box("");
		GUILayout.Box("");
		GUILayout.Box("");
		GUILayout.Box("");
		GUILayout.FlexibleSpace();
		
		GUI.skin = GUIOptions.Singleton.appStyle;
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
		GUI.skin = transparentGuiSkin;
		GUILayout.Box("");
		if (SensorData.LocationAvailable)
		{
			GUILayout.Box("Leveysaste: " + SensorData.Latitude);
			GUILayout.Box("Pituusaste: " + SensorData.Longitude);
		}
		else
		{
			GUILayout.Box("Paikannusdataa ei saatavilla");
		}
		GUILayout.Box("---");
		GUILayout.Box("Ilmansuunta: " + Mathf.Round(SensorData.TrueHeading));
		GUILayout.Box("---");
		float angle = -Vector3.Angle(Vector3.forward, SensorData.Acceleration) + 90f;
		GUILayout.Box("Korkeuskulma: " + Mathf.Round(angle));
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
