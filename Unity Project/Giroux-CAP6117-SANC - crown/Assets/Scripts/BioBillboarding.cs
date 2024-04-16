using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the billboarding effect for biography boards.
/// </summary>
public class BioBillboarding : MonoBehaviour
{
    private Camera theCamera;
    public HSdata hsData;

    /// <summary>
    /// Set the reference to the main camera.
    /// </summary>
    void Start()
    {
        theCamera = Camera.main;
    }

    /// <summary>
    /// Make the object look at the camera.
    /// </summary>
    void LateUpdate()
    {
        // check the settings flag for whether to rotate
        if (!hsData.bioStationary)
        {
            // point the bio board at the camera
            transform.LookAt(theCamera.transform);

            //transform.rotation = theCamera.transform.rotation;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
