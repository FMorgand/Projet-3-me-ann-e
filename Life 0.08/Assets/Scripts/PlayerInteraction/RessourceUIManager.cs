using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RessourceUIManager : MonoBehaviour {

	public Text _typeARessource;
	public Text _terrainRessource;
	public Text _speciesRessource;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_typeARessource.text = EntityManager._ressources.ToString();
	}
}
