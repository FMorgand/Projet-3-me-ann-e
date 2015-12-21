using UnityEngine;
using System.Collections;

public class WallSpawner : MonoBehaviour {

	public GameObject _wall;
	public bool _startOk = false;
	private Vector3 _startingPoint;
	public bool _endOk = false;
	private Vector3 _endingPoint;
	public GameObject _wallPreview;

	// Use this for initialization
	void Start () {
		_wallPreview = Instantiate (_wall, Vector3.zero, Quaternion.identity) as GameObject;
		Destroy( _wallPreview.GetComponent<NavMeshObstacle> ());
		_wallPreview.SetActive (false);
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
						_wallPreview.SetActive (true);
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
					_wallPreview.transform.position = ( _endingPoint - (_endingPoint - _startingPoint)/2);
					_wallPreview.transform.localScale = new Vector3( Vector3.Distance(_startingPoint, _endingPoint), _wallPreview.transform.localScale.y, _wallPreview.transform.localScale.z);
					_wallPreview.transform.LookAt( new Vector3(_startingPoint.x, 0, _startingPoint.z));
					_wallPreview.transform.right = (_endingPoint - _startingPoint);
				}
			}
		}

		if(Input.GetMouseButtonUp(0) && _startOk)
		{
			if(Physics.Raycast(ray.origin, ray.direction, out hit))
			{
				_endOk = true;
				_wallPreview.SetActive(false);
			}
		}

		if(_endOk && _startOk)
		{
			GameObject newWall = Instantiate(_wall, _endingPoint - (_endingPoint - _startingPoint)/2, Quaternion.identity) as GameObject;
			newWall.transform.localScale = new Vector3( Vector3.Distance(_startingPoint, _endingPoint), newWall.transform.localScale.y, newWall.transform.localScale.z);
			newWall.transform.LookAt( new Vector3(_startingPoint.x, 0, _startingPoint.z));
			newWall.transform.right = (_endingPoint - _startingPoint);
			_endOk = false;
			_startOk = false;
		}
	}
}
