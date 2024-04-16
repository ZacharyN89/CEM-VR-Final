using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class controls the data entry for input fields.
/// </summary>
public class KeyboardProcess : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the currently active input field.
    /// </summary>
    public TMP_InputField inputField;
    /// <summary>
    /// Holds a reference to the QueryUIProcess script.
    /// </summary>
    public QueryUIProcess queryUIProcess;
    // list of buttons to disable 
    private GameObject[] buttonList;

    /// <summary>
    /// Insert the current character into the input field.
    /// </summary>
    /// <param name="c"></param>
    public void InsertChar (string c)
    {
        // need to test for max length here
        if (inputField.text.Length < inputField.characterLimit)
        {
            inputField.text += c;
        }
    }

    /// <summary>
    /// Delete a character from the input field.
    /// </summary>
    public void DeleteChar()
    {
        // make sure there are characters to delete
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    /// <summary>
    /// Deactivate the keyboard on Done.
    /// </summary>
    public void DoneClicked()
    {
        // the current game object is the keyboard
        gameObject.SetActive(false);

        // turn things back on 
        buttonList = GameObject.FindGameObjectsWithTag("AdvancedItems");
        foreach (GameObject butt in buttonList)
        {
            butt.GetComponent<Button>().enabled = true;
        }
        // recheck the validity to set the submit
        queryUIProcess.CheckValidity();
        // re-enable the editable fields
        queryUIProcess.SetAdvFields(inputField, true);


    }

    /// <summary>
    /// Disable other fields when in an input field.
    /// </summary>
    public void DisableOtherFields()
    {
        buttonList = GameObject.FindGameObjectsWithTag("AdvancedItems");
        foreach (GameObject butt in buttonList)
        {
            butt.GetComponent<Button>().enabled = false;
        }
        // disable the editable fields except the one we're on
        queryUIProcess.SetAdvFields(inputField, false);
    }
}
