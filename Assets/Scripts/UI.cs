using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Management _management;
    void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        _levelText.text = _management.LevelIndex.ToString();
    }
}
