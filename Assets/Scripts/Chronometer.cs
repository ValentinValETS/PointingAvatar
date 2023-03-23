using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Chronometer : MonoBehaviour
{
    public GameObject bar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutomaticTimer(float time)
    {
        LeanTween.scaleX(bar, 1, time);
    }

    public void setTimer(float value)
    {
        LeanTween.scaleX(bar, Mathf.Clamp01(value), 0);
    }

    public void CleanTimer()
    {
        LeanTween.scaleX(bar, 0, 0);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        CleanTimer();
        gameObject.SetActive(false);
    }
}
