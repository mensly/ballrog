using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementControl : MonoBehaviour, IPointerUpHandler
{
    public float Horizontal;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = 2 * (slider.value - 0.5f);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        slider.value = 0.5f;
    }
}
