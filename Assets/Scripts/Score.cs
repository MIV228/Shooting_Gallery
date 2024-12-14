using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public TMP_Text text;

    public GameObject[] weapons;

    public Animator animator;
    public AudioSource sound;

    private void Start()
    {
        score = 0;
        text.text = score.ToString();
    }

    public void AddScore()
    {
        score++;
        text.text = score.ToString();

        if (score == 15)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
            animator.Play("levelup");
            sound.Play();
        }
        else if (score == 50)
        {
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
            animator.Play("levelup");
            sound.Play();
        }
        else if (score == 100)
        {
            weapons[2].SetActive(false);
            weapons[3].SetActive(true);
            animator.Play("levelup");
            sound.Play();
        }
    }
}
