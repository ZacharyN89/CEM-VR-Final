using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class processes the personal headstone toggle value.
/// </summary>
public class PersonalMarkers : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the personal headstones toggle field.
    /// </summary>
    Toggle theToggle;
    /// <summary>
    /// Holds a reference to the label for the toggle.
    /// </summary>
    public TMP_Text labelText;

    /// <summary>
    /// Holds a reference to the personal headstones parent gameobject.
    /// </summary>
    public GameObject pHeadstones;
    /// <summary>
    /// Holds a reference to the military headstones parent gameobject.
    /// </summary>
    public GameObject mHeadstones;


    /// <summary>
    /// Sets up the biography board toggle for closing each or leaving them open.
    /// </summary>
    void Start()
    {
        // get reference to personal headstone group
        pHeadstones = FindInActiveObjectByTag("PH");

        // get reference to military headstone group
        mHeadstones = FindInActiveObjectByTag("MH");

        //Fetch the Toggle GameObject
        theToggle = GetComponent<Toggle>();
        // default it to on
        theToggle.isOn = false;
        //Add listener for when the state of the Toggle changes, to take action
        theToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(theToggle);
        });
    }

    /// <summary>
    /// Outputs the new state of the Toggle into the label text.
    /// </summary>
    /// <param name="change">The state of change.</param>
    void ToggleValueChanged(Toggle change)
    {
        labelText.text = "Display Personal Models: ";

        if (theToggle.isOn)
        {
            labelText.text += "ON";
        }
        else
        {
            labelText.text += "OFF";
        }
        // toggle the parent to show/hide
        pHeadstones.SetActive(theToggle.isOn);
        mHeadstones.SetActive(!theToggle.isOn);
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
