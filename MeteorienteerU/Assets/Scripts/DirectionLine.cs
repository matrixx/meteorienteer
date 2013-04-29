using UnityEngine;
using System.Collections;

public class DirectionLine : MonoBehaviour
{
	public Transform startedFrom;
	public Transform wentTo;
	public LineRenderer line;
	
	public static DirectionLine Singleton {get; private set;}
	
	private Transform movedPoint;
	
	void Awake()
	{
		Singleton = this;
	}
	
	void OnEnable()
	{
		startedFrom.position = CamViewer.Current.transform.position 
			 - Vector3.right * CamViewer.Current.renderer.bounds.extents.x / 3f
			 - Vector3.forward * 0.1f;
		wentTo.position = CamViewer.Current.transform.position
			+ Vector3.right * CamViewer.Current.renderer.bounds.extents.x / 3f
			- Vector3.forward * 0.1f;
		startedFrom.gameObject.SetActiveRecursively(true);
		wentTo.gameObject.SetActiveRecursively(true);
		line.gameObject.SetActiveRecursively(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			
			if (startedFrom.collider.Raycast(touchRay, out hit, 20f))
			{
				movedPoint = startedFrom;
				
			}
			else if (wentTo.collider.Raycast(touchRay, out hit, 20f))
			{
				movedPoint = wentTo;
			}
		}
		
		if (movedPoint)
		{
			Plane cameraPlane = new Plane(CamViewer.Current.transform.up, CamViewer.Current.transform.position - Vector3.forward * 0.1f);
			float distance;
			cameraPlane.Raycast(touchRay, out distance);
			movedPoint.position = touchRay.GetPoint(distance);
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			movedPoint = null;
		}
		
		wentTo.right = (wentTo.position - startedFrom.position).normalized;
		
		line.SetPosition(0, startedFrom.position);
		line.SetPosition(1, wentTo.position);
	}
	
	void OnDisable()
	{
		startedFrom.gameObject.SetActiveRecursively(false);
		wentTo.gameObject.SetActiveRecursively(false);
		line.gameObject.SetActiveRecursively(false);
	}
}
