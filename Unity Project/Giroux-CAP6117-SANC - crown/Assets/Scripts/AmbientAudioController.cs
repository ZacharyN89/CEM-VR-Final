using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioController : MonoBehaviour
{
    private GameObject day;
    private GameObject night;
    private GameObject rain;

    private bool isDaytime;
    private bool isRaining;
    // Start is called before the first frame update
    void Start()
    {
        isDaytime = true;
        isRaining = false;
        day = transform.GetChild(0).gameObject;
        night = transform.GetChild(1).gameObject;
        rain= transform.GetChild(2).gameObject;
    }

    public void ToggleRain()
    {
        if (isRaining)
        {
            rain.SetActive(false);
            if (isDaytime) 
            { 
                day.SetActive(true); 
            }
            else
            {
                night.SetActive(true);
            }
        } 
        else
        {
            rain.SetActive(true);
            day.SetActive(false);
            night.SetActive(false);
        }
        isRaining=!isRaining;
    }
    public void ToggleDaytime()
    {
        if (isDaytime)
        {
            day.SetActive(false);
            if (isRaining)
            {
                rain.SetActive(true);
            }
            else
            {
                night.SetActive(true);
            }
        }
        else
        {
            night.SetActive(false);
            if (isRaining)
            {
                rain.SetActive(true);
            }
            else
            {
                day.SetActive(true); 
            }
        }
        isDaytime=!isDaytime;
    }
}
