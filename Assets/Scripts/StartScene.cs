using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Image _logoPreview;
    private void Start()
    {
        Invoke(nameof(FadeImage), 1.5f);
        Invoke(nameof(StartGame), 2.5f);
    }

    private void StartGame()
    {
        var level = PlayerPrefs.GetInt("LevelIndex", 1);
        //SceneManager.LoadScene(level);
        SceneManager.LoadScene(1);
    }
    private void FadeImage()
    {
        _logoPreview.DOFade(0, .95f);
    }
}
