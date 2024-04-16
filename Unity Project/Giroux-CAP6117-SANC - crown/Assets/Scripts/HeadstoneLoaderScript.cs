using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeadstoneLoaderScript : MonoBehaviour
{
    private Texture2D defaultTex; 
    private Texture2D fallback; //Deprecated
    private Queue<HSdata> headstoneQueue;
    private bool isProcessing = false;

    private void Awake()
    {
        StartCoroutine(getDefault());
        headstoneQueue = new Queue<HSdata>();
    }

    public void OnTriggerEnter(Collider other)
    {
        HSdata hsd = other.GetComponentInParent<HSdata>();
        if (hsd != null)
        {
            hsd.inRange = true;
            headstoneQueue.Enqueue(hsd);
            if (!isProcessing)
            {
                StartCoroutine(processQueue());
            }
        }
    }

    private IEnumerator processQueue()
    {
        isProcessing = true;

        while (headstoneQueue.Count > 0)
        {
            HSdata next = headstoneQueue.Dequeue();
            if (next.inRange)
            {
                yield return StartCoroutine(next.GetHSTexture());
            }
        }

        isProcessing = false;
    }

    public void OnTriggerExit(Collider other)
    {
        HSdata hsd = other.GetComponentInParent<HSdata>();
        if (hsd != null)
        {
            hsd.inRange = false;
            hsd.SetTexture(defaultTex);
        }
    }

    private IEnumerator getDefault()
    {
        string _strFaceFileName;
         //This is the name you want the "default" headstone texture to be.
         //Should be set to the same thing as whatever you made the default texture of the
         //headstone material be.
        string tFileName = "plainHeadstone";

        #if UNITY_EDITOR
        _strFaceFileName = "file://" + Application.dataPath + "/StreamingAssets/hsMaterials/" + tFileName + ".png";
#elif UNITY_ANDROID
        _strFaceFileName = "jar:file://" + Application.dataPath + "!/assets/hsMaterials/" + tFileName + ".png";
#endif
        // get the new texture via a web file request         
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_strFaceFileName))
        {
            // wait for the request to complete
            yield return uwr.SendWebRequest();

            // check for read errors
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("HS file: " + gameObject.name.Trim());
                Debug.Log(uwr.error);
            }
            else
            {
                // read in the file for the texture
                defaultTex = DownloadHandlerTexture.GetContent(uwr);
            }
        }
    }
}
