using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the processing for the query UI tabs.
/// </summary>
public class QueryTabs : MonoBehaviour
{
    /// <summary>
    /// Holds the reference to the DatabaseControl script
    /// </summary>
    DatabaseControl databaseControl;

    /// <summary>
    /// Holds the reference to the basic tab game object.
    /// </summary>
    public GameObject tabButton1;
    /// <summary>
    /// Holds the reference to the advanced tab game object.
    /// </summary>
    public GameObject tabButton2;
    /// <summary>
    /// Holds the reference to the settings tab game object.
    /// </summary>
    public GameObject tabButton3;
    /// <summary>
    /// Holds the reference to the content of the basic tab.
    /// </summary>
    public GameObject tabContent1;
    /// <summary>
    /// Holds the reference to the content of the advanced tab.
    /// </summary>
    public GameObject tabContent2;
    /// <summary>
    /// Holds the reference to the content of the settings tab.
    /// </summary>
    public GameObject tabContent3;
    /// <summary>
    /// Hold the reference to which of the tabs is currently active.
    /// </summary>
    public int activeTab;

    private void Awake()
    {
        activeTab = 3;
    }
    /// <summary>
    /// Hides all of the tab contents and changes the tab colors to non-selected values.
    /// </summary>
    public void HideAllTabs()
    {
        tabContent1.SetActive(false);
        tabContent2.SetActive(false);
        tabContent3.SetActive(false);
        tabButton1.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabButton2.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabButton3.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        activeTab = 0;
    }

    /// <summary>
    /// Displays the basic query tab.
    /// </summary>
    public void ShowTab1()
    {
        HideAllTabs();
        tabContent1.SetActive(true);
        tabButton1.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);
        activeTab = 1;
        // enable the all button for basic tab
        databaseControl.allHSB.enabled = true;
    }

    /// <summary>
    /// Displays the advanced query tab.
    /// </summary>
    public void ShowTab2()
    {
        HideAllTabs();
        tabContent2.SetActive(true);
        tabButton2.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);
        activeTab = 2;
        // enable the all button for Advanced tab
        databaseControl.allHSA.enabled = true;
    }

    /// <summary>
    /// Displays the settings tab.
    /// </summary>
    public void ShowTab3()
    {
        HideAllTabs();
        tabContent3.SetActive(true);
        tabButton3.GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);
        activeTab = 3;
    }
}
