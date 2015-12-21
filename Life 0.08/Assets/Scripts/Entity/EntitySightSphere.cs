using UnityEngine;
using System.Collections;

public class EntitySightSphere : MonoBehaviour 
{
	Entity entity;

	void Awake () 
	{
		entity = GetComponentInParent<Entity> ();
		transform.localScale = new Vector3 (entity.rangeOfView, entity.rangeOfView, entity.rangeOfView);
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Entity" && other != transform.parent.GetComponent<Collider>()) {
			entity.encounterEntity(other.gameObject.GetComponent<Entity>());
		}
	}
}
