using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using Unity.UIWidgets.widgets;
using UnityEngine;

public class ChargeIndicator : MonoBehaviour
{
    private float initialScale;
    readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    void Start()
    {
        initialScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        if (model.player.charging)
        {
            transform.localRotation = Quaternion.AngleAxis(180 / Mathf.PI *
                Mathf.Atan2(model.player.jumpTakeoff.y, model.player.jumpTakeoff.x),
                Vector3.forward);
            scale.x = Mathf.Min(model.player.charge / 2, 1f) * initialScale;
        }
        else
        {
            scale.x = 0;
        }
        transform.localScale = scale;
    }
}
