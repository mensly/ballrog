using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenControl : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MainScene").completed += (obj) =>
        {
            var tutorial = FindObjectOfType<Level>();
            Instantiate(tutorial.NextLevel);
            Destroy(tutorial.gameObject);
        };
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("MainScene");

    }
}
