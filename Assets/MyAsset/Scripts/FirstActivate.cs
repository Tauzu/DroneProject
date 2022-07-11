using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム開始（Startボタンクリック）時に指定ゲームオブジェクトをActivateする

public class FirstActivate : MonoBehaviour
{
    public GameObject[] activationObj;

    //public GameObject PlayerPrefab;
    //public int numClone;

    public FPS_counter fps_counter;
    GameObject startButton;

    public AudioSource audioSrc;
    public AudioClip titleBGM;


    void Start()
    {
        //playerObj = GameObject.Find("Player");
        // drone.enabled = false;   //ドローン制御をオフにする(処理が安定するのを待つ)

        startButton = transform.Find("TitleCanvas/StartButton").gameObject;

    }

    void Update()
    {
        if(fps_counter.get_FPS() > 30f && !startButton.activeSelf)
        {
            startButton.SetActive(true);   //FPSが基準値を超えたらスタートボタン表示

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


        //drone.enabled = true;
        //playerObj.SetActive(true);

        Destroy(this.gameObject);
    }

}
