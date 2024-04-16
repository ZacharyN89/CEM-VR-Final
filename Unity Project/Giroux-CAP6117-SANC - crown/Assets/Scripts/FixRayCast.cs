using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class adjusts the ray cast to work with the UI.
/// </summary>
public class FixRayCast : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = transform.GetComponentInChildren<Canvas>();
        StartCoroutine(OneFrame(() => {
            canvas.overrideSorting = false;
        }));
    }

    private IEnumerator OneFrame(Action callback)
    {
        yield return null;
        callback();
    }
}
