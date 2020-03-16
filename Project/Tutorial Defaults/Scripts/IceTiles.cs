using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class IceTiles : MonoBehaviour
{
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        model.player.StartIce();
    }


    void OnCollisionStay2D(Collision2D collision)
    {
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        model.player.EndIce();
    }
}