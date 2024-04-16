using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class processes the headstone biography boards based on the closebio toggle value.
/// </summary>
public class CloseBioToggle : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the biographies toggle field.
    /// </summary>
    Toggle theToggle;
    /// <summary>
    /// Holds a reference to the label for the toggle.
    /// </summary>
    public TMP_Text labelText;

    /// <summary>
    /// Holds a reference to the DatabaseControl object for the updating of the headstone settings.
    /// </summary>
    public DatabaseControl databaseControl;

    /// <summary>
    /// Sets up the biography board toggle for closing each or leaving them open.
    /// </summary>
    void Start()
    {
        //Fetch the Toggle GameObject
        theToggle = GetComponent<Toggle>();
        // default it to on
        theToggle.isOn = true;
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
        labelText.text = "Close Previous Biography: ";

        if (theToggle.isOn)
        {
            labelText.text += "ON";
        }
        else
        {
            labelText.text += "OFF";
        }
        databaseControl.closeBioFlag = theToggle.isOn;
    }
}

