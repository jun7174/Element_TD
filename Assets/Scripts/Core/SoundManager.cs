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

namespace ElementTD
{
    // SFX 하나당 클립과 개별 볼륨을 함께 지정하기 위한 구조체이다.
    // Type을 명시적으로 지정하므로, 인스펙터에 채우는 순서가 enum 선언 순서와 달라도 안전하다.
    [System.Serializable]
    public struct SfxEntry
    {
        public ESfx Type;
        public AudioClip Clip;

        [Range(0f, 2f)]
        public float Volume;
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] bgmClips;   // BGM 클립 배열
        [SerializeField] private SfxEntry[] sfxEntries;  // SFX 클립 + 개별 볼륨 배열

        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource; // BGM 재생 AudioSource
        [SerializeField] private AudioSource sfxSource; // SFX 재생 AudioSource

        private Dictionary<EBgm, AudioClip> bgmDict; // BGM Dictionary
        private Dictionary<ESfx, SfxEntry> sfxDict;   // SFX Dictionary

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

            sfxDict = new Dictionary<ESfx, SfxEntry>();
            foreach (SfxEntry entry in sfxEntries)
            {
                sfxDict[entry.Type] = entry;
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

        // SFX 재생 (엔트리에 지정된 개별 볼륨이 함께 적용된다)
        public void PlaySFX(ESfx sfxType)
        {
            if (sfxDict.TryGetValue(sfxType, out var entry))
            {
                sfxSource.PlayOneShot(entry.Clip, entry.Volume);
            }
            else
            {
                Debug.LogWarning("SFX not found in Dictionary!");
            }
        }
    }
}
