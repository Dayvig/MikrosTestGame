using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
    public Vector3 targetPos;
    public float moveSpeed;
    public float fireRate;
    public bool movingRight;
    public float timer;
    private GameObject newBullet;
    public Manager gameManager;
    public float bulletSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        gameManager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSmoothMovement();
        if (transform.position.y < 6f){
        timer += Time.deltaTime;
        }
        if (timer > fireRate)
        {
            timer = 0f;
            newBullet = Instantiate(gameManager.eBullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<eBulletBehavior>().speed = bulletSpeed;
        }

        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            if (transform.position.x > 2f)
            {
                targetPos = new Vector3(-3.5f, transform.position.y, 0f);
            }
            else
            {
                targetPos = new Vector3(3.5f, transform.position.y, 0f);
            }
        }
    }
    
    void SetSmoothMovement()
    {
        Vector3 toPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = toPos;
    }
}
