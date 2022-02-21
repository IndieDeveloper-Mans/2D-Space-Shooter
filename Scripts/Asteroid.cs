using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;
    [Space]
    [SerializeField]
    private float _speed;
    [Space]
    [SerializeField]
    private GameObject _explosionPrefab;
    [Space]
    [SerializeField]
    private float _destroyDelay;
 
    void Update()
    {
        MoveAndRotate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Laser>(out var laser))
        {
            Destroy(GetComponent<CircleCollider2D>());

            Instantiate(_explosionPrefab, transform.position, transform.rotation);

            collision.gameObject.SetActive(false);

            SpawnManager.Instance.StartSpawning();

            Destroy(gameObject, _destroyDelay);
        }

        if (collision.TryGetComponent<Player>(out var player))
        {
            Destroy(GetComponent<CircleCollider2D>());

            player.PlayerDeath();

            Destroy(collision.gameObject);
        }
    }

    public void MoveAndRotate()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }
}