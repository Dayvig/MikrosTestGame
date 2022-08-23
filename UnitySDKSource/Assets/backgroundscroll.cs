using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundscroll : MonoBehaviour
{
    public float scrollSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * scrollSpeed;
        if (transform.position.y < -28f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 28f, transform.position.z);
        }
    }
}
