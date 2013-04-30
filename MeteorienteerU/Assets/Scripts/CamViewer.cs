using UnityEngine;
using System.Collections;

public class CamViewer : MonoBehaviour
{
	public float distanceFromCamera = 10f;
	
	public WebCamTexture webCamTex;
	
	public static CamViewer Current {get; private set;}
	
	void Awake()
	{
		Current = this;
		Vector3 newEulers = transform.eulerAngles;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			newEulers.y = 0;
		}
		else
		{
			newEulers.y = 180;
		}
		transform.eulerAngles = newEulers;
	}
	
	void OnEnable()
	{
		if (!webCamTex) webCamTex = new WebCamTexture();
		webCamTex.Play();
		renderer.material.mainTexture = webCamTex;
	}
	
	void Update()
	{
		Ray topLeftRay = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0f));
		Ray bottomRightRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0f));
		Debug.DrawRay(topLeftRay.origin, topLeftRay.direction, Color.green);
		Debug.DrawRay(bottomRightRay.origin, bottomRightRay.direction, Color.red);
		Vector3 topLeftPoint = topLeftRay.GetPoint(distanceFromCamera);
		Vector3 bottomRightPoint = bottomRightRay.GetPoint(distanceFromCamera);
		transform.position = new Vector3(0f, 0f, topLeftPoint.z);
		transform.localScale = new Vector3((bottomRightPoint.x - topLeftPoint.x) / 10f, 
			1f, (topLeftPoint.y - bottomRightPoint.y) / 10f);
	}
	
	void OnDisable()
	{
		webCamTex.Stop();
	}
	
	void OnDestro()
	{
		Current = null;
	}
}
