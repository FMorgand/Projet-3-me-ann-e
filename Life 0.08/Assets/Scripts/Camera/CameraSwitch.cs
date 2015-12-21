using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraSwitch : MonoBehaviour 
{
	[SerializeField] camType activeCamera;
	
	GameObject camera2D, cameraIso, camera3D;

	[SerializeField] KeyCode key_2D, key_Iso, key_3D;

	public enum camType {
		Cam2D,
		CamIso,
		Cam3D
	}

	void OnValidate ()
	{
		ChangeCam();
	}

	void ChangeCam ()
	{
		if (camera2D == null || cameraIso == null || camera3D == null) {
			GetCameras ();
		}

		camera2D.SetActive(false);
		cameraIso.SetActive(false);
		camera3D.SetActive(false);

		switch (activeCamera)
		{
		case camType.Cam2D:
			camera2D.SetActive(true);
			break;
		case camType.CamIso:
			cameraIso.SetActive(true);
			break;
		case camType.Cam3D:
			camera3D.SetActive(true);
			break;
		}
	}

	void GetCameras ()
	{
		Debug.Log("Loading Cameras");

		camera2D = transform.FindChild("Camera 2D").gameObject;
		cameraIso = transform.FindChild("Camera Iso").gameObject;
		camera3D = transform.FindChild("Camera 3D").gameObject;
	}

	void Update () 
	{
		if (Input.GetKeyDown (key_2D)) {
			activeCamera = camType.Cam2D;
			ChangeCam();
		}
		if (Input.GetKeyDown (key_Iso)) {
			activeCamera = camType.CamIso;
			ChangeCam();
		}
		if (Input.GetKeyDown (key_3D)) {
			activeCamera = camType.Cam3D;
			ChangeCam();
		}
	}
}
