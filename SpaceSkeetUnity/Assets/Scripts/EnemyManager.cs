using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    float timeBetweenSpawns = 1f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (!GameManager.instance.gameOver)
        {
            Transform pos = GetPlace();
            if (pos != null)
            {
                Instantiate(GetEnemy(), pos);
            }
            else
            {
                Debug.Log("No available spots");
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
            timeBetweenSpawns -= Time.time * 0.001f;
            timeBetweenSpawns = Mathf.Clamp(timeBetweenSpawns, 0.2f, 5f);
        }
    }

    GameObject GetEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[randomIndex];
    }

    Transform GetPlace()
    {
        List<Transform> availablePoints = new List<Transform>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].childCount == 0)
            {
                availablePoints.Add(spawnPoints[i]);
            }
        }
        int randomIndex = Random.Range(0, availablePoints.Count);
        if (availablePoints.Count== 0)
        {
            return null;
        }
        return availablePoints[randomIndex];
    }
}
