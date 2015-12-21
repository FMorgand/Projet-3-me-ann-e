/*  : INFORMATION :
 * 
 * Version : 1.00
 * ScriptableObjectCreator.cs is here to create 
 * menu shortcuts that use ScriptableObjectUtility.cs 
 * to create ScriptableObjects assets files.
 * 
 * This class must be placed
 * in the "Assets/Editor/" folder.
 */

using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectCreator {

	[MenuItem("ScriptableObjects/CreateNewConfig")]
	public static void createNewConfigSO ()
	{
		ScriptableObjectUtility.CreateAsset<ConfigSO> ();
	}

	[MenuItem("ScriptableObjects/SaveCurrentConfig")]
	public static void saveCurrentConfig ()
	{
		EntityManager man = GameObject.FindObjectOfType<EntityManager>();
		ConfigSO newSO = (ConfigSO)ScriptableObjectUtility.CreateAsset<ConfigSO> (man.currentConfigName);
		newSO.GetStats(man);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
	}

	[MenuItem("Hidden prOn folder/Don't !/Seriously don't click")]
	public static void prOn ()
	{
		Application.OpenURL("http://bringvictory.com/");
		Debug.Log ("Rickrolled !");
	}

	/* SAMPLE : 
	[MenuItem("ScriptableObjects/SampleSO")]
	public static void createSampleSO ()
	{
		//ScriptableObjectUtility.CreateAsset<SampleSO> ();
	}
	*/
}
