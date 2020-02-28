﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text text;
    float start;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = System.TimeSpan.FromSeconds(Time.time - start).ToString("c");
    }
}
