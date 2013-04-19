using UnityEngine;
using System.Collections;

public class DirectionLine : MonoBehaviour
{
	public Transform startedFrom;
	public Transform wentTo;
	public LineRenderer line;
	
	void OnEnable()
	{
		startedFrom.position = CamViewer.Current.transform.position - Vector3.right * CamViewer.Current.renderer.bounds.extents.x / 2f;
		wentTo.position = CamViewer.Current.transform.position + Vector3.right * CamViewer.Current.renderer.bounds.extents.x / 2f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			if (startedFrom.collider.Raycast(touchRay, out hit, 20f))
			{
				//TODO: startedFrom.position = ?
			}
			else if (wentTo.collider.Raycast(touchRay, out hit, 20f))
			{
				//TODO:
			}
		}
		
		line.SetPosition(0, startedFrom.position);
		line.SetPosition(1, wentTo.position);
	}
}
