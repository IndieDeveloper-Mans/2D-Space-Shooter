using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource _audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        DestroyExplosion();
    }

    void DestroyExplosion()
    {
        _audioSource.Play();

        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
}