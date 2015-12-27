using UnityEngine;
using System.Collections;

public class ZoneSpawner : MonoBehaviour {

	public GameObject _terrain;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(ray.origin, ray.direction, out hit))
			{
				if(hit.collider.gameObject.tag == "Ground")
				{
					Instantiate(_terrain, hit.point, Quaternion.identity);
				}
				
			}
		}
	}
}
