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
        _logoPreview.DOFade(0, 1);
        Invoke(nameof(StartGame), 1.25f);
    }

    private void StartGame()
    {
        var level = PlayerPrefs.GetInt("LevelIndex", 1);
        SceneManager.LoadScene(level);
    }
}
