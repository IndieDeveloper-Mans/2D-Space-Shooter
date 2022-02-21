using UnityEngine;

public enum LaserOwner
{
    player,
    enemy
}

public class Laser : MonoBehaviour
{
    [SerializeField]
    public LaserOwner _laserOwner;
    [Space]
    [SerializeField]
    private float _speed;
    [Space]
    [SerializeField]
    private float _destroyPos;

    void Update()
    {
        LaserMovement();
    }

    public void LaserMovement()
    {
        if (_laserOwner == LaserOwner.player)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        else if (_laserOwner == LaserOwner.enemy)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y >= _destroyPos)
        {         
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var playerScript))
        {
            if (_laserOwner == LaserOwner.enemy)
            {
                playerScript.PlayerDamaged();

                gameObject.SetActive(false);
            }           
        }

        if (other.TryGetComponent<Enemy>(out var enemyScript))
        {
            if (_laserOwner == LaserOwner.player)
            {
                enemyScript.EnemyDamaged();

                gameObject.SetActive(false);
            }
        }
    }
}