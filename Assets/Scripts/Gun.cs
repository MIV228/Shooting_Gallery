using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public GameObject shootFlash;
    public TrailRenderer bulletTrail;
    //public GameObject impactParticles;
    public InputActionProperty attackAction;
    public Transform attackPoint;

    public float maxcd;
    public float cd;

    public Animator animator;

    public GameObject shootSound;

    public bool dontAnimate;
    float lastRotation;

    public void Shoot()
    {
        if (!dontAnimate) animator.Play("shoot");
        cd = maxcd;
        if (!dontAnimate)
        {
            GameObject f = Instantiate(shootFlash, attackPoint.position, attackPoint.rotation);
            f.transform.Rotate(0, 0, Random.Range(-40, 40));
            Destroy(f, 0.5f);
        }
        else
        {
            GameObject f = Instantiate(shootFlash, attackPoint.position, attackPoint.rotation);
            f.transform.Rotate(0, 0, lastRotation);
            lastRotation += 20;
            if (lastRotation >= 360) lastRotation -= 360;
            Destroy(f, 0.5f);
        }
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, Mathf.Infinity))
        {
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<Enemy>().Die();
            }
        }
        GameObject g = Instantiate(shootSound, transform.position, Quaternion.identity);
        Destroy(g, g.GetComponent<AudioSource>().clip.length);
    }

    void Update()
    {
        if (cd > 0) cd -= Time.deltaTime;

        float triggerValue = attackAction.action.ReadValue<float>();
        if (triggerValue > 0 && cd <= 0)
        {
            Shoot();
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 StartPosition = trail.transform.position;

        if (time < 1)
        {
            trail.transform.position = Vector3.Lerp(StartPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        //Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}
