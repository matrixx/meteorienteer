using UnityEngine;
using System.Collections;

public class SensorCaptureView : MonoBehaviour
{
	public CamViewer deviceCamera;
	private MainMenu mainMenu;
	private DirectionView directionView;
	
	void Awake()
	{
		mainMenu = GetComponent<MainMenu>();
		directionView = GetComponent<DirectionView>();
		
	}
	
	void OnEnable()
	{
		if (deviceCamera)
		{
			deviceCamera.gameObject.SetActiveRecursively(true);
			if (!deviceCamera.webCamTex.isPlaying)
			{
				deviceCamera.webCamTex.Play();
			}
		}
	}
	
	void OnGUI()
	{
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label("Suuntaa kamera sinne, missä havaitsit tulipallon, ja ota kuva.");
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Takaisin"))
		{
			this.enabled = false;
			mainMenu.enabled = true;
			if (deviceCamera)
			{
				deviceCamera.gameObject.SetActiveRecursively(false);
			}
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Ota kuva"))
		{
			SensorData.CaptureNow();
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		if (Input.location.status == LocationServiceStatus.Running)
		{
			GUILayout.Label("Latitude: " + Input.location.lastData.latitude);
			GUILayout.Label("Longitude: " + Input.location.lastData.longitude);
		}
		else
		{
			GUILayout.Label("Location services unavailable");
		}
		GUILayout.Label("---");
		GUILayout.Label("Heading: " + Mathf.Round(Input.compass.trueHeading));
		GUILayout.Label("---");
		float angle = Vector3.Angle(Vector3.down, Input.acceleration) - 90f;
		GUILayout.Label("Vertical angle: " + Mathf.Round(angle));
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void OnDisable()
	{
		if (deviceCamera){
			deviceCamera.webCamTex.Pause();
		}
	}
}
