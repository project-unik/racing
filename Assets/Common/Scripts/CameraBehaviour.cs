/*!
 * @file Assets/Behaviours/CameraBehaviour.cs
 * @created 2016/02/28
 * @lastmodified 2016/03/12
 * @brief Implements the camera behaviour following the player.
 *   Must be placed on the target camera object.
 *
 * Todo
 * ====
 *
 * - Make camera follow the Z-direction of the target object
 */

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour {
	#region camera settings
	/// <summary>
	/// Initial distance from <see cref="targetObject"/>. This is automatically initialized in <see cref="Start"/>.
	/// </summary>
	[ReadOnly]
	[SerializeField]
	private float cameraDistance;

	/// <summary>
	/// Initial vertical offset from <see cref="targetObject"/>. This is automatically initialized in <see cref="Start"/>.
	/// </summary>
	[ReadOnly]
	[SerializeField]
	private float cameraVerticalOffset;

	[SerializeField]
	[Range(0, 5)]
	private float maxDownRotation = 1.0f;

	[SerializeField]
	[Range(0, 5)]
	private float maxUpRotation = 1.0f;

	[SerializeField]
	private bool honourUpDownRotation;
	#endregion

	#region target object components
	/// <summary>
	/// The target's <see cref="Rigidbody"/> component.
	/// </summary>
	[ReadOnly]
	[SerializeField]
	private Rigidbody targetRigidbody;

	/// <summary>
	/// The target <see cref="GameObject"/> that the camera should follow. Must have a <see cref="Rigidbody"/> component attached.
	/// </summary>
	[SerializeField]
	private GameObject targetObject;
	#endregion

	/// <summary>
	/// <see cref="Transform"/> on the race track which is used for determining the camera's position.
	/// </summary>
	public Transform trackObject;

	void Start() {
		Assert.IsNotNull(targetObject, "camera is missing a target object");
		Assert.IsNotNull(targetRigidbody = targetObject.GetComponent<Rigidbody>(), "target object is missing a rigidbody component");

		Vector3 diff = transform.position - targetRigidbody.position;
		cameraDistance = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
		cameraVerticalOffset = diff.y;
	}

	void Update() {
		Vector3 xzOffset = -targetRigidbody.transform.forward * cameraDistance;
		if (!honourUpDownRotation) {
			xzOffset.y = 0;
		}

		Vector3 yOffset = targetRigidbody.transform.up * cameraVerticalOffset;

		transform.position = targetRigidbody.position + xzOffset + yOffset;
		transform.LookAt(targetObject.transform);
	}
}
