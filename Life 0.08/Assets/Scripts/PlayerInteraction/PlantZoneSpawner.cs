using UnityEngine;
using System.Collections;

public class PlantZoneSpawner : MonoBehaviour {

	public GameObject _SpawnZone;
	public bool _startOk = false;
	private Vector3 _startingPoint;
	public bool _endOk = false;
	private Vector3 _endingPoint;
	public GameObject _SpawnPreview;
	
	// Use this for initialization
	void Start () {
		_SpawnPreview = Instantiate (_SpawnZone, Vector3.zero, Quaternion.identity) as GameObject;
		Destroy( _SpawnPreview.GetComponent<NavMeshObstacle> ());
		_SpawnPreview.SetActive (false);
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
					if(_startOk == false)
					{
						_startingPoint = hit.point;
						_startOk = true;
						_SpawnPreview.SetActive (true);
					}
				}
				
			}
		}
		
		if(Input.GetMouseButton(0) && _startOk)
		{
			if(Physics.Raycast(ray.origin, ray.direction, out hit))
			{
				if(hit.collider.gameObject.tag == "Ground")
				{
					_endingPoint = hit.point;
					_SpawnPreview.transform.position = new Vector3 ((_endingPoint.x + _startingPoint.x )/2  , 0, ( _endingPoint.z + _startingPoint.z)/2);
					_SpawnPreview.transform.localScale = new Vector3( _startingPoint.x - _endingPoint.x, 10,  _startingPoint.z - _endingPoint.z);
					//_SpawnPreview.transform.LookAt( new Vector3(_startingPoint.x, 0, _startingPoint.z));
					//_SpawnPreview.transform.right = (_endingPoint - _startingPoint);
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0) && _startOk)
		{
			if(Physics.Raycast(ray.origin, ray.direction, out hit))
			{
				_endOk = true;
				_SpawnPreview.SetActive(false);
			}
		}
		
		if(_endOk && _startOk)
		{
			GameObject newWall = Instantiate(_SpawnZone, new Vector3 ((_startingPoint.x + _endingPoint.x)/2  , 0, (_startingPoint.z + _endingPoint.z)/2), Quaternion.identity) as GameObject;
			newWall.transform.localScale = new Vector3( _startingPoint.x - _endingPoint.x, 10,  _startingPoint.z - _endingPoint.z);
			//newWall.transform.LookAt( new Vector3(_startingPoint.x, 0, _startingPoint.z));
			//newWall.transform.right = (_endingPoint - _startingPoint);
			_endOk = false;
			_startOk = false;
		}
	}
}
