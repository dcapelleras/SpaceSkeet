using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    float timeBetweenSpawns = 1f;
    [SerializeField] TMP_Text pointsText;
    int points;
    [SerializeField] TMP_Text playerHpText;
    int playerHp;
    bool gameOver;
    [SerializeField] GameObject GameOverText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        playerHp = 10;
        playerHpText.text = "Vida: " + playerHp.ToString();
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
        }
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

    public void ScorePoint()
    {
        points++;
        pointsText.text = points.ToString();
    }

    public void DamagePlayer()
    {
        playerHp--;
        playerHpText.text = "Vida: " + playerHp.ToString();
        if (playerHp <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        GameOverText.SetActive(true);
        gameOver = true;
        Time.timeScale = 0;
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
