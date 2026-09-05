// Assets/Scripts/Core/SoundManager.cs

using UnityEngine;
using System.Collections.Generic;

// 배경음악(BGM)
public enum EBgm
{
    TITLE,
    GAME,
    RESULT,
}

// 효과음(SFX)
public enum ESfx
{
    BASIC_ATK,
    RAPID_ATK,
    SPLAH_ATK,
    SNIPER_ATK,
    BUTTON_CLICK,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgmClips;  // BGM 클립 배열
    [SerializeField] private AudioClip[] sfxClips; // SFX 클립 배열

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // BGM 재생 AudioSource
    [SerializeField] private AudioSource sfxSource; // SFX 재생 AudioSource

    private Dictionary<EBgm, AudioClip> bgmDict; // BGM Dictionary
    private Dictionary<ESfx, AudioClip> sfxDict; // SFX Dictionary

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

        InitDictionaries();
    }

    // Dictionary 초기화
    private void InitDictionaries()
    {
        bgmDict = new Dictionary<EBgm, AudioClip>();
        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmDict[(EBgm)i] = bgmClips[i];
        }

        sfxDict = new Dictionary<ESfx, AudioClip>();
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxDict[(ESfx)i] = sfxClips[i];
        }
    }

    private void Start()
    {
        PlayBGM(EBgm.GAME);
    }
    // BGM 재생
    public void PlayBGM(EBgm bgmType)
    {
        if (bgmDict.TryGetValue(bgmType, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // 배경음악은 기본적으로 반복 재생
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found in Dictionary!");
        }
    }

    // SFX 재생
    public void PlaySFX(ESfx sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found in Dictionary!");
        }
    }
}