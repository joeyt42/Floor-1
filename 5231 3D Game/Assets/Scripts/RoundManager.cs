using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{

    public GameObject enemy1; // regular
    public GameObject enemy2; // big
    public GameObject enemy3; // health
    public int currentRound = 1;
    public int enemiesRemaining = 0;
    public Transform player;
    private bool roundActive = false;
    public TextMeshProUGUI roundNumberText;
    public MusicManager musicManager;

    void Start()
    {
        roundNumberText.text = "Round " + currentRound.ToString();
        musicManager.UpdateMusic(currentRound);
        StartCoroutine(StartNextRound());
    }

    void Update()
    {
        if (enemiesRemaining <= 0 && !roundActive)
        {
            currentRound++;
            roundNumberText.text = "Round " + currentRound.ToString();
            musicManager.UpdateMusic(currentRound);
            StartCoroutine(StartNextRound());
        }
    }

    IEnumerator StartNextRound()
    {
        roundActive = true;
        yield return new WaitForSeconds(5f);
        int enemiesToSpawn = GetEnemiesForRound(currentRound);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemyType(currentRound);
            enemiesRemaining++;
            yield return new WaitForSeconds(0.5f);
        }
        roundActive = false;
    }

    int GetEnemiesForRound(int round)
    {
        if (round == 1)
        {
            return 3;
        }

        if (round <= 5)
        {
            return 3 + (round - 1) * 2;
        }
        else if (round > 5 && round <= 10)
        {
            return 3 + (round - 1) * 3;
        }
        else
        {
            return 10 + (round - 1) * 5;
        }
    }

    void SpawnEnemyType(int round)
    {
        float spawnDistance = 7f;
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 randomPoint = new Vector3(player.position.x + randomCircle.x, player.position.y + 5f, player.position.z + randomCircle.y);

        UnityEngine.AI.NavMeshHit hit;
        Vector3 spawnPosition;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        else
        {
            spawnPosition = randomPoint;
        }

        float randomRoll = Random.Range(0f, 100f);

        GameObject typeToSpawn;

        if (round <= 5)
        {
            if (randomRoll < 70f)
            {
                typeToSpawn = enemy1;
            }
            else
            {
                typeToSpawn = enemy3;
            }
        }
        else if (round <= 10)
        {
            if (randomRoll < 70f)
            {
                typeToSpawn = enemy1;
            }
            else if (randomRoll < 85f)
            {
                typeToSpawn = enemy2;
            }
            else
            {
                typeToSpawn = enemy3;
            }
        }
        else
        {
            if (randomRoll < 50f)
            {
                typeToSpawn = enemy1;
            }
            else if (randomRoll < 75f)
            {
                typeToSpawn = enemy2;
            }
            else
            {
                typeToSpawn = enemy3;
            }
        }

        Instantiate(typeToSpawn, spawnPosition, Quaternion.identity);
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;
    }
}
