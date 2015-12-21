#region Libraries
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

public class NewEntity : MonoBehaviour 
{
	public float _size, _speed, _sneak, _intel;

	public bool _isHungry;
	public float _baseEnergy;
	public float _currentEnergy;
	
	public float _sightRange;
	
	Vector3 _destination;
	
	public float _baseReproTimer;
	public float _reproTimer;

	public GameObject _prefab;

	public enum Status {
		idle,
		moving, 
		hunting,
		fleeing,
		dead
	}

	public Status _status;

	void Awake () 
	{
		Interval interval = new Interval (0.5f, 2f);
		Interval intervalSize = new Interval (0.5f, 2f);
		_size = _size * intervalSize.Random ();
		_speed = _speed * intervalSize.Random ();
		_sneak = _sneak * interval.Random ();
		_intel = _intel * interval.Random ();
		//_baseEnergy = _baseEnergy * interval.Random ();

		tag = "Entity";
		ActualizeSize ();
		//_currentEnergy = _baseEnergy;
	}

	void ActualizeSize ()
	{
		transform.localScale = new Vector3 (_size, _size, _size);
		_baseEnergy = 20 + _size * 1;
		_currentEnergy = _baseEnergy;
		_speed = 1 / _size;
		_sightRange = 50 + _size / 2;
	}
	

	void FixedUpdate () 
	{
		if (_currentEnergy <= 0f) {
			Destroy (this.gameObject);
		} 
		if (_reproTimer <= 0f) {
			//MakeBaby();
		}
		if (!_isHungry) {
			if (_currentEnergy <= _baseEnergy / 2) {
				_isHungry = true;
			} else {
				_reproTimer -= Time.deltaTime;
			} 
		}
		_currentEnergy -= Time.deltaTime;

		switch (_status) {
		case Status.fleeing:
			Flee(); break;
		case Status.idle:
			Idle (); break;
		case Status.moving:
			Move (); break;
		case Status.hunting:
			Hunt();	break;
		case Status.dead:
			Dead(); break;
		//case Status.loving:
		//	love();	break;
		}
	}

	#region Behaviors
	public void Flee ()
	{
		_status = Status.idle;
	}
	
	public void Move ()
	{	
		if (_isHungry) {
			_status = Status.hunting;
		}

		Vector3 distance = _destination - transform.position;
		if (distance.magnitude < 1) {
			// If i reached my destination, I stop.
			_status = Status.idle;
		} else {
			// Else, I move toward it
			MoveToward(_destination, _speed);
		}
	}
	
	public void Hunt ()
	{
		// Looking for a prey in sight
		List<NewEntity> nearPreys = CheckForPreys ();

		if (nearPreys.Capacity == 0) {
			// If no prey in sight, continue moving randomly
			Move ();
		} else {
			// Prey found ! Chasing.
			NewEntity prey = ChoosePrey(nearPreys);
			Vector3 preyPosition = prey.transform.position;
			Vector3 distance = preyPosition - transform.position;
			if (distance.magnitude < _size + (prey._size) / 2) {
				// If reach prey, eat it
				EatEntity(prey);
			} else {
				// Else, move toward it
				MoveToward (preyPosition, _speed);
			}
		}
	}
	
	public void Idle ()
	{
		if (_isHungry) {
			_status = Status.hunting;
			_destination = new Vector3 (Random.Range (-100f, 100f), 0f, Random.Range(-100f, 100f));
		}

		// Entity have a change to pick a random direction
		if (Random.Range(0, 100/* / Mathf.CeilToInt(config.timeSpeed)*/) == 0) {
			_destination = new Vector3 (Random.Range (-100f, 100f), 0f, Random.Range(-100f, 100f));
			_status = Status.moving;
		}
	}
	
	public void Love () 
	{

	}

	public void Dead ()
	{
		_currentEnergy = 0f;
		//Destroy (this.gameObject);
	}
	#endregion

	public List<NewEntity> CheckAround () 
	{
		Collider[] thingsInSight = Physics.OverlapSphere (this.transform.position, _sightRange);
		
		List<NewEntity> entitiesInSight = new List<NewEntity> ();
		
		foreach (Collider thing in thingsInSight) {
			if (thing != null && thing.tag == "Entity" && thing != GetComponent<Collider>()) {
				entitiesInSight.Add (thing.GetComponent<NewEntity>());
			}
		}
		return entitiesInSight;
	}

	public List<NewEntity> CheckForPreys () 
	{
		List<NewEntity> entities = CheckAround ();
		List<NewEntity> preys = new List<NewEntity>();
		foreach (NewEntity entity in entities) {
			// Size check
			if (entity._size <= _size) {
				preys.Add(entity);
			}
		}
		return preys;
	}

	public NewEntity ChoosePrey (List<NewEntity> entities) 
	{
		NewEntity nearestEntity = entities [0];
		
		foreach (NewEntity prey in entities) {
			if (Vector3.Distance(prey.transform.position, transform.position) < Vector3.Distance (nearestEntity.transform.position, transform.position)) {
				nearestEntity = prey;
			} 
		}
		nearestEntity.ChasedBy (this);
		return nearestEntity;
	}

	public void EatEntity (NewEntity entity) 
	{
		Debug.Log (name + " have eaten " + entity.name);
		_size += entity._size / 2;
		ActualizeSize ();
		entity.Die ();
		_currentEnergy = _baseEnergy;
		_isHungry = false;
		_status = Status.idle;
	}
	
	public void MoveToward (Vector3 target, float speed, bool flee = false) 
	{
		if (flee) transform.Translate (Vector3.Normalize (transform.position - target) * speed);
		else transform.Translate (Vector3.Normalize (target - transform.position) * speed);
	}
	public void MoveToward (NewEntity target, float speed, bool flee = false) 
	{
		if (flee) transform.Translate (Vector3.Normalize (transform.position - target.transform.position) * speed);
		else transform.Translate (Vector3.Normalize (target.transform.position - transform.position) * speed);
	}

	public void Die () 
	{
		_status = Status.dead;
	}

	public void MakeBaby ()
	{
		Instantiate (_prefab, transform.position, Quaternion.identity);
		_reproTimer = _baseReproTimer; 
	}

	public void ChasedBy (NewEntity predator) 
	{
		_status = Status.fleeing;
		MoveToward (predator, _speed, true);
	}
}
