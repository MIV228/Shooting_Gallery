using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject particles;

    public int hp;
    public int score;

    public bool boss;

    public void Hurt()
    {
        hp -= 1;

        if (hp <= 0)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Score s = FindObjectOfType<Score>();
            for (int i = 0; i < score; i++)
            {
                s.AddScore();
            }
            Destroy(gameObject);
        }
    }
}
