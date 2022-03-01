using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission3 : MonoBehaviour
{
    Text message;
    ClearProcess CP;
    public GameObject amezonPrefab;
    GameObject amezonClone;
    ScoreManager ScoreMan;
    Transform playerTf;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "[E]を押すと磁場発生。\n 宅配物を届けよう！\n目標SCORE:6000";

        CP = this.GetComponent<ClearProcess>();

        playerTf = GameObject.Find("Player").transform;

        ScoreMan = GameObject.Find("GameController").GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (ScoreMan.score >= 6000)
        {
            message.text = "";
            CP.ClearNotify();
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
