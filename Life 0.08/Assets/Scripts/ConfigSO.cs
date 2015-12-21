using UnityEngine;
using System.Collections;

public class ConfigSO : ScriptableObject 
{
	public Description description;

	public Vector3 mapSize;

	public EntityManager.EatMechanics eatMode;
	public float eatValue;
	public bool canOutnumberMaxEnergy = false;

	public GameObjectWithRate[] entitiesFromStart;
	public GameObjectWithRate[] entitiesSpawnRate;
	
	public bool activateSpawn = true;
	public float spawnTime; 

	public bool darwinEvolution = false;

	public void GetStats (EntityManager man) 
	{
		description = man.description;
		mapSize = man.mapSize;
		eatMode = man.eatMode;
		eatValue = man.eatValue;
		canOutnumberMaxEnergy = man.canOutnumberMaxEnergy;
		entitiesFromStart = man.entitiesFromStart;
		entitiesSpawnRate = man.entitiesSpawnRate;
		activateSpawn = man.activateSpawn;
		spawnTime = man.spawnTime;
		darwinEvolution = man.darwinEvolution;
	}
}