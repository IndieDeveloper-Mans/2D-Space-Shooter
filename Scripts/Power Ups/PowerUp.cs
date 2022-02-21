using UnityEngine;

public enum PowerUpType
{
    tripleShot,
    speed,
    shield
}

public class PowerUp : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField]
    private PowerUpType _powerUpType;
    [Space]
    [SerializeField]
    private AudioClip _audioClip;
    [Space]
    [SerializeField]
    private float _speed;

    [Header("Power Up Bounds")]
    [SerializeField]
    private Vector2 _positiveBounds;
    [Space]
    [SerializeField]
    private Vector2 _negativeBounds;

    private void Start()
    {
        RandomizePosition();
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= _negativeBounds.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var playerScript))
        {
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            switch (_powerUpType)
            {
                case PowerUpType.tripleShot:
                    playerScript.TripleShot();
                    Debug.Log("TripleShot");
                    break;

                case PowerUpType.speed:
                    playerScript.SpeedUp();
                    Debug.Log("Speed");
                    break;

                case PowerUpType.shield:
                    playerScript.Shield();
                    Debug.Log("Shield");
                    break;
            }

            Destroy(gameObject);
        }
    }

    void RandomizePosition()
    {
        float randomX = Random.Range(_negativeBounds.x, _positiveBounds.x);

        transform.position = new Vector3(randomX, _positiveBounds.y, 0);
    }
}