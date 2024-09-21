using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Slider musicSlider;  // �����̴� ����
    public Slider soundSlider;
    public AudioSource[] audioSource;  // ����� �ҽ� ����

    void Start()
    {
        // �����̴��� ���� ����� ������ OnMusicChange �Լ� ȣ��
        musicSlider.onValueChanged.AddListener(OnMusicChange);

        // �����̴� ���� AudioSource ������ ����ȭ (�ʱⰪ ����)
        musicSlider.value = audioSource[0].volume;

        // �����̴��� ���� ����� ������ OnMusicChange �Լ� ȣ��
        soundSlider.onValueChanged.AddListener(OnSoundChange);

        // �����̴� ���� AudioSource ������ ����ȭ (�ʱⰪ ����)
        soundSlider.value = audioSource[1].volume;
    }

    // �����̴� ���� ����� �� ����Ǵ� �Լ�
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

