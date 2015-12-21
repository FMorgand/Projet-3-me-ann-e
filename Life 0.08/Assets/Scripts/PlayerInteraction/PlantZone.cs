using UnityEngine;
using System.Collections;

public class PlantZone : MonoBehaviour {

	public float _spawnRate;
	public GameObject _prefabs;
	private EntityManager _manager;

	// Use this for initialization
	void Start () {
		_manager = GameObject.Find ("EntityManager").GetComponent<EntityManager> ();
		StartCoroutine ("TimerSpawn");
	}
	
	// Update is called once per frame
	void Update () {
	


	}

	IEnumerator TimerSpawn()
	{
		for(;;)
		{
			yield return  new WaitForSeconds(_spawnRate);

			float x = Random.Range (this.transform.position.x - this.transform.localScale.x/2, this.transform.position.x + this.transform.localScale.x/2);
			float y = 1f;
			float z = Random.Range (this.transform.position.z - this.transform.localScale.z/2, this.transform.position.z + this.transform.localScale.z/2);

			Vector3 pso = new Vector3(x,y,z);

			_manager.StockNewEntityInList(_manager.CreateEntity(_prefabs, pso, Color.black));
		}
	}
}
