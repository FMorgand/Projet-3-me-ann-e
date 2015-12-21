using UnityEngine;
using System.Collections; 
using UnityEditor;

#region Class
[System.Serializable]
public class GameObjectWithRate
{
	public GameObject prefab;
	public int rate;
}
#endregion

#region Property Drawer
[CustomPropertyDrawer(typeof(GameObjectWithRate))]
public class GameObjectWithIntegerDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		Rect contentPosition = position;

		contentPosition.width -= 50f;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("prefab"), new GUIContent("Object & Rate"));
		contentPosition.x += contentPosition.width;
		contentPosition.width = 50f;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("rate"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}
#endregion