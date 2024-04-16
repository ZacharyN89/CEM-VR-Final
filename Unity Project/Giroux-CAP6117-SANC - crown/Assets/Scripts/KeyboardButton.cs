using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class processes a keyboard button press.
/// </summary>
public class KeyboardButton : MonoBehaviour
{
    /// <summary>
    /// Holds reference to the keyboard process script.
    /// </summary>
    public KeyboardProcess keyboard;
    /// <summary>
    /// Holds a reference to the current button.
    /// </summary>
    public Button buttonHit;


    /// <summary>
    /// Process the key press.
    /// </summary>
    public void HitKey()
    {
        //debugamy
        Debug.Log("Button " + buttonHit.name);

        // letter or number keys are length 1
        if (buttonHit.name.Length == 1)
        {
            //debugamy
            Debug.Log("Keylength 1");

            keyboard.InsertChar(buttonHit.name);
        }
        else if (buttonHit.name == "Delete")
        {
            keyboard.DeleteChar();
        } 
        else if (buttonHit.name == "Apostrophe")
        {
            keyboard.InsertChar("'");
        }
        else
        {
            // only button left is Done
            keyboard.DoneClicked();
        }
    }
}
