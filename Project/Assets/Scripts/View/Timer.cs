using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text text;
    float start;
    private float? stopTime = null;
    public System.TimeSpan CurrentTime
    {
        get
        {
            return System.TimeSpan.FromSeconds((stopTime ?? Time.time) - start);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = CurrentTime.ToString("c");
    }

    public void Stop()
    {
        stopTime = Time.time;
    }

    public void Reset()
    {
        start = Time.time;
        stopTime = null;
    }
}
