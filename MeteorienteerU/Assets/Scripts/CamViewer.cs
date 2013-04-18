using UnityEngine;
using System.Collections;

public class CamViewer : MonoBehaviour
{
	WebCamTexture webCamTex;
	
	void Start()
	{
		webCamTex = new WebCamTexture();
		renderer.material.mainTexture = webCamTex;
	}
	
	void Update()
	{
		
	}
}
