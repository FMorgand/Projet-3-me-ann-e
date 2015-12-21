using UnityEngine;
using System.Collections;

public class PlayerActionManager : MonoBehaviour {

	public WallSpawner _wallSpawn;
	public PlantZoneSpawner  _plantZoneSpawn;
	public GameObject _creationPanel;
	public GameObject _wallSpawnDisplay;
	public GameObject _plantSpawnDisplay;
	public GameObject _CreationSpeciesTool;

	// Use this for initialization
	void Start () {
		_wallSpawn = this.GetComponent<WallSpawner> ();
		_plantZoneSpawn = this.GetComponent<PlantZoneSpawner> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			_wallSpawn.enabled = true;
			_plantZoneSpawn.enabled = false;
			_creationPanel.SetActive(false);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			_plantZoneSpawn.enabled = true; 
			_wallSpawn.enabled = false;
			_creationPanel.SetActive(false);
		}

		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			_plantZoneSpawn.enabled = false; 
			_wallSpawn.enabled = false;
			_creationPanel.SetActive(true);
		}

		if(_wallSpawn.enabled)
		{
			_wallSpawnDisplay.SetActive(true);
		}
		else
		{
			_wallSpawnDisplay.SetActive(false);
		}

		if(_plantZoneSpawn.enabled)
		{
			_plantSpawnDisplay.SetActive(true);
		}
		else
		{
			_plantSpawnDisplay.SetActive(false);
		}

		if(_creationPanel.activeInHierarchy)
		{
			_CreationSpeciesTool.SetActive(true);
		}
		else
		{
			_CreationSpeciesTool.SetActive(false);
		}

	}
}
