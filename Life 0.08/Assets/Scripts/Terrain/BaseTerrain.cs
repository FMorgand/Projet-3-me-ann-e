using UnityEngine;
using System.Collections;

public class BaseTerrain : MonoBehaviour {

	public int _ressourceNeed;
	public float _destructionTreshold;
	public int _accordedRessources;
	protected float _radius;

	// Use this for initialization
	void Start () {
		_radius = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if(_accordedRessources > _ressourceNeed) { IncreaseTerrainRadius(0.01f); }

		if(_accordedRessources < _ressourceNeed) { DecreaseTerrainRadius(0.01f); }

		if(_radius < _destructionTreshold) { DestroyTerrain(); }

		RadiusManager ();
	}

	void RadiusManager()
	{
		this.transform.localScale = new Vector3 (_radius, 1, _radius);
	}

	void IncreaseTerrainRadius(float increaseValue)
	{
		_radius += increaseValue;
	}

	void DecreaseTerrainRadius(float decreaseValue)
	{
		_radius -= decreaseValue;
	}

	void DestroyTerrain()
	{
		Destroy (this.gameObject);
	}
}
