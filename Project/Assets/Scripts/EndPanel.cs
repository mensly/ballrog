using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    public Text levelEndMessage;
    public Image star;
    public Color gold;
    public Color silver;
    public Color bronze;

    public void Show()
    {
        var time = model.timer.CurrentTime;
        levelEndMessage.text = string.Format("You beat the level in {0}", time.ToString("g"));
        if (time.TotalSeconds <= model.level.Gold)
        {
            star.color = gold;
        }
        else if (time.TotalSeconds <= model.level.Silver)
        {
            star.color = silver;
        }
        else
        {
            star.color = bronze;
        }
        gameObject.SetActive(true);

    }
}
