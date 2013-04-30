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
			GUILayout.Label("Latitude: " + Input.location.lastData.latitude + ", Longitude: " + Input.location.lastData.longitude);
		}
		else
		{
			GUILayout.Label("Location services unavailable"); // 2 | 1
		}
		GUILayout.Label("---"); // 3 | 2
		GUILayout.Label("Heading: " + Mathf.Round(Input.compass.trueHeading)); // 4 | 3
		GUILayout.Label("---"); // 5 | 4
		float angle = -Vector3.Angle(Vector3.forward, Input.acceleration) + 90f;
		GUILayout.Label("Vertical angle: " + Mathf.Round(angle)); // 6 | 5
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
