using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    [Header("Enemy Spawn")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [Space]
    [SerializeField]
    private GameObject _enemyContainer;
    [Space]
    [SerializeField]
    private float _enemyStartSpawnDelay = 1f;
    [Space]
    [SerializeField]
    private float _enemySpawnDelay = 4f;
    [Space]
    [SerializeField]
    private bool canSpawnEnemies = true;
    
    [Header("Power Up Spawn")]
    [SerializeField]
    private List<GameObject> _powerUps;
    [Space]
    [SerializeField]
    private bool canSpawnPowerUps = true;
    [Space]
    [SerializeField]
    private GameObject _powerUpContainer;
    [Space]
    [SerializeField]
    private float _powerUpSpawnDelay = 3f;
    [Space]
    [SerializeField]
    private float _powerUpStarSpawnDelay = 1f;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyCoroutine());

        StartCoroutine(SpawnPowerUpCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        yield return new WaitForSeconds(_enemyStartSpawnDelay);

        while (canSpawnEnemies)
        {
            Instantiate(_enemyPrefab, _enemyContainer.transform);

            yield return new WaitForSeconds(_enemySpawnDelay);
        }
    }

    public void StopSpawningEnemies()
    {
        canSpawnEnemies = false;

        StopAllCoroutines();
    }

    IEnumerator SpawnPowerUpCoroutine()
    {
        yield return new WaitForSeconds(_powerUpStarSpawnDelay);

        while (canSpawnPowerUps)
        {
            int randomizePowerUp = Random.Range(0, _powerUps.Count);

            Instantiate(_powerUps[randomizePowerUp], _powerUpContainer.transform);

            yield return new WaitForSeconds(_powerUpSpawnDelay);
        }
    }
}