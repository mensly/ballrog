using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class Catnip : MonoBehaviour
{
    private Vector3 originalPosition;
    float moveDistance = 0.1f;
    float moveSpeed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalPosition + moveDistance * Mathf.Sin(Time.time / moveSpeed) * Vector3.up;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.catnip = true;
            Destroy(gameObject);
        }
    }
}
