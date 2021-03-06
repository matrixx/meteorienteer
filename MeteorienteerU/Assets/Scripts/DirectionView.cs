﻿using UnityEngine;
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
//		if (sensorCaptureView.deviceCamera)
//		{
//			sensorCaptureView.deviceCamera.gameObject.SetActiveRecursively(true);
//		}
	}
	
	void OnGUI()
	{
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str ("directionview_info"));
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Loc.Str ("directionview_back")))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(Loc.Str ("directionview_next")))
		{
			this.enabled = false;
			additionalData.enabled = true;
//			if (sensorCaptureView.deviceCamera)
//			{
//				sensorCaptureView.deviceCamera.gameObject.SetActiveRecursively(false);
//			}
		}
		GUILayout.EndHorizontal();
		
		
		GUILayout.FlexibleSpace();
		if (SensorData.LocationAvailable)
		{
			GUILayout.Label(Loc.Str("sensorcaptureview_latitude") + SensorData.Latitude + ", " +
				Loc.Str("sensorcaptureview_longitude") + SensorData.Longitude);
		}
		else
		{
			GUILayout.Label(Loc.Str("sensorcaptureview_locationunavailable"));
		}
		GUILayout.Label("---");
		GUILayout.Label(Loc.Str("sensorcaptureview_heading") + Mathf.Round(SensorData.TrueHeading));
		GUILayout.Label("---");
		float angle = -Vector3.Angle(Vector3.forward, SensorData.Acceleration) + 90f;
		GUILayout.Label(Loc.Str("sensorcaptureview_verticalangle") + Mathf.Round(angle));
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
