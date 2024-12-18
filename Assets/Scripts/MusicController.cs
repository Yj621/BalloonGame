using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public Slider musicSlider;           // 배경음악 볼륨 슬라이더
    public Slider soundSlider;           // 효과음 볼륨 슬라이더
    public AudioSource musicSource;      // 배경음악 재생용 AudioSource
    public AudioSource soundSource;      // 효과음 재생용 AudioSource

    // 여러 효과음을 관리하는 Dictionary
    public List<AudioClip> soundClips;   // Inspector에서 효과음 목록 추가
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Dictionary에 효과음 추가
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundClips)
        {
            soundDictionary[clip.name] = clip; // 이름을 키로 사용
        }

        // 배경음악 슬라이더 설정
        if (musicSlider != null)
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // 효과음 슬라이더 설정
        if (soundSlider != null)
        {
            soundSlider.value = soundSource.volume;
            soundSlider.onValueChanged.AddListener(SetSoundVolume);
        }
    }

    /// <summary>
    /// 배경음악 볼륨 설정
    /// </summary>
    void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    /// <summary>
    /// 효과음 볼륨 설정
    /// </summary>
    void SetSoundVolume(float value)
    {
        soundSource.volume = value;
    }

    /// <summary>
    /// 특정 버튼 소리 재생
    /// </summary>
    public void PlaySoundEffect(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundSource.PlayOneShot(soundDictionary[soundName]);
            Debug.Log("효과음 재생: " + soundName);
        }
        else
        {
            Debug.LogWarning("해당 효과음이 없습니다: " + soundName);
        }
    }
}
