using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class loads the main scene assets including the headstones and props.
/// </summary>
public class LoadMainScene : MonoBehaviour
{
    /// <summary>
    /// Used to control the single loading of the scene in the update.
    /// </summary>
    public bool loadingScene = true;
    /// <summary>
    /// Holds references to the hidden start up items (i.e. controllers and UI).
    /// </summary>
    public GameObject[] StartItems;

    /// <summary>
    /// Start the loading of the main assets.
    /// </summary>
    void Update()
    {
        if (loadingScene == true)
        {
            loadingScene = false;
            // kick off the load
            StartCoroutine(LoadPrototype());
        }
    }
    /// <summary>
    /// Asynchronously loads the main assets scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadPrototype()
    {
        // start the asynchronous load of the main assets
        //var progress = SceneManager.LoadSceneAsync("SmallCemAssets", LoadSceneMode.Additive);
        var progress = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // wait until the scene fully loads
        while (!progress.isDone)
        {
            yield return null;
        }

        StartCoroutine(WaitSome());
    }

    private IEnumerator WaitSome()
    {
        yield return new WaitForSecondsRealtime(10);

        // turn the blocking sphere off
        GameObject.Find("LoadSphere").SetActive(false);
        // turn the UI and controllers on
        foreach (GameObject sItem in StartItems)
        {
            sItem.SetActive(true);
        }
    }
}