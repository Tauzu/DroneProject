using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class ScoreUI : MonoBehaviour
{
    Text scoreText;
    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE:" + scoreManager.score.ToString().PadLeft(8);
    }
}
