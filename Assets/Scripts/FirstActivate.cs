using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstActivate : MonoBehaviour
{
    public GameObject[] activationObj;

    //public GameObject PlayerPrefab;
    //public int numClone;

    FPS_counter FPS_script;
    GameObject startButton;

    public AudioSource audioSrc;
    public AudioClip titleBGM;


    void Start()
    {
        //playerObj = GameObject.Find("Player");
        // DMScript.enabled = false;   //ドローン制御をオフにする(処理が安定するのを待つ)

        FPS_script = GameObject.Find("GameController").GetComponent<FPS_counter>();

        startButton = transform.Find("TitleCanvas/StartButton").gameObject;

    }

    void Update()
    {
        if(FPS_script.fps > 45f && !startButton.activeSelf)
        {
            startButton.SetActive(true);   //FPSが45を超えたらスタートボタン表示

            audioSrc.clip = titleBGM;
            audioSrc.Play();
        }

    }

    public void ActivateDroneAndMission()
    {

        for (int i = 0; i < activationObj.Length; i++)
        {
            activationObj[i].SetActive(true);
        }

        //for (int i = 0; i < numClone; i++)
        //{
        //    GameObject clone = Instantiate(PlayerPrefab);
        //    clone.transform.position += 2*(i+1)*Vector3.forward;

        //}


        //DMScript.enabled = true;
        //playerObj.SetActive(true);

        Destroy(this.gameObject);
    }

}
