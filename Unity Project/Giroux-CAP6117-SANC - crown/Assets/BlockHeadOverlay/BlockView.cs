using UnityEngine;

public class BlockView : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the layer that holds the objects to block.
    /// </summary>
    public LayerMask collisionLayer;
    private Material cameraFadeMat;
    private Color cameraFadeColor;
    private float sphereCheckSize = 0.15f;

    /// <summary>
    /// Retrieves the references for the material, color, and alpha.
    /// </summary>
    private void Awake()
    {
        cameraFadeMat = GetComponent<Renderer>().material;
        cameraFadeColor = cameraFadeMat.color;
    }

    /// <summary>
    /// Checks for the collision of the head sphere and the object
    /// </summary>
    void Update()
    {
        // check for the collision on the specific blockable layer
        if (Physics.CheckSphere(transform.position, sphereCheckSize, collisionLayer, QueryTriggerInteraction.Ignore))
        {
            // when collided, turn it black
            CameraFade(255f);
        }
        else
        {
            // when removed, turn it transparent
            CameraFade(0f);
        }
    }

    /// <summary>
    /// Fade the view to black.
    /// </summary>"
    /// <param name="targetAlpha">The level of alpha to set.</param>
    public void CameraFade(float targetAlpha)
    {
        // set the alpha to the target value.
        cameraFadeColor.a = targetAlpha;
        // update the material's color
        cameraFadeMat.color = cameraFadeColor;
    }
}
