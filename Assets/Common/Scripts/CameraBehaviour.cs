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
	/// Initial distance from <see cref="targetObject"/>.
	/// </summary>
	private float cameraDistance = 10;

	/// <summary>
	/// Initial vertical offset from <see cref="targetObject"/>.
	/// </summary>
	private float cameraVerticalOffset = 2;

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
	/// The target <see cref="GameObject"/> that the camera should follow.
	/// </summary>
	private GameObject targetObject = null;
	#endregion

	/// <summary>
	/// <see cref="Transform"/> on the race track which is used for determining the camera's position.
	/// </summary>
	public Transform trackObject;

	void Update() {
        if(targetObject==null)
        {
            return;
        }

        Vector3 xzOffset = -targetObject.transform.forward * cameraDistance;
		if (!honourUpDownRotation) {
			xzOffset.y = 0;
		}

        Vector3 yOffset = targetObject.transform.up * cameraVerticalOffset;

        transform.position = targetObject.transform.position + xzOffset + yOffset;
		transform.LookAt(targetObject.transform);
	}

    public void SetTargetObject(GameObject targetObject)
    {
        this.targetObject = targetObject;
    }
}
