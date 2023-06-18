using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        var level = PlayerPrefs.GetInt("LevelIndex", 1);
        SceneManager.LoadScene(level);
    }
}
