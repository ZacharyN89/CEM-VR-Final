using UnityEngine;
using TMPro;

/// <summary>
/// This class processes the input fields on the advanced search screen.
/// </summary>
public class InputFieldProcess : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the KeyboardProcess script.
    /// </summary>
    public KeyboardProcess keyboardProcess;

    /// <summary>
    /// Holds a reference to the keyboard game object.
    /// </summary>
    public GameObject keyboard;
    /// <summary>
    /// Holds a reference to the current input field.
    /// </summary>
    public TMP_InputField inField;
    /// <summary>
    /// Holds a reference to the adjacent field name dropdown.
    /// </summary>
    public TMP_Dropdown dd;

    /// <summary>
    /// On entering the input field, activate the keyboard.
    /// </summary>
    public void OnEnter()
    {
        // activate the keyboard
        string whereAmI = dd.options[dd.value].text.ToLower();
        whereAmI = whereAmI.Substring(whereAmI.Length - 4);

        bool ltr = false;
        bool num = false;

        if (whereAmI == "name")
        {
            ltr = true;
            inField.characterLimit = 30;
        }
        else
        {
            // on a date field
            num = true;
            inField.characterLimit = 4;
        }

        // turn the proper sets of keys on/off
        keyboard.transform.Find("Numbers").gameObject.SetActive(num);
        keyboard.transform.Find("Letters").gameObject.SetActive(ltr);
        keyboard.transform.Find("Apostrophe").gameObject.SetActive(!num);

        // disable all the other fields on the screen while the keyboard is open
        keyboardProcess.DisableOtherFields();

        // turn the keyboard on
        keyboard.SetActive(true);
    }
}
