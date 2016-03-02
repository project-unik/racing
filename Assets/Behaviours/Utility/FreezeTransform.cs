/*!
 * @file Assets/Behaviours/Utility/FreezeTransform.cs
 * @created 2016/02/28
 * @lastmodified 2016/02/28
 * @brief Freeze the transformation of a #GameObject.
 */
 
using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
public class FreezeTransform : MonoBehaviour {

  public bool freezePosition = true;
  public bool freezeRotation = true;
  public bool freezeScale = true;

  private long lastUpdate = 0;
  private Vector3 lastPosition;
  private Quaternion lastRotation;
  private Vector3 lastScale;
  	
	void FixedUpdate () {
    // todo
	}

  /*!
   * Return the time elapsed since the Unix epoch.
   */
  static int GetTime() {
    TimeSpan tspan = DateTime.UtcNow - new DateTime(1970, 1, 1);
    return (int) tspan.TotalSeconds;
  }

}
