using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action<int> onPlayerDamaged;

    [Header("Player")]
    [SerializeField]
    private int _lives = 3;
    public int Lives
    {
        get
        {
            return _lives;
        }
    }
    [Space]
    [SerializeField]
    private float _speed = 3.0f;
    [Space]
    [SerializeField]
    private int _score;

    [Header("Player Bounds")]
    [SerializeField]
    private Vector2 _positiveBounds;
    [Space]
    [SerializeField]
    private Vector2 _negativeBounds;

    [Header("Laser")]
    [SerializeField]
    private GameObject _laserPrefab;
    [Space]
    [SerializeField]
    private Vector3 _laserOffset;

    [Header("Cooldown")]
    [SerializeField]
    private float _fireRate = 0.25f;
    [Space]
    [SerializeField]
    private float _canFire = 0f;

    [Header("Inputs")]
    [SerializeField]
    private Vector2 _direction;

    [Header("Shots")]
    [SerializeField]
    private bool _tripleShotEnabled;
    [Space]
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [Space]
    [SerializeField]
    private float _tripleShotTime;

    [Header("SpeedUp")]
    [SerializeField]
    private bool _speedUpEnabled;
    [Space]
    [SerializeField]
    private float _speedUpTime;
    [Space]
    [SerializeField]
    private float _speedUpMultiplier;

    [Header("Shield")]
    [SerializeField]
    private GameObject _shieldObject;
    [Space]
    [SerializeField]
    private bool _shieldEnabled;
    [Space]
    [SerializeField]
    private float _shieldTime;

    [Header("Fire")]
    [SerializeField]
    private List<GameObject> _fires;

    void Start()
    {
        transform.position = Vector3.zero;
    }

    private void OnEnable()
    {
        Enemy.onEnemyKilled += AddScore;
    }

    private void OnDisable()
    {
        Enemy.onEnemyKilled -= AddScore;
    }

    void Update()
    {
        Movement();

        UseBounds();

        ShootLaser();
    }

    void Movement()
    {
        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Translate(new Vector3(_direction.x, _direction.y) * _speed * Time.deltaTime/*, Space.World*/);
    }

    void UseBounds()
    {
        // clamp player y pos between y bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _negativeBounds.y, _positiveBounds.y), 0);

        // x bounds with wrap pos
        if (transform.position.x >= _positiveBounds.x)
        {
            transform.position = new Vector3(-_positiveBounds.x, transform.position.y, 0);
        }
        else if (transform.position.x <= _negativeBounds.x)
        {
            transform.position = new Vector3(-_negativeBounds.x, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotEnabled)
            {
                Instantiate(_tripleShotPrefab, transform.position + _laserOffset, Quaternion.identity);
            }
            else
            {
                GameObject laser = PoolManager.Instance.RequestLaser();

                laser.transform.position = transform.position + _laserOffset;
            }

            AudioManager.Instance.PlayShootSounds();
        }
    }

    #region Power Ups
    public void TripleShot()
    {
        _tripleShotEnabled = true;

        StartCoroutine(TripleShotCoroutine());
    }

    IEnumerator TripleShotCoroutine()
    {
        yield return new WaitForSeconds(_tripleShotTime);

        _tripleShotEnabled = false;
    }

    public void SpeedUp()
    {
        _speedUpEnabled = true;

        _speed *= _speedUpMultiplier;

        StartCoroutine(SpeedUpCoroutine());
    }

    IEnumerator SpeedUpCoroutine()
    {
        yield return new WaitForSeconds(_speedUpTime);

        _speedUpEnabled = false;

        _speed /= _speedUpMultiplier;
    }

    public void Shield()
    {
        _shieldEnabled = true;

        _shieldObject.SetActive(true);

        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        yield return new WaitForSeconds(_shieldTime);

        _shieldEnabled = false;

        _shieldObject.SetActive(false);
    }
    #endregion

    public void AddScore(int points)
    {
        _score += points;
    }

    public void RandomizeFire()
    {
        if (_fires.Count != 0)
        {
            int randomFire = UnityEngine.Random.Range(0, _fires.Count);

            if (!_fires[randomFire].activeInHierarchy)
            {
                _fires[randomFire].SetActive(true);

                _fires.RemoveAt(randomFire);
            }
        }
    }

    public void PlayerDamaged()
    {
        if (_shieldEnabled)
        {
            _shieldEnabled = false;

            _shieldObject.SetActive(false);

            Debug.Log("Deactivate Shield");
            return;
        }
        else
        {
            _lives--;

            onPlayerDamaged(_lives);

            RandomizeFire();

            if (_lives < 1)
            {
                PlayerDeath();

                Destroy(gameObject);
            }

            AudioManager.Instance.PlayExplosionSound();
        }
    }

    public void PlayerDeath()
    {
        SpawnManager.Instance.StopSpawningEnemies();

        GameManager.Instance.GameOver();
    }
}