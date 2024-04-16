using UnityEngine;

/// <summary>
/// This class controls the hands physics.
/// </summary>
public class HandPhysics : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the target hand.
    /// </summary>
    public Transform target;
    /// <summary>
    /// Holds a reference to the rigidbody of the hand.
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Holds a reference to all the hand colliders.
    /// </summary>
    private Collider[] handColliders;

    /// <summary>
    /// Sets the rotation of the rigidbody to the controller position.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // set it to the controller position
        rb.position = target.position;
        rb.rotation = target.rotation;
        // get the colliders
        handColliders = GetComponentsInChildren<Collider>();
    }

    /// <summary>
    /// Enable the hand colliders.
    /// </summary>
    public void EnableHandColliders()
    {
        foreach (var item in handColliders)
        {
            item.enabled = true;
        }
    }

    /// <summary>
    /// Enable the hand colliders.
    /// </summary>
    public void DisableHandColliders()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
        }
    }


    /// <summary>
    /// Adjusts the position and rotation of the hand.
    /// </summary>
    void FixedUpdate()
    {
        // position
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        // rotation
        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);

        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
