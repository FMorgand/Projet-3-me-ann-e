/*  : INFORMATION :
 * 
 * Version : 1.02
 * ScriptableObjectUtility.cs makes easy to
 * create ScriptableObjects assets files.
 * 
 * This class must be placed
 * in the "Assets/Editor/" folder.
 */

using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	/// <summary>
	///	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static ScriptableObject CreateAsset<T> (string name = null, string path = "Assets/ScriptableObjects") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();

		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
			Debug.Log("New folder(s) created : " + path + ".");
		}

		if (name == null) {
			name = typeof(T).ToString();
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();

		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;

		return asset;
	}
}