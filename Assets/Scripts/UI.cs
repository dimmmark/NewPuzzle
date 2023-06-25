using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Management _management;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Sprite _soundImageOn;
    [SerializeField] private Sprite _soundImageOff;
    private static bool _isMute;
    void Start()
    {
        _soundButton.onClick.AddListener(Mute);
        UpdateInfo();
        if (_isMute)
            _soundButton.image.sprite = _soundImageOff;
        else
            _soundButton.image.sprite = _soundImageOn;
    }

    private void UpdateInfo()
    {
        _levelText.text = _management.LevelIndex.ToString();
    }
    public void Mute()
    {
        if (_isMute)
        {
            AudioListener.volume = 1;
            _isMute = false;
            _soundButton.image.sprite = _soundImageOn;
        }
        else
        {
            AudioListener.volume = 0;
            _isMute = true;
            _soundButton.image.sprite = _soundImageOff;
        }
    }
}
