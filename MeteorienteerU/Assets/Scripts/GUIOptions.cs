using UnityEngine;
using System.Collections;

public class GUIOptions : MonoBehaviour {
	
	public GUISkin appStyle;

	public static GUIOptions Singleton{
		get;
		private set;
	}
	
	void Awake(){
		DontDestroyOnLoad(gameObject);
		Singleton = this;
	}
	
}