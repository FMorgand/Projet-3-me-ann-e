/*  : INFORMATION :
 * 
 * Version : 1.04
 * Interval.cs is a custom class representing
 * an interval between two floats.
 * 
 * The Interval class contains some functions : 
 * 	Average & AverageToInt
 * 	CenterOn
 * 	Random & RandomInt
 * 	maxInt & minInt
 * 	CutUnder, CutAbove & Clamp
 * 	Length & LengthToInt
 * 	Contains
 */

using UnityEngine;
//using System.Collections; 
using UnityEditor;

#region Class Interval
[System.Serializable] // Making this class visible in inspector
public class Interval 
{
	// General constructor
	public Interval (float float1 = 0, float float2 = 0)
	{ 
		Set (float1, float2); 
	}
	private void Set (float float1, float float2) 
	{
		min = Mathf.Min(float1, float2);
		max = Mathf.Max (float1, float2);
	}

	#region Secondary Constructors
	// These constructors are here if the user wants to use doubles or integers.

	public Interval (double double1, float float2) 
	{ Set ((float)double1, float2); }
	public Interval (float float1, double double2) 
	{ Set (float1, (float) double2); } 
	public Interval (double double1, double double2) 
	{ Set ((float) double1, (float) double2); }
	#endregion

	#region Public Functions and Attributes
	
	public float min;
	public float max;
	
	/// <summary> Returns the middle value of the interval. </summary>
	public float Average () { return ((min + max) / 2); }
	/// <summary> Returns the middle value of the interval, rounded. </summary>
	public int AverageToInt () { return Mathf.RoundToInt((min + max) / 2); }

	/// <summary> Centers the interval on a float but keep the same length. </summary>
	public Interval CenterOn (float newCenter) { return new Interval (newCenter - (Length() / 2), newCenter + (Length() / 2)); }

	/// <summary> Returns a random float in the interval. </summary>
	public float Random () { return UnityEngine.Random.Range(min, max); }
	/// <summary> Returns a random int in the interval. </summary>
	public int RandomInt () { return UnityEngine.Random.Range(maxInt(), minInt()); }

	/// <summary> Returns the highest int in the interval. </summary>
	public int maxInt () { return Mathf.FloorToInt(max); }
	/// <summary> Returns the lowest int in the interval. </summary>
	public int minInt () { return Mathf.CeilToInt(min); }
	
	/// <summary> Cut the part of the interval which is lower than specified float. </summary>
	public Interval CutUnder (float newMin) { return new Interval (Mathf.Max(min, newMin), max); }
	/// <summary> Cut the part of the interval which is higher than specified float. </summary>
	public Interval CutAbove (float newMax) { return new Interval (min, Mathf.Min(max, newMax)); }
	/// <summary> Cut the parts of the interval which are lower or higher than specified floats. </summary>
	public Interval Clamp (float lowCut, float highCut) { return new Interval (Mathf.Max(min, lowCut), Mathf.Min(max, highCut));}
	/// <summary> Cut the parts of the base interval which are not included in the specified interval. </summary>
	public Interval Clamp (Interval interval) { return new Interval (Mathf.Max(min, interval.min), Mathf.Min(max, interval.max)); }

	/// <summary> Returns the length of the interval. </summary>
	public float Length () { return Mathf.Abs(max - min); }
	/// <summary> Returns the length of the interval, floored. </summary>
	public int LengthToInt () { return Mathf.FloorToInt(Mathf.Abs(max - min)); }

	/// <summary> Returns true if the value is in the interval. </summary>
	public bool Contains (float value) { return value >= min && value <= max; }
	public bool Contains (int value) { return value >= min && value <= max; }
	public bool Contains (double value) { return (float)value >= min && (float)value <= max; }

	#endregion
}
#endregion

#region Property Drawer
[CustomPropertyDrawer(typeof(Interval))]
public class CustomRangeInspector : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID(FocusType.Passive), label);
		
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		// Rect for min value, "to", and max value
		Rect minRect = new Rect(position.x, position.y, (position.width - 30) / 2, position.height);
		Rect toRect = new Rect(position.x + position.width / 2 - 10, position.y, 20, position.height); 
		Rect maxRect = new Rect(position.x + position.width / 2 + 10, position.y, (position.width - 30) / 2, position.height);
		
		// Displaying
		EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
		EditorGUI.LabelField(toRect, "to");
		EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);
		
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty();
	}
}
#endregion