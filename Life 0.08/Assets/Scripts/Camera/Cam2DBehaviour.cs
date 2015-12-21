using UnityEngine;
using System.Collections;

public class Cam2DBehaviour : MonoBehaviour 
{
	Camera cam;

	public float zoomSpeed;
	public Interval zoomLimit;

	public int _borderSize;
	float range { get { 
			return (float)_borderSize;
		} set {
			_borderSize = Mathf.RoundToInt(value);
		}
	}
	public float byBorderSpeed;

	public float byKeyboardSpeed;
	public KeyCode up = KeyCode.UpArrow;
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode right = KeyCode.RightArrow;

	void Start () 
	{
		cam = GetComponent<Camera>();
	}

	void Update () 
	{
		// Zoom
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.orthographicSize < zoomLimit.max) 
		{
			cam.orthographicSize += zoomSpeed;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.orthographicSize > zoomLimit.min) 
		{
			cam.orthographicSize -= zoomSpeed;
		}
		Mathf.Clamp (cam.orthographicSize, zoomLimit.min, zoomLimit.max);

		// Displacement by mouse approaching borders 
		Vector3 mPos = Input.mousePosition;
		if (mPos.x > 0 && mPos.x < Screen.width) { 
			if (mPos.x < range / 2) {
				transform.Translate(Vector3.left * byBorderSpeed * 2);
			} else if (mPos.x < range) {
				transform.Translate(Vector3.left * byBorderSpeed);
			} else if (mPos.x > Screen.width - range / 2) {
				transform.Translate(Vector3.right * byBorderSpeed * 2);
			} else if (mPos.x > Screen.width - range) {
				transform.Translate(Vector3.right * byBorderSpeed);
			}
		}
		if (mPos.y > 0 && mPos.y < Screen.height) { 
			if (mPos.y < range / 2) {
				transform.Translate(Vector3.down * byBorderSpeed * 2);
			} else if (mPos.y < range) {
				transform.Translate(Vector3.down * byBorderSpeed);
			} else if (mPos.y > Screen.height - range / 2) {
				transform.Translate(Vector3.up * byBorderSpeed * 2);
			} else if (mPos.y > Screen.height - range) {
				transform.Translate(Vector3.up * byBorderSpeed);
			}
		}

		// Displacement by keyboard
		if (Input.GetKey(up)) { 
			transform.Translate(Vector3.up * byKeyboardSpeed); 
		} else if (Input.GetKey(down)) {
			transform.Translate(Vector3.down * byKeyboardSpeed); }
		if (Input.GetKey(left)) { 
			transform.Translate(Vector3.left * byKeyboardSpeed); 
		} else if (Input.GetKey(right)) { 
			transform.Translate(Vector3.right * byKeyboardSpeed); }
	}
}
