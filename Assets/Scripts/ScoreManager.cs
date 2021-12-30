using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class ScoreManager : MonoBehaviour
{
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public int score = 0;
    public GameObject scoreTextObj;
    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreTextObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE:" + score.ToString().PadLeft(8);
    }
}
