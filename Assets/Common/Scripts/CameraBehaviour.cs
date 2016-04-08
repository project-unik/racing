/*!
 * @file Assets/Behaviours/CameraBehaviour.cs
 * @created 2016/02/28
 * @lastmodified 2016/03/30
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

/// <summary>
/// Based on https://unity3d.com/learn/tutorials/projects/stealth/camera-movement
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour
{
    #region camera settings
    /// <summary>
    /// speed at which the camera will catch up to the followed object
    /// </summary>
    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float smooth = 20f;

    /// <summary>
    /// absolute speed at which the target will be considered as moving backwards
    /// </summary>
    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float backwardsThreshold = 10f;

    /// <summary>
    /// whether or not the camera should take the target object's up/down rotation into account when positioning
    /// </summary>
    [SerializeField]
    private bool honourUpDownRotation = true;

    /// <summary>
    /// position which the camera is trying to reach
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private Vector3 newPosition;

    /// <summary>
    /// horizontal distance of the camera from the followed object
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private float horizontalDistance;

    /// <summary>
    /// vertical distance of the camera from the followed object
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private float verticalDistance;
    #endregion

    #region targets
    /// <summary>
    /// reference to the target's transform
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private Transform target;

    /// <summary>
    /// reference to the target's rigidbody
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private Rigidbody targetRigidbody;

    /// <summary>
    /// <see cref="GameObject"/> which is tracked by this camera. New targets need to have a <see cref="Rigidbody"/> component attached.
    /// If <code>null</code> is provided as the new value, the first <see cref="GameObject"/> tagged with <see cref="Tags.GameObjects.PLAYER"/>
    /// will be used instead.
    /// </summary>
    public void setTrackedObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            gameObject = GameObject.FindGameObjectWithTag(Tags.GameObjects.PLAYER);
        }
        Assert.IsNotNull(value, "camera is missing a target");
        Assert.IsNotNull(target = value.transform, "target is missing a Transform component");
        Assert.IsNotNull(targetRigidbody = value.GetComponent<Rigidbody>(), "target is missing a Rigidbody component");

        // set relative position
        Vector3 rel = transform.position - target.position;
        horizontalDistance = Mathf.Sqrt(rel.x * rel.x + rel.z * rel.z);
        verticalDistance = rel.y;

        //temp fix
        verticalDistance += 2;
        horizontalDistance += 5;
    }
    #endregion

    #region Unity messages

    #region Update messages
    private void FixedUpdate()
    {
        // check if the target is moving forward
        bool movingForward = targetRigidbody.IsMovingForward(backwardsThreshold);

        // horizontal offset from the target's position
        Vector3 horizontalOffset = target.forward * horizontalDistance;
        if (movingForward)
        {
            horizontalOffset.x *= -1f;
            horizontalOffset.z *= -1f;
        }
        if (!honourUpDownRotation)
        {
            horizontalOffset.y = 0;
        }

        // vertical offset from the target's position
        Vector3 verticalOffset;
        if (honourUpDownRotation)
        {
            verticalOffset = target.up * verticalDistance;
        }
        else
        {
            verticalOffset = Vector3.up * verticalDistance;
        }

        // relative position of the camera from the target object
        Vector3 standard = target.position + horizontalOffset + verticalOffset;

        // position directly above the target object at the same distance as standard
        Vector3 above = target.position + Vector3.up * horizontalDistance;

        // points that are used to check if the camera can see the target object
        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standard;
        checkPoints[1] = standard.EaseInOut(above, 0.25f);
        checkPoints[2] = standard.EaseInOut(above, 0.5f);
        checkPoints[3] = standard.EaseInOut(above, 0.75f);
        checkPoints[4] = above;

        // check from which position the camera can see the target object
        foreach (Vector3 point in checkPoints)
        {
            if (CanSeeTargetFrom(point))
            {
                newPosition = point;
                break;
            }
        }

        // smooth camera position between its current position and the new position
        transform.position = transform.position.EaseInOut(newPosition, smooth * Time.deltaTime);

        // look at the target
        LookAtTarget();
    }
    #endregion
    #endregion

    private bool CanSeeTargetFrom(Vector3 position)
    {
        RaycastHit hit;

        if (Physics.Raycast(position, target.position - position, out hit, horizontalDistance))
        {
            if (hit.transform != target) return false;
        }

        return true;
    }

    private void LookAtTarget()
    {
        Vector3 relativePosition = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smooth * Time.deltaTime);
    }
}
