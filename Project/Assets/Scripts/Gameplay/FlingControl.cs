using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlingControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal;
    public float Vertical;

    public GameObject nub;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        nub.transform.position = eventData.position;
        Horizontal = nub.transform.localPosition.x;
        Vertical = nub.transform.localPosition.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        nub.transform.localPosition = Vector2.zero;
        Horizontal = 0;
        Vertical = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        nub.transform.position = eventData.position;
        Horizontal = nub.transform.localPosition.x;
        Vertical = nub.transform.localPosition.y;
    }
}
