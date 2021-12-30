using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceManager : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject firstObj;
    FPS_counter FPS_script;

    bool firstSetting = false;

    void Start()
    {
        Application.targetFrameRate = 60; //目標FPSを60に設定

        FPS_script = this.GetComponent<FPS_counter>();
    }

    void Update()
    {
        if(!firstSetting){
            if(FPS_script.fps < 45f){
                playerObj.SetActive(false);

            }
            else{
                playerObj.SetActive(true);
                firstObj.SetActive(true);
                firstSetting = true;
            }

        }

    }

}
