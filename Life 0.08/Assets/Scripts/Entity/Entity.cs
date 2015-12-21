using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour 
{
	public enum Status {
		idle,
		moving,
		hunting,
		fleeing,
		loving
	}

	#region Parameters to configure in inspector
	// A float value will be randomly generated between range of given interval
	public Interval _speed;
	public Interval _rangeOfView;
	public Interval _lifetime;
	public Interval _maxEnergy;
	[Range(0f, 1f)] public float _hungryLimit; // Fraction of Max Energy
	public Interval _reproductionTime;
	public Entity[] fleeFromEntities;
	public GameObjectWithRate[] canEatEntities;
	#endregion

	#region Real parameters
	[HideInInspector] public float speed;
	[HideInInspector] public float rangeOfView; 
	[HideInInspector] public float lifetime; 
	float lifetimer;
	[HideInInspector] public float maxEnergy; 
	[HideInInspector] public float energy;
	[HideInInspector] public float hungryLimit; 
	bool hungry;
	[HideInInspector] public float reproductionTime;
	float reproTimer;
	bool canReproduct= true;
	Status status = Status.idle; 
	public Status statusDisplay;
	public Vector3 destination;
	EntityManager Manager;
	private NavMeshAgent _agent;

	private List<Entity> _mates;
	#endregion

	#region Awake
	public void Awake () 
	{
		Manager = FindObjectOfType<EntityManager> ();
		tag = "Entity";
		_agent = this.GetComponent<NavMeshAgent> ();
	
		speed = Mathf.Max(_speed.Random (), 0f);
		rangeOfView = Mathf.Max(_rangeOfView.Random (), 0f);
		lifetime = Mathf.Max(_lifetime.Random (), 0f);
		maxEnergy = Mathf.Max(_maxEnergy.Random (), 0f);
		hungryLimit = _hungryLimit * maxEnergy;
		reproductionTime = Mathf.Max(_reproductionTime.Random (), 1f);

		_agent.speed = speed;

		energy = maxEnergy;
		reproTimer = reproductionTime;
		lifetimer = lifetime;

		if (reproductionTime <= 0f) {
			canReproduct = false;
		} else {
			canReproduct = true;
		}
	}

	void Start()
	{
		int index = Manager._speciesName.IndexOf (this.gameObject.name);
		_mates = Manager._speciesList[index];
	}
	#endregion

	#region Update
	void Update () 
	{

		statusDisplay = status;
		// Lifetime gestion
		if (lifetimer <= 0) {
			Die ();
		} else {
			lifetimer -= Time.deltaTime * Manager.timeSpeed;
		}

		// Energy gestion
		if (energy <= 0) {
			Die ();
		} else if (!hungry && energy <= hungryLimit) {
			hungry = true;
			status = Status.hunting;
		} 
		energy -= Time.deltaTime * Manager.timeSpeed;

		// Reproduction time gestion
		if (!hungry && canReproduct) {
			if (reproTimer <= 0) {
				status = Status.loving;
			} else {
				reproTimer -= Time.deltaTime * Manager.timeSpeed;
			}
		}

		// IA
		switch (status) {
		case Status.fleeing: flee(); break;
		case Status.idle: idle (); break;
		case Status.moving: move (); break;
		case Status.hunting: hunt(); break;
		case Status.loving: love(); break;
		default : idle (); break;
		}
	}

	void Die () 
	{
		Destroy(this.gameObject);
		int index = Manager._speciesName.IndexOf (this.gameObject.name);
		Manager._speciesList[index].Remove(this);
	}
	#endregion

	#region Behaviors
	void flee ()
	{
		// Looking for a predator in sight
		List<Entity> nearPredators = CheckFor ("predator");
		
		if (nearPredators.Capacity == 0) {
			// Lost predator 
			status = Status.idle;
		} else {
			// Fleeing
			Vector3 predatorPosition = GetNearestEntity (nearPredators);
			_agent.destination = transform.position - (predatorPosition - transform.position);
			//transform.position -= Vector3.Normalize (predatorPosition - transform.position) * speed * Manager.timeSpeed;
		}
	}
	/*{
		// Looking for a predator in sight
		List<Entity> nearPredators = CheckFor ("predator");

		if (nearPredators.Capacity == 0) {
			// Lost predator 
			status = Status.idle;
		} else {
			// Fleeing
			Vector3 predatorPosition = GetNearestEntity (nearPredators);
			transform.position -= Vector3.Normalize (predatorPosition - transform.position) * speed * Manager.timeSpeed;
		}
	}*/

	void move ()
	{
		Vector3 distance = destination - transform.position;
		if (distance.magnitude < 1) {
			status = Status.idle;
		} else {
			if(_agent.destination != destination)
				_agent.destination = destination;
		}
	}

	void move (Vector3 direction)
	{
		_agent.destination = direction;
	}
	/*{		
		Vector3 distance = destination - transform.position;
		if (distance.magnitude < 1) {
			status = Status.idle;
		} else {
			transform.position += speed * Vector3.Normalize(destination - transform.position) / 2 * factor * Manager.timeSpeed;
		}
	}*/

	void hunt ()
	{
		move ();

		// Looking for a prey in sight
		List<Entity> nearPreys = CheckFor ("prey");
		
		if (nearPreys.Capacity != 0) 
		{
			// Prey found ! Chasing.
			Vector3 preyPosition = GetNearestEntity (nearPreys);
			
			Vector3 distance = preyPosition - transform.position;
			if (distance.magnitude < 1.2f) {
				EatAround ();
			} else {
				_agent.destination = preyPosition;
			}
		}
		
	}
	/*{
		// Looking for a prey in sight
		List<Entity> nearPreys = CheckFor ("prey");

		if (nearPreys.Capacity == 0) {
			destination = GetRandomDestination ();
			move (2f);
		} else {
			// Prey found ! Chasing.
			Vector3 preyPosition = GetNearestEntity (nearPreys);

			Vector3 distance = preyPosition - transform.position;
			if (distance.magnitude < 1) {
				EatAround();
			} else {
				transform.position += Vector3.Normalize (preyPosition - transform.position) * speed * Manager.timeSpeed;
			}
		}
	}*/

	void idle ()
	{
		if (hungry) {
			status = Status.hunting;
			destination = GetRandomDestination ();
		}

		if (Random.Range(0, 100 / Mathf.CeilToInt(Manager.timeSpeed)) == 0) {
			destination = GetRandomDestination ();
			status = Status.moving;
		}
	}

	void love () 
	{
		MakeABaby ();
	}
	#endregion

	Vector3 GetRandomDestination () 
	{
		float x = Random.Range (-Manager.mapSize.x, Manager.mapSize.x);
		float y = Random.Range (-Manager.mapSize.y, Manager.mapSize.y);
		float z = Random.Range (-Manager.mapSize.z, Manager.mapSize.z);
		return new Vector3 (x, y, z);
	}

	// This method is called when an entity enters in the range of view by the script EntitySightSphere
	public void encounterEntity (Entity entity) 
	{
		// If encounters predator, flee
		if (isPredator (entity) && status != Status.hunting) {
			status = Status.fleeing;
		}
	}

	#region Reproduction Mechanics
	void MakeABaby () 
	{
		GameObject baby = (GameObject) Instantiate (this.gameObject, transform.position , Quaternion.identity);
		baby.name = name;
		Entity babyScript = baby.GetComponent<Entity>();
		if (Manager.darwinEvolution) {

			babyScript._speed = Darwinate (_speed, speed);
			babyScript._rangeOfView = Darwinate (_rangeOfView, rangeOfView);
			babyScript._lifetime = Darwinate (_lifetime, lifetime);
			babyScript._maxEnergy = Darwinate (_maxEnergy, maxEnergy);
			babyScript._reproductionTime = Darwinate (_reproductionTime, reproductionTime);
		}

		Manager.StockNewEntityInList (babyScript);

		reproTimer = reproductionTime;
		status = Status.idle;
	}

	/// <summary>Centers the range interval of the child on the parent stat.</summary>
	Interval Darwinate (Interval parentRange, float parentStat) 
	{
		return parentRange.CenterOn(parentStat);
	}
	#endregion

	#region Checks
	Vector3 GetNearestEntity (List<Entity> entities) 
	{
		Vector3 nearestPosition = entities[0].transform.position;
		
		foreach (Entity predator in entities) {
			if (Vector3.Distance(predator.transform.position, transform.position) < Vector3.Distance (nearestPosition, transform.position)) {
				nearestPosition = predator.transform.position;
			} 
		}
		return nearestPosition;
	}
	
	List<Entity> GetNearPredators ()
	{
		return CheckFor ("predator");
	}

	List<Entity> GetNearPreys ()
	{
		return CheckFor ("preys");
	}

	/// <summary>Check around for all entities of a type. Type can be "mate", "prey" or "predator"</summary>
	List<Entity> CheckFor (string typeSearched) 
	{
		// Get all objects with a collider within the range of view
		Collider[] allEntities = Physics.OverlapSphere (this.transform.position, rangeOfView);

		// Keep only entities
		List<Entity> targets = new List<Entity> ();
		foreach (Collider entity in allEntities) {
			if (entity != null && entity.tag == "Entity" && entity != this.GetComponent<Collider>()) {
				Entity thisEntity = entity.gameObject.GetComponent<Entity>();
				if ((typeSearched == "mate" && thisEntity != null && isMate (thisEntity) ||
				    typeSearched == "prey" && isPrey (thisEntity) ||
				    typeSearched == "predator" && isPredator (thisEntity)) && thisEntity != null) {
						targets.Add (entity.gameObject.GetComponent<Entity>());
				}
			}
		}
		return targets;
	}

	bool isPredator (Entity entity) 
	{
		foreach (Entity predator in fleeFromEntities) {
			if (entity != null && predator != null)	
			{
				if(predator.name == entity.name) {
					return true;
				}
			}
		}
		return false;
	}

	bool isPrey (Entity entity) 
	{
		foreach (GameObjectWithRate prey in canEatEntities) {
			if (entity != null && prey.prefab != null)	
			{
				if (prey.prefab.name == entity.name) {
					return true;
				}
			}
		}
		return false;
	}

	bool isMate (Entity entity) 
	{
		if (name == entity.name) {
			return true;
		}
		return false;
	}
	#endregion

	#region Eat Mechanics
	void EatAround()
	{
		Collider[] allEntities = Physics.OverlapSphere(this.transform.position, 1f);
		foreach (Collider entity in allEntities) {
			if (entity != null && entity.tag != "Trigger" && entity.tag != "Ground") {
				if (isPrey(entity.gameObject.GetComponent<Entity>())) {
					Eat(entity.gameObject.GetComponent<Entity>());
				}
			}
		}
	}

	void Eat(Entity entity)
	{
		switch (Manager.eatMode)
		{
		case EntityManager.EatMechanics.RestorePreyMaxEnergyPercentage:
			energy += entity.energy * Manager.eatValue;
			break;
		case EntityManager.EatMechanics.RestorePreyCurrentEnergyPercentage:
			energy += entity.maxEnergy * Manager.eatValue;
			break;
		case EntityManager.EatMechanics.FixedByPrey:
			foreach (GameObjectWithRate prey in canEatEntities) {
				if (entity.name == prey.prefab.name) {
					energy += prey.rate;
				}
			}
			break;
		case EntityManager.EatMechanics.FixedAmount:
			energy += Manager.eatValue;
			break;
		case EntityManager.EatMechanics.RestoreMaxEnergyPercentage:
			energy += maxEnergy * Manager.eatValue;
			break;
		}
		if (!Manager.canOutnumberMaxEnergy) {
			Mathf.Clamp (energy, 0f, maxEnergy);
		} 
		entity.Die ();
		hungry = false;
		status = Status.idle;
	}
	#endregion

	#region Gizmo

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color (1,0,0,0.4f);
		Gizmos.DrawSphere (_agent.destination, 2);

		Debug.DrawLine (this.transform.position, _agent.destination, Color.red);
	}

	#endregion

	#region Boids

	public Vector3 _position;

	public float _maxDistance = 7f;
	public float _minDistance = 2f;

	Vector3 _desiredPostion;

	void BoidBehaviour()
	{
		_position = this.transform.position;

		_desiredPostion = (Barycentre () +  Repulsion ());

		move (_desiredPostion);
	}

	Vector3 Barycentre()
	{
		Vector3 barycentre = Vector3.zero;
		int nbFriends = 0;
		foreach (Entity boid in _mates) 
		{
			if (boid != this && Vector3.Distance(_position, boid.transform.position) <= rangeOfView)
			{
				barycentre += boid.transform.position;
				nbFriends++;
			}
		}
		if (nbFriends != 0) 
		{ 
			return (barycentre / nbFriends);
		} 
		else 
		{
			return Vector3.zero;
		}
	}

	Vector3 Repulsion()
	{
		Vector3 repulsionValue = Vector3.zero;
		int nbFriends = 0;
		foreach (Entity boid in _mates) 
		{
			if (boid != this && Vector3.Distance(_position, boid.transform.position) <= _minDistance)
			{
				Vector3 rep = this.transform.position - boid.transform.position;
				float dist = rep.magnitude;
				rep.Normalize();

				repulsionValue += (rep/dist) ;
				nbFriends++;
			}
		}

		if (nbFriends != 0) 
		{ 
			return _position - ( repulsionValue / nbFriends);
		} 
		else 
		{
			return Vector3.zero;
		}
	}

	Vector3 Allignement()
	{
		Vector3 velocitySum = Vector3.zero;
		int nbFriends = 0;
		foreach (Entity boid in _mates) 
		{
			if (boid != this && Vector3.Distance(_position, boid._position) <= _maxDistance)
			{
				velocitySum += boid._agent.velocity;
				nbFriends++;
			}
		}
		if (nbFriends != 0) 
		{ 
			velocitySum = velocitySum/nbFriends;
		} 
		else 
		{
			velocitySum = _agent.velocity;
		}

		Vector3 final = (velocitySum - _agent.velocity);
		final.Normalize ();

		return final;
	}



	/*void CheckForMate()
	{
		GameObject toCloseMate = this.gameObject;
		float distanceToToCloseMate = 0f;
		GameObject targetedMate = this.gameObject;
		float distanceToTarget = 0f;

		foreach(GameObject i in _mates)
		{
			float dist = Vector3.Distance(this.gameObject.transform.position, i.transform.position);

			if(dist < rangeOfView)
		   	{
				if(targetedMate == this.gameObject && dist > 3f)
				{
					targetedMate = i.gameObject;
					distanceToTarget = dist;
				}
				else if(dist < distanceToTarget && dist > 1.5f)
				{
					targetedMate = i.gameObject;
					distanceToTarget = dist;
				}

				if(dist < 3f && (toCloseMate == this.gameObject|| distanceToToCloseMate > dist))
				{
					toCloseMate = i.gameObject;
					distanceToToCloseMate = dist;
				}
			}
		}

		if(targetedMate == this.gameObject && toCloseMate == this.gameObject)
		{
			move();
		}
		else
		{
			Vector3 towardMate = Vector3.zero;
			if(targetedMate == this.gameObject)
			{
				towardMate = Vector3.zero;
			}
			else
			{
				towardMate = targetedMate.transform.position - this.transform.position;
				towardMate.Normalize ();
			}
			Vector3 outwardMate = this.transform.position - toCloseMate.transform.position;
			outwardMate.Normalize ();

			Vector3 finalVect = towardMate + outwardMate;
			finalVect.Normalize();

			move(finalVect);
		}

		if(toCloseMate != this.gameObject)
		{
			Debug.Log("ttt");
		}

	}
	*/
	#endregion
}
