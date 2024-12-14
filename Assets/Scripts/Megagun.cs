using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Megagun : MonoBehaviour
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

    public GameObject impact;
    public GameObject impactMetal;

    public ParticleSystem particles;

    public bool dontAnimate;
    float lastRotation;

    float lastAttackRotation;
    bool goingBack;

    public void Shoot()
    {
        cd = maxcd;
        GameObject f = Instantiate(shootFlash, attackPoint.position, attackPoint.rotation);
        f.transform.Rotate(0, 0, lastRotation);
        lastRotation += 30;
        if (lastRotation >= 360) lastRotation -= 360;
        Destroy(f, 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, Mathf.Infinity))
        {
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<Enemy>().Hurt();
                if (hit.collider.gameObject.GetComponent<Enemy>().hp > 0 && !hit.collider.gameObject.GetComponent<Enemy>().boss) Instantiate(impactMetal, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        attackPoint.Rotate(attackPoint.up, lastAttackRotation);
        RaycastHit hit1;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit1, Mathf.Infinity))
        {
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit1));

            if (hit1.collider.gameObject.tag == "Enemy")
            {
                hit1.collider.gameObject.GetComponent<Enemy>().Hurt();
                if (hit1.collider.gameObject.GetComponent<Enemy>().hp > 0) Instantiate(impactMetal, hit1.point, Quaternion.LookRotation(hit1.normal));
            }
            else
            {
                Instantiate(impact, hit1.point, Quaternion.LookRotation(hit1.normal));
            }
        }
        attackPoint.localRotation = Quaternion.identity;
        attackPoint.Rotate(attackPoint.up, -lastAttackRotation);
        RaycastHit hit2;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit2, Mathf.Infinity))
        {
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit2));

            if (hit2.collider.gameObject.tag == "Enemy")
            {
                hit2.collider.gameObject.GetComponent<Enemy>().Hurt();
                if (hit2.collider.gameObject.GetComponent<Enemy>().hp > 0) Instantiate(impactMetal, hit2.point, Quaternion.LookRotation(hit2.normal));
            }
            else
            {
                Instantiate(impact, hit2.point, Quaternion.LookRotation(hit2.normal));
            }
        }
        attackPoint.localRotation = Quaternion.identity;
        if (!goingBack) lastAttackRotation += 2.5f;
        else lastAttackRotation -= 2.5f;
        if (lastAttackRotation >= 25) goingBack = true;
        else if (lastAttackRotation <= 0) goingBack = false;
        GameObject g = Instantiate(shootSound, transform.position, Quaternion.identity);
        Destroy(g, g.GetComponent<AudioSource>().clip.length);
    }

    void Update()
    {
        if (cd > 0) cd -= Time.deltaTime;

        float triggerValue = attackAction.action.ReadValue<float>();
        if (triggerValue > 0)
        {
            if (cd <= 0) Shoot();
            particles.emissionRate = 60;
        }
        else
        {
            particles.emissionRate = 0;
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
