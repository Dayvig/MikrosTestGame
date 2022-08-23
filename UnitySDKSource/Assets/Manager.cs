using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    private int gameState;
    public float xBounds;
    public float yBounds;
    public GameObject playerShip;
    public GameObject pBullet;
    public GameObject eBullet;
    public GameObject enemy;
    public float spawnRate;
    public float spawnNumber;
    public float timer;
    private GameObject newEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        gameState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > spawnRate)
        {
            timer = 0f;
            for (int i = 0; i < spawnNumber; i++)
            {
                float randomDelay = Random.Range(5f, 1f);
                float randomX = Random.Range(-3.5f, 3.5f);
                float randomOffset = Random.Range(-2f, 0f);
                Vector3 startPos = new Vector3(randomX, 4.5f + randomDelay, 0f);
                newEnemy = Instantiate(enemy, startPos, Quaternion.identity);
                newEnemy.GetComponent<enemyBehavior>().targetPos = new Vector3(randomX, 4.5f + randomOffset, 0f);
            }

            spawnRate /= 1.1f;
            if (spawnRate < 0.8f)
            {
                spawnNumber += 2;
                spawnRate = 2f;
            }
        }
    }
}
