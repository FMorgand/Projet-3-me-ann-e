using UnityEngine;
using System.Collections;

public class PlantZone : BaseTerrain {

	public float _spawnRate;
	public GameObject _prefabs;
	private EntityManager _manager;

	// Use this for initialization
	void Start () {
		_manager = GameObject.Find ("EntityManager").GetComponent<EntityManager> ();
		StartCoroutine ("TimerSpawn");
		_radius = this.transform.localScale.x;
	}
	
	// Update is called once per frame
	IEnumerator TimerSpawn()
	{
		for(;;)
		{
			yield return  new WaitForSeconds(_spawnRate);

			Vector2 V2pos = Random.insideUnitCircle * (_radius/2);
			Vector3 pos = new Vector3(this.transform.position.x + V2pos.x,this.transform.position.y,this.transform.position.z + V2pos.y);

			_manager.StockNewEntityInList(_manager.CreateEntity(_prefabs, pos, Color.black));
		}
	}
}
