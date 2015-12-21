/*  : INFORMATION :
 * 
 * Version : 1.05
 * DrawCollider.cs is a component wich draws
 * the colliders in the scene, even if the object
 * isn't selected. 
 * 
 * Working with Box, Sphere and Capsule colliders.
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class DrawCollider : MonoBehaviour 
{
	// Draw full volumes or wires or both 
	[SerializeField] private bool solid = true, wires = false;
	// Color when collider is active (default = green)
	[SerializeField] private Color colorActive = new Color(0, 1, 0, 0.2f);
	// Toggle inactive collider drawing
	[SerializeField] private bool drawInactive = true;
	// Color when collider is inactive (default = red)
	[SerializeField] private Color colorInactive = new Color(1, 0, 0, 0.2f);

	private void OnDrawGizmos()
	{
		var oldMatrix = Gizmos.matrix;

		Collider collider = GetComponent<Collider> ();

		if (collider.enabled)
			Gizmos.color = colorActive;
		else
			Gizmos.color = colorInactive;

		if (collider.enabled || drawInactive) 
		{
			if (collider is BoxCollider)
				DrawCube((BoxCollider)collider);
			else if (collider is SphereCollider)
				DrawSphere((SphereCollider)collider);
			else if (collider is CapsuleCollider)
				DrawCapsule((CapsuleCollider)collider);
		}

		Gizmos.matrix = oldMatrix;
	}

	private void DrawCube(BoxCollider collider)
	{
		Gizmos.matrix = Matrix4x4.TRS (
			transform.TransformPoint(collider.center), 
			transform.rotation,
			transform.localScale);

		if (solid)
			Gizmos.DrawCube (Vector3.zero, collider.size);
		if (wires)
			Gizmos.DrawWireCube(Vector3.zero, collider.size);
	}

	private void DrawSphere(SphereCollider collider)
	{
		Gizmos.matrix = Matrix4x4.TRS (
			transform.TransformPoint(collider.center), 
			transform.rotation,
			transform.localScale);

		if (solid)
			Gizmos.DrawSphere (Vector3.zero, collider.radius);
		if (wires) 
			Gizmos.DrawWireSphere (Vector3.zero, collider.radius);
	}

	private void DrawCapsule(CapsuleCollider collider)
	{
		Gizmos.matrix = Matrix4x4.TRS (
			transform.TransformPoint(collider.center), 
			Quaternion.identity,
			new Vector3(1.0f, 1.0f, 1.0f));

		Vector3 scale = transform.localScale;
		float radius = collider.radius * Mathf.Max(scale.x, scale.z);

		Vector3 up = Vector3.zero;
		switch (collider.direction) {
		case 0: // X axis
			up = (collider.height * scale.y / 2 - radius) * transform.right;
			break;
		case 1: // Y axis, default
			up = (collider.height * scale.y / 2 - radius) * transform.up;
			break;
		case 2 : // Z axis
			up = (collider.height * scale.y / 2 - radius) * transform.forward;
			break;
		}

		Vector3 right = radius * transform.right;
		Vector3 forward = radius * transform.forward;
	
		if (solid) {
			Gizmos.DrawSphere (up, radius);
			Gizmos.DrawSphere (-up, radius);
		}
		if (wires) {
			Gizmos.DrawWireSphere (up, radius);
			Gizmos.DrawWireSphere (-up, radius);
		}
		if (solid || wires) {
			Gizmos.DrawLine (up + right, -up + right);
			Gizmos.DrawLine (up - right, -up - right);
			Gizmos.DrawLine (up + forward, -up + forward);
			Gizmos.DrawLine (up - forward, -up - forward);
		}
	}
}
