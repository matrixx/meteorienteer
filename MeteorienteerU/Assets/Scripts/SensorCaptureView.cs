using UnityEngine;
using System.Collections;

public class SensorCaptureView : MonoBehaviour
{
	public CamViewer deviceCamera;
	public GUISkin transparentGuiSkin;
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
		Input.location.Start();
		Input.compass.enabled = true;
	}
	
	void OnGUI()
	{
		// draw the screen
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		
		// above the menu
		GUILayout.Label(Loc.Str ("sensorcaptureview_info"));
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.FlexibleSpace();
		
		// menu
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Loc.Str ("sensorcaptureview_back")))
		{
			this.enabled = false;
			mainMenu.enabled = true;
			if (deviceCamera)
			{
				deviceCamera.gameObject.SetActiveRecursively(false);
			}
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(Loc.Str ("sensorcaptureview_next")))
		{
			SensorData.CaptureNow();
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		// below the menu
		GUILayout.FlexibleSpace();
//		GUI.skin = transparentGuiSkin;
		if (Input.location.status == LocationServiceStatus.Running)
		{
			GUILayout.Label(Loc.Str("sensorcaptureview_latitude") + Input.location.lastData.latitude + ", " +
				Loc.Str("sensorcaptureview_longitude") + Input.location.lastData.longitude);
		}
		else
		{
			GUILayout.Label(Loc.Str("sensorcaptureview_locationunavailable"));
		}
		GUILayout.Label("---");
		GUILayout.Label(Loc.Str("sensorcaptureview_heading") + Mathf.Round(Input.compass.trueHeading));
		GUILayout.Label("---");
		float angle = -Vector3.Angle(Vector3.forward, Input.acceleration) + 90f;
		GUILayout.Label(Loc.Str("sensorcaptureview_verticalangle") + Mathf.Round(angle));
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void OnDisable()
	{
		if (deviceCamera)
		{
			deviceCamera.webCamTex.Stop();
		}
		Input.location.Stop();
		Input.compass.enabled = false;
	}
}
