using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Slider musicSlider;  // 슬라이더 참조
    public Slider soundSlider;
    public AudioSource[] audioSource;  // 오디오 소스 참조

    void Start()
    {
        // 슬라이더의 값이 변경될 때마다 OnMusicChange 함수 호출
        musicSlider.onValueChanged.AddListener(OnMusicChange);

        // 슬라이더 값을 AudioSource 볼륨과 동기화 (초기값 설정)
        musicSlider.value = audioSource[0].volume;

        // 슬라이더의 값이 변경될 때마다 OnMusicChange 함수 호출
        soundSlider.onValueChanged.AddListener(OnSoundChange);

        // 슬라이더 값을 AudioSource 볼륨과 동기화 (초기값 설정)
        soundSlider.value = audioSource[1].volume;
    }

    // 슬라이더 값이 변경될 때 실행되는 함수
    void OnMusicChange(float value)
    {
        audioSource[0].volume = value;
    }

    void OnSoundChange(float value)
    {
        audioSource[1].volume = value;
    }

    public void PlayBtnSound()
    {
        audioSource[1].Play();
    }
}

