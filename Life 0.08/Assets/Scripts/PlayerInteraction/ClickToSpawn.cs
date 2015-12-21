using UnityEngine;
using System.Collections;

public class ClickToSpawn : MonoBehaviour {

	public GameObject _green;
	public GameObject _blue;
	public GameObject _red;
	public GameObject _yellow;
	public GameObject _purple;

	public GameObject _prefabSelected;

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
					Instantiate(_prefabSelected, new Vector3( hit.point.x, hit.point.y+10f, hit.point.z), Quaternion.identity);
				}

			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			_prefabSelected = _green;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			_prefabSelected = _blue;
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			_prefabSelected = _red;
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			_prefabSelected = _yellow;
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			_prefabSelected = _purple;
		}

	}
}
