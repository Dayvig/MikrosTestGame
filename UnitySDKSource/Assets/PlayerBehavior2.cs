using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerBehavior2 : MonoBehaviour
{
    public Vector3 targetPos;
    public float moveSpeed;
    public Manager gameManager;
    public float fireRate;
    public float timer;
    public float bulletSpeed;
    public int life = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void TakeInputs()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (targetPos.x > -gameManager.xBounds)
            {
                targetPos += Vector3.left * Time.deltaTime * moveSpeed;                
                if (targetPos.x < -gameManager.xBounds)
                {
                    targetPos.x = -gameManager.xBounds;
                }
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (targetPos.x < gameManager.xBounds)
            {
                targetPos += Vector3.right * Time.deltaTime * moveSpeed;                
                if (targetPos.x > gameManager.xBounds)
                {
                    targetPos.x = gameManager.xBounds;
                }
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (targetPos.y < gameManager.yBounds)
            {
                targetPos += Vector3.up * Time.deltaTime * moveSpeed;                
                if (targetPos.y > gameManager.yBounds)
                {
                    targetPos.y = gameManager.yBounds;
                }
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (targetPos.y > -gameManager.yBounds)
            {
                targetPos += Vector3.down * Time.deltaTime * moveSpeed;                
                if (targetPos.y < -gameManager.yBounds)
                {
                    targetPos.y = -gameManager.yBounds;
                }
            }
        }

        foreach (Touch touch in Input.touches)
        {
            if (Input.touches.Length == 1)
            {
                targetPos = Camera.main.ScreenToWorldPoint(touch.position);
                targetPos.z = transform.position.z;
            }
        }

        GameObject newBullet;
            if (timer >= fireRate)
            {
                timer = 0f;
                newBullet = Instantiate(gameManager.pBullet, transform.position, Quaternion.identity);
                newBullet.GetComponent<pBulletBehavior>().speed = bulletSpeed;

            }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TakeInputs();
        SetSmoothMovement();
        timer += Time.deltaTime;
        if (life < 0)
        {
            Application.Quit(0);
        }
    }

    void SetSmoothMovement()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 14f);
        transform.position = newPos;
    }
}
