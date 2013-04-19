using UnityEngine;
using System.Collections;

public class AdditionalData : MonoBehaviour {
	
	private DirectionView directionView;

	void Awake(){
		directionView = GetComponent<DirectionView>();
	}
	
	void OnGUI(){
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label(Loc.Str("data_guide"));
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Loc.Str("data_back")))
		{
			this.enabled = false;
			directionView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.Button(Loc.Str("data_next"));
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
