using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class ScoreManager : MonoBehaviour
{
    public Text scoreLabel;
    // public GameObject winnerLabelObject;
    public GameObject goalObject;

    AudioSource audioSource;
    public AudioClip getItemSE;
    public AudioClip activeGoalSE;

    private int norma;
    private int gotten;
    private int[] count = new int[2];
    private bool goal_activated;

    // Start is called before the first frame update
    void Start()
    {
        count[0] = GameObject.FindGameObjectsWithTag("item").Length;
        audioSource = this.GetComponent<AudioSource>();
        norma = count[0];
    }

    // Update is called once per frame
    void Update()
    {
        count[1] = GameObject.FindGameObjectsWithTag("item").Length;
        gotten = norma - count[1];
        scoreLabel.text = gotten.ToString() + "/" + norma.ToString();

        if(count[1] != count[0])
        {
            audioSource.PlayOneShot(getItemSE);
        }

        if((count[1] == 0) && !(goal_activated))
        {
            // winnerLabelObject.SetActive(true);
            goalObject.SetActive(true);
            audioSource.PlayOneShot(activeGoalSE);
            goal_activated = true;

        }

        count[0] = count[1];
        
    }
}
