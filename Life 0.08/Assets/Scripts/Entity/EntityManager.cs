using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class EntityManager : MonoBehaviour 
{
	public ConfigSO loadScriptableObject;

	public string currentConfigName = "none";
	public Description description;

	public GameObject _entityMainPrefab;

	public float timeSpeed = 1f;
	public Vector3 mapSize;

	public GameObjectWithRate[] entitiesFromStart;

	public bool activateSpawn = true;
	public float spawnTime; 
	public GameObjectWithRate[] entitiesSpawnRate;

	static public int _ressources = 0;


	
	public enum EatMechanics {
		RestoreMaxEnergyPercentage, // energyRestored : 0 to 1
		FixedAmount, // energyRestored : 0 to infinite
		FixedByPrey, // don't look energyRestored 
		RestorePreyMaxEnergyPercentage, // energyRestored : 0 to 1 
		RestorePreyCurrentEnergyPercentage // energyRestored : 0 to 1 
	}
	public EatMechanics eatMode;
	public float eatValue;
	public bool canOutnumberMaxEnergy = false;
	
	public bool darwinEvolution = false;

	float spawn_timer;

	public List<string> _speciesName = new List<string>();
	public List<GameObject> _speciesPrefabs = new List<GameObject> ();
	public List<List<Entity>> _speciesList = new List<List<Entity>>();


	void Awake () 
	{
		spawn_timer = spawnTime;
		//ActualizeDataFromScriptableObject ();
		CreateStartEntities ();
	}

	void Update () 
	{
		if (activateSpawn) {
			if (spawn_timer <= 0f) {
				if (spawnTime == 0f) {
					activateSpawn = false;
				} else {
					CreateRandomEntity ();
				}
				spawn_timer = spawnTime;
			} else {
				spawn_timer -= Time.deltaTime * timeSpeed;
			}
		}
	}

	void ActualizeDataFromScriptableObject ()
	{
		if (loadScriptableObject != null) {
			currentConfigName = loadScriptableObject.name;
			description = loadScriptableObject.description;
			mapSize = loadScriptableObject.mapSize;
			entitiesFromStart = loadScriptableObject.entitiesFromStart;
			entitiesSpawnRate = loadScriptableObject.entitiesSpawnRate;
			eatMode = loadScriptableObject.eatMode;
			eatValue = loadScriptableObject.eatValue;
			canOutnumberMaxEnergy = loadScriptableObject.canOutnumberMaxEnergy;
			activateSpawn = loadScriptableObject.activateSpawn;
			spawnTime = loadScriptableObject.spawnTime;
			darwinEvolution = loadScriptableObject.darwinEvolution;

			loadScriptableObject = null;
		}
	}

	void CreateStartEntities () 
	{
		foreach (GameObjectWithRate entity in entitiesFromStart) {
			Debug.Log (entity.prefab);
			if (entity.prefab != null) {

				_speciesName.Add(entity.prefab.name);
				int index = _speciesName.IndexOf(entity.prefab.name);
				_speciesPrefabs.Add(entity.prefab);
				List<Entity> newRace = new List<Entity>();
				_speciesList.Add(newRace);
				
				for (int i = 0; i < entity.rate; i++) {

					_speciesList[index].Add(CreateEntity (entity.prefab));
				}
			} else {
				Debug.LogError ("Missing prefab in Manager");
			}
		}
	}
	
	// Creates a random entity at a random place on the map
	void CreateRandomEntity () 
	{
		int range = 0;
		foreach (GameObjectWithRate entity in entitiesSpawnRate)
		{
			range += entity.rate;
		}
		int number = Random.Range (0, range);

		foreach (GameObjectWithRate entity in entitiesSpawnRate) {
			if (number < entity.rate) {
				StockNewEntityInList(CreateEntity (entity.prefab));
			} else {
				number -= entity.rate;
			}
		}
	}

	public void CreateNewRace(string name,float speed, float rangeOfView, float lifeTime, float maxEnergy,
	                          float reproductionTime, Entity[] predators, GameObjectWithRate[] preys)
	{
		GameObject newRace = _entityMainPrefab;
		newRace.name = name;
		Entity newRaceParameter = newRace.GetComponent<Entity> ();

		newRaceParameter._speed.min = speed - 5f;
		newRaceParameter._speed.max = speed + 5f;

		newRaceParameter._rangeOfView.min = rangeOfView - 5f;
		newRaceParameter._rangeOfView.max = rangeOfView + 5f;

		newRaceParameter._lifetime.min = lifeTime - 5f;
		newRaceParameter._lifetime.max = lifeTime + 5f;

		newRaceParameter._maxEnergy.min = maxEnergy - 5f;
		newRaceParameter._maxEnergy.max = maxEnergy + 5f;

		newRaceParameter._reproductionTime.min = reproductionTime - 5f;
		newRaceParameter._reproductionTime.max = reproductionTime + 5f;

		newRaceParameter.fleeFromEntities = predators;

		newRaceParameter.canEatEntities = preys;

		Color newColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));

		newRace.transform.FindChild ("Sprite").GetComponent<SpriteRenderer> ().color = newColor;

		_speciesName.Add(newRace.name);
		int index = _speciesName.IndexOf(newRace.name);
		_speciesPrefabs.Add(newRace);
		List<Entity> newSpecies = new List<Entity>();
		_speciesList.Add(newSpecies);

		_speciesList[index].Add(CreateEntity (newRace));

	}

	Entity CreateEntity (GameObject prefab, Color color) 
	{
		float x = Random.Range (-mapSize.x, mapSize.x);
		float y = Random.Range (-mapSize.y, mapSize.y);
		float z = Random.Range (-mapSize.z, mapSize.z);
		Vector3 position = new Vector3(x, y, z);
		
		GameObject newEntity = (GameObject)Instantiate (prefab, position, Quaternion.identity);
		newEntity.name = prefab.name;

		return newEntity.GetComponent<Entity>();
	}

	public Entity CreateEntity (GameObject prefab,Vector3 position, Color color) 
	{
		GameObject newEntity = (GameObject)Instantiate (prefab, position, Quaternion.identity);
		newEntity.name = prefab.name;

		//StockNewEntityInList(newEntity.GetComponent<Entity>());
		
		return newEntity.GetComponent<Entity>();
	}


	Entity CreateEntity (GameObject prefab) 
	{
		float x = Random.Range (-mapSize.x, mapSize.x);
		float y = Random.Range (-mapSize.y, mapSize.y);
		float z = Random.Range (-mapSize.z, mapSize.z);
		Vector3 position = new Vector3(x, y, z);

		GameObject newEntity = (GameObject)Instantiate (prefab, position, Quaternion.identity);
		newEntity.name = prefab.name;

		return newEntity.GetComponent<Entity>();
	}

	public void StockNewEntityInList(Entity target)
	{
		int index = _speciesName.IndexOf(target.name);
		
		_speciesList[index].Add(target.GetComponent<Entity>());

	}

	void OnValidate ()
	{
		ActualizeDataFromScriptableObject();
	}
}
