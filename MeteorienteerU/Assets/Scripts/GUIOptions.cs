using UnityEngine;
using System.Collections;

public class GUIOptions : MonoBehaviour
{
	public GUISkin appStyle;
	
	private Matrix4x4 guiMatrix = new Matrix4x4();
	public Matrix4x4 GUIMatrix
	{
		get
		{
			return guiMatrix;
		}
	}
	
	public float desiredWidth = 1280f;
	public float desiredHeight = 720f;
	public Vector2 guiResolution = new Vector2(1280f, 720f);

	public static GUIOptions Singleton
	{
		get;
		private set;
	}
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Singleton = this;
	}
	
	private float screenWidth;
	private float screenHeight;
	private float desiredRatio;
	private float realRatio;
	private Vector3 tmpVec;
	
	void FixedUpdate()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		desiredRatio = desiredWidth / desiredHeight;
		
		realRatio = screenWidth / screenHeight;
		
		if (screenWidth / screenHeight > desiredRatio)
		{
			guiResolution.y = desiredHeight;
			guiResolution.x = desiredHeight * realRatio;
		}
		else
		{
			guiResolution.x = desiredWidth;
			guiResolution.y = desiredWidth / realRatio;
		}

		tmpVec.Set( Screen.width / guiResolution.x, Screen.height / guiResolution.y, 1.0f );
		guiMatrix.SetTRS(Vector3.zero, Quaternion.identity, tmpVec);
	}
	
}