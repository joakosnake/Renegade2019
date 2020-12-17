using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 0.5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)   
            Destroy(gameObject);
    }
}
