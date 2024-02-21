using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObjects/AudioClips")]
public class AudioClipsSO : ScriptableObject
{
    public AudioClip[] soundsUI;
}