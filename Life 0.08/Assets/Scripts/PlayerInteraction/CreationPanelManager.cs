using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreationPanelManager : MonoBehaviour {

	public InputField _name;
	public Slider _speed;
	public Text _speedValueText;
	public Slider _rangeOfView;
	public Text _rangeOfViewValueText;
	public Slider _lifeTime;
	public Text _lifeTimeValueText;
	public Slider _maxEnergy;
	public Text _maxEnergyValueText;
	public Slider _reproductionTime;
	public Text _reproductionTimeValueText;

	public InputField _predator;
	public InputField _prey;

	public Button _createNewSpecies;

	private EntityManager _manager;

	// Use this for initialization
	void Start () {
		_manager = GameObject.Find ("EntityManager").GetComponent<EntityManager>();
	}
	
	// Update is called once per frame
	void Update () {
		_speedValueText.text = _speed.value.ToString ("00");
		_rangeOfViewValueText.text =  _rangeOfView.value.ToString ("00");
		_lifeTimeValueText.text = _lifeTime.value.ToString ("00");
		_maxEnergyValueText.text = _maxEnergy.value.ToString ("00");
		_reproductionTimeValueText.text = _reproductionTime.value.ToString ("00");

	}

	public void CreateParameter()
	{
		Entity[] predators = new Entity[1];
		predators.SetValue (_manager._speciesPrefabs[_manager._speciesName.IndexOf(_predator.text)].GetComponent<Entity>(),0);

		GameObjectWithRate[] preys = new GameObjectWithRate[1];
		GameObjectWithRate newPrey = new GameObjectWithRate();
		newPrey.prefab = (_manager._speciesPrefabs[_manager._speciesName.IndexOf(_prey.text)]);
		newPrey.rate = 10;
		preys.SetValue (newPrey, 0);

		string newName = _name.text;

		_manager.CreateNewRace (newName ,_speed.value, _rangeOfView.value, _lifeTime.value, _maxEnergy.value, _reproductionTime.value, predators, preys);
	}
}
