using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject particles;

    public int hp;
    public int score;

    public bool boss;
    public bool active;

    public Animator animator;

    public int nextAttack;
    public float attackCD;

    public bool canSpawn;
    public GameObject spawn;
    public Transform spawnPoint;

    public GameObject projectile;

    public Transform shootPoint;

    public bool snowman;
    public Enemy parent;


    private void Start()
    {
        if (boss)
        {
            animator = GetComponent<Animator>();
            attackCD = 5;
        }
    }

    public void Shoot()
    {
        Quaternion q = shootPoint.rotation;
        shootPoint.Rotate(Vector3.up, -15);
        for (int i = 0; i < 5; i++)
        {
            Enemy e = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Enemy>();
            e.transform.Translate(shootPoint.forward * 3);
            e.SetupProjectile(shootPoint.forward, 10);
            shootPoint.Rotate(Vector3.up, 10);
        }
        shootPoint.rotation = q;
    }

    public void Spawn()
    {
        Enemy e = Instantiate(spawn, spawnPoint.position, spawnPoint.rotation).GetComponent<Enemy>();
        e.parent = this;
    }

    public void ShootOnce()
    {
        shootPoint.LookAt(Camera.main.transform);
        Enemy e = Instantiate(projectile, shootPoint.position, shootPoint.rotation).GetComponent<Enemy>();
        e.SetupProjectile(shootPoint.forward, 7);
    }

    public void SetupProjectile(Vector3 dir, float speed)
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().AddForce(speed * dir, ForceMode.Impulse);
        GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(ProjectileAnimation());
        active = true;
    }

    IEnumerator ProjectileAnimation()
    {
        transform.Rotate(transform.forward, 180 * Time.deltaTime);
        yield return null;
    }

    public void Hurt()
    {
        hp -= 1;

        if (boss)
        {
            FindObjectOfType<Score>().AddScore();
            if (FindObjectOfType<Score>().score >= 800)
            {
                animator.Play("death");
                nextAttack = -1;
            }

            return;
        }

        if (hp <= 0)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Score s = FindObjectOfType<Score>();
            for (int i = 0; i < score; i++)
            {
                s.AddScore();
            }
            if (snowman) parent.canSpawn = true;
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (boss && active)
        {
            if (nextAttack == -1) return;

            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                if (!canSpawn) nextAttack = 1;
                else
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        nextAttack = 2;
                        canSpawn = false;
                    }
                    else nextAttack = 1;
                }
                attackCD = 5;

                if (nextAttack == 1) animator.SetTrigger("shoot");
                else animator.SetTrigger("spawn");
            }
        }
        else if (snowman)
        {
            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                ShootOnce();
                attackCD = 3;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<Score>().Deduct();
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (active && other.tag != "Enemy")
        {
            Hurt();
        }
    }
}
