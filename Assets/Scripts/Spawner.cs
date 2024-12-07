using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float cd;
    public float maxcd;

    public Transform[] points;
    public GameObject bottle;

    void Update()
    {
        if (cd > 0) cd -= Time.deltaTime;
        else
        {
            if (FindObjectsOfType<Enemy>().Length > 40) return;

            cd = maxcd;
            if (maxcd > 0.2f) maxcd -= 0.05f;
            Instantiate(bottle, points[Random.Range(0, points.Length)].position, bottle.transform.rotation);
        }
    }
}
