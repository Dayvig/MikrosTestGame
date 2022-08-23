using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eBulletBehavior : MonoBehaviour
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
        transform.position += Vector3.down * Time.deltaTime * speed;
        CheckCollision();
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }

    }
    
    void CheckCollision()
    {
        var colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var c in colliders)
        {
            if (c.gameObject.tag == "PlayerShip")
            {
                c.GetComponent<PlayerBehavior2>().life--;
                Destroy(this.gameObject);
            }
        }
    }
}
