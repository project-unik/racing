/*!
 * @file Assets/Behaviours/CameraBehaviour.cs
 * @created 2016/02/28
 * @lastmodified 2016/02/28
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

public class CameraBehaviour : MonoBehaviour {
  
  /*!
   * The target #GameObject that the camera will follow. Must
   * have a #Rigidbody component.
   */
  public GameObject targetObject;

  /*!
   * The initial camera offset. This is autoamtically initialized in #Start().
   */
  public Vector3 cameraOffset;

	void Start () {
    Assert.IsNotNull(targetObject);
	  Assert.IsNotNull(targetObject.GetComponent<Rigidbody>());
    Assert.IsNotNull(this.GetComponent<Camera>());
    cameraOffset = this.transform.position - targetObject.transform.position;
	}
	
	void FixedUpdate () {
	  Rigidbody rb = targetObject.GetComponent<Rigidbody>();
    Camera cam = this.GetComponent<Camera>();
    this.transform.position = rb.position + cameraOffset;
	}
}
