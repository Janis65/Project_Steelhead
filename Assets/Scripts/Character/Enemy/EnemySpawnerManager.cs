using JO;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnerManager : MonoBehaviour
{
    public static List<EnemySpawnerManager> AllSpawners = new List<EnemySpawnerManager>();

    [SerializeField] GameObject enemyPrefab;
    GameObject spawnedEnemy;


    [SerializeField] int enemyLevel = 1;
    [SerializeField] int healthLevel = 10;
    [SerializeField] public int strengthLevel = 10;
    [SerializeField] int baseSufferingReward = 10;

    private void OnEnable()
    {
        AllSpawners.Add(this);
    }

    private void OnDisable()
    {
        AllSpawners.Remove(this);
    }

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        EnemyManager enemyManager = spawnedEnemy.GetComponent<EnemyManager>();

        // 3. Jeœli klon faktycznie ma ten skrypt, wywo³ujemy w nim funkcjê i przekazujemy dane
        if (enemyManager != null)
        {
            enemyManager.enemyStats.enemyLevel = enemyLevel;
            enemyManager.enemyStats.strengthLevel = strengthLevel;
            enemyManager.enemyStats.baseSufferingReward = baseSufferingReward;

            enemyManager.enemyStats.SetMaxHealthFromHealthLevel(healthLevel);
            enemyManager.enemyStats.currentHealth = enemyManager.enemyStats.maxHealth;
            enemyManager.enemyStats.enemyHealthBar.SetMaxHealth(enemyManager.enemyStats.maxHealth);

        }
        else
        {
            Debug.LogError("EnemyManager not found");
        }
    }

    public void DestroyEnemy()
    {
        if (spawnedEnemy != null)
        {
            Destroy(spawnedEnemy);
        }
    }
}
