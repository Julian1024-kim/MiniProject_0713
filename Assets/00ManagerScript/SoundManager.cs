using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SFXType
{
    Shot,
    Die,
    Attack,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    [System.Serializable]
    public struct SceneBGMData
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [Header("BGM Settings")]
    public List<SceneBGMData> sceneBGMList;

    [Header("SFX Settings")]
    public AudioClip[] soundClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1)); //로컬에 저장된거 가져오기. 처음실행할땐 저장된게 없으니 뒤에 1이라는 기본값 넣어주기
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1));
    }
    public void SetBGMVolume(float volume)
    {
        BGMSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume); //로컬에 저장
    }
    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); //로컬에 저장

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 바뀔 때마다 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneBGMData data = sceneBGMList.Find(x => x.sceneName == scene.name);

        if (data.bgmClip != null)
        {
            ChangeBGM(data.bgmClip);
        }
    }

    public void ChangeBGM(AudioClip newClip)
    {
        // 같은음악재생중이면 무시
        if (BGMSource.clip == newClip)
            return;

        BGMSource.Stop();
        BGMSource.clip = newClip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlaySFX(SFXType type)
    {
        if ((int)type >= soundClip.Length)
            return;

        SFXSource.PlayOneShot(soundClip[(int)type]);
    }
    public float GetBGMVolume()
    {
        return BGMSource.volume;
    }
    public float GetSFXVolume()
    {
        return SFXSource.volume;
    }
}