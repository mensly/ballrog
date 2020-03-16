using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;

[RequireComponent(typeof(Collider2D))]
public class TutorialSection : MonoBehaviour
{
    public Canvas canvas;
    public Text tutorial;
    [TextArea(3, 5)]
    public string text;


    void OnTriggerEnter2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            if (text.Length == 0)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                canvas.gameObject.SetActive(true);
                tutorial.text = text;
            }
        }
    }
}
