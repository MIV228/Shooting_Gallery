using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float cd;
    public float maxcd;

    public Transform[] points;
    public GameObject bottle;
    public GameObject bottle_lvl2;

    void Update()
    {
        if (cd > 0) cd -= Time.deltaTime;
        else
        {
            if (FindObjectsOfType<Enemy>().Length > 40) return;

            cd = maxcd;
            if (maxcd > 0.2f) maxcd -= 0.05f;
            if (Random.Range(1, Mathf.RoundToInt(maxcd * 10) + 1) == 1)
            {
                Instantiate(bottle_lvl2, points[Random.Range(0, points.Length)].position, bottle_lvl2.transform.rotation);
            }
            else Instantiate(bottle, points[Random.Range(0, points.Length)].position, bottle.transform.rotation);
        }
    }
}
