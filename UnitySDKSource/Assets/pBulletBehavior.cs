using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pBulletBehavior : MonoBehaviour
{
    public float limit;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
        CheckCollision();
        if (transform.position.y > 6)
        {
            Destroy(this.gameObject);
        }
    }

    void CheckCollision()
    {
        var colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                Destroy(c.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
