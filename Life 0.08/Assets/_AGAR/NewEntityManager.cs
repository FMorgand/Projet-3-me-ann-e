using UnityEngine;
using System.Collections;

public class NewEntityManager : MonoBehaviour {

	public int nbToSpawn;
	public GameObject entity;
	public Vector2 dimensions;

	void Start () {
		for (int i = 0; i < nbToSpawn; i++) {
			float x = Random.Range (-dimensions.x, dimensions.x);
			float y = Random.Range (-dimensions.y, dimensions.y);
			Vector3 position = new Vector3(x, 0, y);
			
			Instantiate (entity, position, Quaternion.identity);
			timer = spawnTime;
		}
	}
	
	public float spawnTime;
	float timer;

	void Update () 
	{
		if (timer <= 0) 
		{
			float x = Random.Range (-dimensions.x, dimensions.x);
			float y = Random.Range (-dimensions.y, dimensions.y);
			Vector3 position = new Vector3(x, 0, y);
			
			Instantiate (entity, position, Quaternion.identity);
			timer = spawnTime;
		}
		timer -= Time.deltaTime;
	}
}
