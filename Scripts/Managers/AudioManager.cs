using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private AudioSource _audioSource;
    [Space]
    [SerializeField]
    private List<AudioClip> _shootAudioClips;
    [Space]
    [SerializeField]
    private List<AudioClip> _explosionAudioClips;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootSounds()
    {
        _audioSource.clip = _shootAudioClips[Random.Range(0, _shootAudioClips.Count)];

        _audioSource.Play();
    }

    public void PlayExplosionSound()
    {
        _audioSource.clip = _explosionAudioClips[Random.Range(0, _explosionAudioClips.Count)];

        _audioSource.Play();
    }
}