using UnityEngine;

/// <summary>
/// This class is used to adjust the height of the cemetery signage
/// so that it is accessible to seated and standing users of varying height.
/// </summary>
public class SetSignHeight : MonoBehaviour
{
    private Vector3 setHeight;
    /// <summary>
    /// Holds the height of the camera.
    /// </summary>
    public static float camHeight;
    private Camera theCamera;
    private bool oneTime = true;

    /// <summary>
    /// Gets a reference to the main camera.
    /// </summary>
    private void Start()
    {
        theCamera = Camera.main;
    }

    /// <summary>
    /// Adjusts the height for the cemetery signage based on the camera height.
    /// </summary>
    /// <remarks>
    /// The height is adjusted once in the scene based on the initial value of the camera height.
    /// This allows seated users to access all of the signage.
    /// </remarks>
    void Update()
    {
        // only adjust the sign height once
        if (oneTime)
        {
            if (theCamera.transform.position.y != 0)
            {

                // get the camera's height
                camHeight = theCamera.transform.position.y;
                // if the user is standing, move the sign height down 1/2 meter
                // otherwise, put the sign at seated height
                if (camHeight >= 1.2f)
                {
                    camHeight -= 0.6f;
                }
                else
                {
                    camHeight = 0.75f;
                }
                setHeight = new Vector3(gameObject.transform.position.x, camHeight, gameObject.transform.position.z);
                gameObject.transform.position = setHeight;
                oneTime = false;
            }
        }
    }
}
