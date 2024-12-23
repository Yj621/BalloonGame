using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public Slider musicSlider;
    public Slider soundSlider;
    public AudioSource musicSource;  // BGM 재생용
    public AudioSource soundSource;  // 효과음 재생용
    
    public List<AudioClip> soundClips;  // 효과음 목록
    public List<BGMClip> bgmClips;      // 씬 별 BGM 목록

    private Dictionary<string, AudioClip> soundDictionary;
    private Dictionary<string, AudioClip> bgmDictionary;

    [System.Serializable]
    public class BGMClip
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    private void Awake()
    {
        // 싱글톤 설정 (중복 방지)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);  // 중복 생성 방지
            return;
        }
    }

    private void Start()
    {
        // 효과음 초기화
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundClips)
        {
            soundDictionary[clip.name] = clip;
        }

        // 씬별 BGM 초기화
        bgmDictionary = new Dictionary<string, AudioClip>();
        foreach (var bgm in bgmClips)
        {
            bgmDictionary[bgm.sceneName] = bgm.bgmClip;
        }

        // 씬 전환 감지 (씬 로드 시 BGM 변경)
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 현재 씬 BGM 재생
        PlayBGMForScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 씬이 로드될 때 호출 (BGM 변경)
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    /// <summary>
    /// 씬 이름에 맞는 BGM 재생
    /// </summary>
    public void PlayBGMForScene(string sceneName)
    {
        if (bgmDictionary.ContainsKey(sceneName))
        {
            PlayBGM(bgmDictionary[sceneName]);
        }
        else
        {
            StopBGM();
        }
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    public void PlayBGM(AudioClip bgmClip)
    {
        if (musicSource.clip == bgmClip && musicSource.isPlaying)
            return;

        musicSource.clip = bgmClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// BGM 정지
    /// </summary>
    public void StopBGM()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public void PlaySoundEffect(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundSource.PlayOneShot(soundDictionary[soundName]);
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
    public void PlayBtnSound()
    {
        if (soundSource != null && soundDictionary.ContainsKey("ClickSound"))
        {
            soundSource.PlayOneShot(soundDictionary["ClickSound"]);
        }
        else
        {
            Debug.LogWarning("ClickSound가 존재하지 않거나 AudioSource가 없습니다.");
        }
    }
}
