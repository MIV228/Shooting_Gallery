using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject particles;

    public void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        FindObjectOfType<Score>().AddScore();
        Destroy(gameObject);
    }
}
