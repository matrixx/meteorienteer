using UnityEngine;
using System.Collections;

public class CanHasGps : MonoBehaviour
{	
	void Start()
	{
		Input.location.Start();
		Input.compass.enabled = true;
	}
	
	void OnGUI()
	{
		if (Input.location.status == LocationServiceStatus.Running)
		{
			/*GUILayout.Label("Horizontal accuracy: " + Input.location.lastData.horizontalAccuracy);
			GUILayout.Label("Vertical accuracy: " + Input.location.lastData.verticalAccuracy);
			GUILayout.Label("Latitude: " + Input.location.lastData.latitude);
			GUILayout.Label("Longitude: " + Input.location.lastData.longitude);
			GUILayout.Label("Altitude: " + Input.location.lastData.altitude);*/
		}
		else
		{
			/*GUILayout.Label("Location services unavailable");*/
		}
		/*GUILayout.Label("---");
		GUILayout.Label("Magnetic heading: " + Input.compass.magneticHeading);
		GUILayout.Label("True heading: " + Input.compass.trueHeading);
		GUILayout.Label("Raw vector: " + Input.compass.rawVector);
		GUILayout.Label("---");
		GUILayout.Label("Accelerometer vector: " + Input.acceleration);*/
	}
}
