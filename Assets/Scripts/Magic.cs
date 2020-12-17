using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject firePrefab;

    public void FireAttack(bool Right)
    {
        GameObject fireObject = Instantiate(firePrefab);
        fireObject.transform.position = transform.position;
        fireObject.transform.right = Right ? transform.right : transform.right * -1.0f;
    }
}
