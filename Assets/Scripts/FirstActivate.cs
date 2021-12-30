using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstActivate : MonoBehaviour
{
    public GameObject[] activationObj;
    FPS_counter FPS_script;

    void Start()
    {
        Application.targetFrameRate = 60; //目標FPSを60に設定

        FPS_script = this.GetComponent<FPS_counter>();
    }

    void Update()
    {
        if(FPS_script.fps < 45f)
        {
            for(int i=0; i<activationObj.Length; i++)
            {
                activationObj[i].SetActive(false);
            }

        }
        else
        {
            for(int i=0; i<activationObj.Length; i++)
            {
                activationObj[i].SetActive(true);
            }

            Destroy(this);
        }


    }

}
