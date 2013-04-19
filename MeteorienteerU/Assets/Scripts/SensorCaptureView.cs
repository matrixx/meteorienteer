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
		if (deviceCamera) deviceCamera.gameObject.SetActiveRecursively(true);
	}
	
	void OnGUI()
	{
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str("sensorcapture_guide"));
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Loc.Str("sensorcapture_back")))
		{
			this.enabled = false;
			mainMenu.enabled = true;
		}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(Loc.Str("sensorcapture_capture")))
		{
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	void OnDisable()
	{
		if (deviceCamera) deviceCamera.gameObject.SetActiveRecursively(false);
	}
}
