using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission3 : Mission
{
    public GameObject amezonPrefab;
    GameObject amezonClone;
    ScoreManager ScoreMan;
    Transform playerTf;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        message.text = "[E]を押すと磁場発生。\n 宅配物を届けよう！\n目標SCORE:6000";

        playerTf = GameObject.Find("Player").transform;

        ScoreMan = GameObject.Find("GameController").GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (ScoreMan.score >= 6000)
        {
            complete = true;
        }
        else
        {
            if(amezonClone == null){
                amezonClone = Instantiate(amezonPrefab);
                amezonClone.transform.position
                    = playerTf.position + 2f*playerTf.forward + 5f*Vector3.up;

            }else if(amezonClone.transform.position.y < -1f){
                Destroy(amezonClone);
            }
        }
        // if(GameObject.FindGameObjectsWithTag("coin").Length == 0){
        //     CP.ClearNotify();
        // }
    }
}
