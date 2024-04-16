using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class controls the display of the biography board when the button is pressed.
/// </summary>
public class HSbutton : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the HSdata script for bio board processing.
    /// </summary>
    public HSdata hsData;

    /// <summary>
    /// Holds a reference to the DatabaseControl script.
    /// </summary>
    public DatabaseControl databaseControl;

    /// <summary>
    /// Holds a reference to the button that was pressed.
    /// </summary>
    public GameObject button;
    /// <summary>
    /// Holds a reference to the current headstone logo button game object.
    /// </summary>
    public GameObject lbGO;
    /// <summary>
    /// Controls the press event for the button.
    /// </summary>
    public UnityEvent onPress;
    /// <summary>
    /// Controls the release event for the button.
    /// </summary>
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;
    public GameObject bioSign;
    bool boardState;
    private Camera theCamera;

    private void Awake()
    {
        // get a reference to the already opened database through the control script
        databaseControl = GameObject.Find("DatabaseControl").GetComponent<DatabaseControl>();
    }
    /// <summary>
    /// Sets up the state of the button and the biography board.
    /// </summary>
    void Start()
    {
        // get the logo button this sign is attached to
        lbGO = this.transform.parent.gameObject;
        // get the hsdata
        hsData = lbGO.transform.parent.gameObject.GetComponent<HSdata>();
        boardState = false;
        bioSign.SetActive(boardState);
        isPressed = false;
        theCamera = Camera.main;
    }

    /// <summary>
    /// The trigger event processing for when the user's hand collides with the button.
    /// </summary>
    /// <param name="other">The hand controller object.</param>
    private void OnTriggerEnter(Collider other)
    {
        // check if the button was not pressed
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.001f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
            if (!hsData.loadedBio)
            {
                //Tell hsdata to make the database call to build its biography,
                //if it hasn't been done yet
                hsData.LoadBio();
            }
        }
    }

    /// <summary>
    /// The trigger event for when the user's hand releases the button.
    /// </summary>
    /// <param name="other">The hand controller object.</param>
    private void OnTriggerExit(Collider other)
    {
        // release the button
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    /// <summary>
    /// Toggles the state of the biography board.
    /// </summary>
    /// <remarks>
    /// Calculates the position of the user in relation to the board and rotates the
    /// board to face the front or back of the headstone.
    /// </remarks>
    public void ToggleBioSign()
    {

        if (bioSign != null)
        {
            // toggle state
            boardState = !boardState;

            if (boardState == true)
            {
                // close previous bios if needed
                databaseControl.CloseBios();

                float tY, bioY, tPY;
                // get the headstone orientation
                tPY = lbGO.transform.parent.transform.rotation.eulerAngles.y;
                // set the new look at orientation
                bioSign.transform.LookAt(theCamera.transform);
                tY = bioSign.transform.rotation.eulerAngles.y;
                if (tPY >= 88f && tPY <= 92f)
                {
                    // oriented with faces North and South
                    if ((tY >= 0f && tY < 90f) || (tY >= 270f && tY < 360f))
                    {
                        // set the bio board to 0
                        bioY = 0f;
                    }
                    else
                    {
                        bioY = 180f;
                    }
                }
                else
                {
                    // oriented with faces East and West (between 358 and 2)
                    if (tY >= 0f && tY < 180f)
                    {
                        bioY = 90f;
                    }
                    else
                    {
                        bioY = 270f;
                    }
                }
                // set initial rotation
                bioSign.transform.rotation = Quaternion.Euler(0f, bioY, 0f);
                // set the height
                bioSign.transform.localPosition = new Vector3(0f, SetSignHeight.camHeight, 0f);

                // put this bio on the stack
                databaseControl.bioCloseStack.Add(lbGO.transform.parent.transform.name);
            }

            
            // toggle sign
            bioSign.SetActive(boardState);
        }
        else
        {
            Debug.Log("Sign not found");
        }
    }
}
