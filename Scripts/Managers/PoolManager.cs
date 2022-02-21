using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField]
    private List<GameObject> _laserPool;
    [Space]
    [SerializeField]
    private GameObject _laserPrefab;
    [Space]
    [SerializeField]
    private GameObject _laserContainer;
    [Space]
    [SerializeField]
    private int _laserCount;

    private void Start()
    {
        _laserPool = GenerateLasers(_laserCount);
    }

    List<GameObject> GenerateLasers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateLaser();
        }

        return _laserPool;
    }

    public GameObject RequestLaser()
    {
        foreach (var laser in _laserPool)
        {
            if (!laser.activeInHierarchy)
            {
                laser.SetActive(true);

                return laser;
            }          
        }

        GameObject newLaser = CreateLaser();

        newLaser.SetActive(true);

        return newLaser;
    }

    GameObject CreateLaser()
    {
        GameObject laser = Instantiate(_laserPrefab, _laserContainer.transform);

        laser.SetActive(false);

        _laserPool.Add(laser);

        return laser;
    }
}