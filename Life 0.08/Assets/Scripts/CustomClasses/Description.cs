using UnityEngine;
using System.Collections; 
using UnityEditor;

#region Class
[System.Serializable]
public class Description
{
	public Description (string description) 
	{ text = description; }

	public string text;
}
#endregion

#region Property Drawer
[CustomPropertyDrawer(typeof(Description))]
public class CustomDrawerForDescription : PropertyDrawer
{
	bool foldout = true;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (foldout) {
			return 5f * 16f + 18f;
		} else {
			return 16f;
		}
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		Rect contentPosition = position;
		contentPosition.height = 16f;

		foldout = EditorGUI.Foldout(contentPosition, foldout, new GUIContent("Description"));

		if (foldout) {
			EditorGUI.indentLevel ++;
			contentPosition.height = 5 * 16f;
			contentPosition.y += 18f;
			EditorStyles.textField.wordWrap = true;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("text"), GUIContent.none);
			EditorGUI.indentLevel --;
		}

		EditorGUI.EndProperty();
	}
}
#endregion