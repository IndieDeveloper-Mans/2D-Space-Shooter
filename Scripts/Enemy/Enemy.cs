using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    public static Action<int> onEnemyKilled;

    [Header("Enemy")]
    [SerializeField]
    private Animator _enemyAnimator;
    [Space]
    [SerializeField]
    private AudioSource _audioSource;
    [Space]
    [SerializeField]
    private float _speed = 4.0f;
    [Space]
    [SerializeField]
    private float _diedSpeed = 0.1f;
    [Space]
    [SerializeField]
    private int _pointsForKill;

    [Header("Enemy Shoot")]
    [SerializeField]
    private GameObject _laserPrefab;
    [Space]
    [SerializeField]
    private float _fireRate = 2f;
    [Space]
    [SerializeField]
    private float _canFire = 0;
    [Space]
    [SerializeField]
    private float _shotDelayMin, _shotDelayMax;

    [Header("Enemy Bounds")]
    [SerializeField]
    private Vector2 _positiveBounds;
    [Space]
    [SerializeField]
    private Vector2 _negativeBounds;

    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        RandomizePosition();
    }

    void Update()
    {
        Movement();

        if (Time.time > _canFire)
        {
            _fireRate = UnityEngine.Random.Range(_shotDelayMin, _shotDelayMax);

            _canFire = Time.time + _fireRate;

            Instantiate(_laserPrefab, transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var playerScript))
        {
            Destroy(GetComponent<BoxCollider2D>());

            playerScript.PlayerDamaged();

            _enemyAnimator.SetTrigger("Enemy Explode");

            _speed = _diedSpeed;

            _audioSource.Play();

            Destroy(gameObject, _enemyAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void RandomizePosition()
    {
        float randomX = UnityEngine.Random.Range(_negativeBounds.x, _positiveBounds.x);

        transform.position = new Vector3(randomX, _positiveBounds.y, 0);
    }

    public void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= _negativeBounds.y)
        {
            RandomizePosition();
        }
    }

    public void EnemyDamaged()
    {
        Destroy(GetComponent<BoxCollider2D>());

        onEnemyKilled?.Invoke(_pointsForKill);

        _enemyAnimator.SetTrigger("Enemy Explode");

        _speed = _diedSpeed;

        _audioSource.Play();

        Destroy(gameObject, _enemyAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
}