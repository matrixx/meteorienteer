using UnityEngine;
using System.Collections;

public class SentView : MonoBehaviour
{
	public Texture2D bgImage;
	private MainMenu mainMenu;
	private SensorCaptureView sensorCaptureView;
	private Taivaanvahti taivaanVahti;
	
	void Awake()
	{
		mainMenu = GetComponent<MainMenu>();
		sensorCaptureView = GetComponent<SensorCaptureView>();
		taivaanVahti = GetComponent<Taivaanvahti>();
	}
	
	void OnGUI()
	{
		GUI.matrix = GUIOptions.Singleton.GUIMatrix;
		GUI.skin = GUIOptions.Singleton.appStyle;
		GUI.DrawTexture(new Rect(0, 0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y), bgImage, ScaleMode.StretchToFill);
		GUILayout.BeginArea(new Rect(0,0, GUIOptions.Singleton.guiResolution.x, GUIOptions.Singleton.guiResolution.y));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Loc.Str("sentview_new_observation")))
		{
			this.enabled = false;
			sensorCaptureView.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		if (taivaanVahti.CurrentSendStatus() == Taivaanvahti.SendStatus.ImageSendFailed)
		{
			GUILayout.Label(Loc.Str("sentview_imagesend_failed"));
		}
		else
		{
			GUILayout.Label(Loc.Str("sentview_observation_sent"));
		}
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Loc.Str("sentview_mainmenu")))
		{
			this.enabled = false;
			mainMenu.enabled = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
