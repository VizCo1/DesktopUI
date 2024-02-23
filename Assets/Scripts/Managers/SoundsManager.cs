using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    [SerializeField] private AudioClipsSO _audioClipsSO;
    [SerializeField] private float _volumeUI = 1;

    public float VolumeUI { get => _volumeUI; set => _volumeUI = value; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
    }

    #region PlayAudioLogic

    private void Play3DSound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, _volumeUI);
    }

    private void Play2DSound(AudioClip audioClip)
    {
        // Play audio
        _audioSource.PlayOneShot(audioClip, _volumeUI);
    }

    #endregion

    #region PlayCertainSound

    public void PlayUISound()
    {
        Play2DSound(_audioClipsSO.soundsUI[Random.Range(0, _audioClipsSO.soundsUI.Length)]);
    }

    #endregion
}