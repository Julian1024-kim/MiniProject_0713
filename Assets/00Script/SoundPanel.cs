using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    public Button closeBtn;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (closeBtn != null)
            closeBtn.onClick.AddListener(CloseSettingPanel);

        // 슬라이더 리스너 연결
        bgmSlider.onValueChanged.AddListener(BGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
    }

    private void OnEnable()
    {
        if (SoundManager.instance != null)
        {
            bgmSlider.value = SoundManager.instance.GetBGMVolume();
            sfxSlider.value = SoundManager.instance.GetSFXVolume();
        }
    }

    public void BGMVolumeChanged(float volume)
    {
        SoundManager.instance.SetBGMVolume(volume);
    }

    public void SFXVolumeChanged(float vol)
    {
        SoundManager.instance.SetSFXVolume(vol);
    }

    public void CloseSettingPanel()
    {
        gameObject.SetActive(false);
    }
}
