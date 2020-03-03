using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

public class Level : MonoBehaviour
{
    public float Silver = 5f * 60;
    public float Gold = 1.5f * 60;

    public GameObject NextLevel;
    public Transform SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        model.level = this;
        model.spawnPoint = SpawnPoint;
        model.player.transform.position = SpawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        model.player.transform.position = SpawnPoint.position;
    }
}
