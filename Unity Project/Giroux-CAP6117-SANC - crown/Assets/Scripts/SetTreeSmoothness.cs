using UnityEngine;

/// <summary>
/// This class sets the smoothness on the tree leaves.
/// </summary>
public class SetTreeSmoothness : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the leaf.
    /// </summary>
    public GameObject leafGO;

    /// <summary>
    /// Set the smoothness for the leaves.
    /// </summary>
    void Start()
    {
        leafGO.GetComponent<Renderer>().material.SetFloat("_Glossiness",0f);
    }
}
