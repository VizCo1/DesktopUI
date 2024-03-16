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

    private void Play3DSound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, _volumeUI * volumeMultiplier);
    }

    private void Play2DSound(AudioClip audioClip, float volumeMultiplier = 1f)
    {
        // Play audio
        _audioSource.PlayOneShot(audioClip, _volumeUI * volumeMultiplier);
    }

    #endregion

    #region PlayCertainSound

    public void PlayUISound()
    {
        Play2DSound(_audioClipsSO.UIsounds[Random.Range(0, _audioClipsSO.UIsounds.Length)]);
    }

    public void PlayConfirmSound()
    {
        Play2DSound(_audioClipsSO.confirmSound, 0.75f);
    }

    #endregion
}