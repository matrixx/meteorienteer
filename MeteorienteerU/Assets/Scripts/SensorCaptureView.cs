using UnityEngine;
using System.Collections;

public class SensorCaptureView : MonoBehaviour
{
	
	private MainMenu mainMenu;
	private DirectionView directionView;
	
	void Awake(){
		mainMenu = GetComponent<MainMenu>();
		directionView = GetComponent<DirectionView>();
	}
	
	void OnGUI()
	{
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
		if(GUILayout.Button(Loc.Str("sensorcapture_capture"))){
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
