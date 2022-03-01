using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public int score = 0;

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public int numBuilding;

    public GameObject gameOverObj;

    //ReloadScene reload;

    // Start is called before the first frame update
    void Start()
    {
        //reload = GetComponent<ReloadScene>();
    }

    // Update is called once per frame
    void Update()
    {
        numBuilding = GameObject.FindGameObjectsWithTag("Building").Length;
        if (numBuilding == 0)
        {
            gameOverObj.SetActive(true);
            Destroy(this);
        }
    }
}
