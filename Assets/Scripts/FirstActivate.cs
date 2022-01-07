using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstActivate : MonoBehaviour
{
    public GameObject[] activationObj;

    FPS_counter FPS_script;
    GameObject startButton;

    void Start()
    {
        //playerObj = GameObject.Find("Player");
        // DMScript.enabled = false;   //ドローン制御をオフにする(処理が安定するのを待つ)

        FPS_script = GameObject.Find("GameController").GetComponent<FPS_counter>();

        startButton = transform.Find("TitleCanvas/StartButton").gameObject;
    }

    void Update()
    {
        if(FPS_script.fps > 45f)
        {
            startButton.SetActive(true);   //FPSが45を超えたらスタートボタン表示
        }

    }

    public void ActivateDroneAndMission()
    {

        for (int i = 0; i < activationObj.Length; i++)
        {
            activationObj[i].SetActive(true);
        }

        //DMScript.enabled = true;
        //playerObj.SetActive(true);

        Destroy(this.gameObject);
    }

}
