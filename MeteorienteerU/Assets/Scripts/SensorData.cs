using UnityEngine;
using System.Collections;

public class SensorData
{
	public static bool LocationAvailable {get; private set;}
	public static float HorizontalAccuracy {get; private set;}
	public static float VerticalAccuracy {get; private set;}
	public static float Latitude {get; private set;}
	public static float Longitude {get; private set;}
	public static float Altitude {get; private set;}
	
	public static float MagneticHeading {get; private set;}
	public static float TrueHeading {get; private set;}
	public static Vector3 RawMagneticVector {get; private set;}
	
	public static Vector3 Acceleration {get; private set;}
	
	public static Texture2D image;
	
	public static void CaptureNow()
	{
		if (Input.location.status == LocationServiceStatus.Running)
		{
			LocationAvailable = true;
			HorizontalAccuracy = Input.location.lastData.horizontalAccuracy;
			VerticalAccuracy = Input.location.lastData.verticalAccuracy;
			Latitude = Input.location.lastData.latitude;
			Longitude = Input.location.lastData.longitude;
			Altitude = Input.location.lastData.altitude;
		}
		else
		{
			LocationAvailable = false;
		}
		
		MagneticHeading = Input.compass.magneticHeading;
		TrueHeading = Input.compass.trueHeading;
		RawMagneticVector = Input.compass.rawVector;
		
		Acceleration = Input.acceleration;
		
		image = new Texture2D(CamViewer.Current.webCamTex.width, CamViewer.Current.webCamTex.height);
		image.SetPixels(CamViewer.Current.webCamTex.GetPixels());
		image.Apply();
	}
}
