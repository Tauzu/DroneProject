using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstActivate : MonoBehaviour
{
    public GameObject[] activationObj;

    DroneMove DMScript;
    FPS_counter FPS_script;

    void Start()
    {
        DMScript = GameObject.Find("Player").GetComponent<DroneMove>();
        // DMScript.enabled = false;   //ドローン制御をオフにする(処理が安定するのを待つ)

        FPS_script = this.GetComponent<FPS_counter>();
    }

    void Update()
    {
        if(FPS_script.fps > 45f)
        {
            DMScript.enabled = true;   //FPSが45を超えたらドローン制御をオン

            for(int i=0; i<activationObj.Length; i++)
            {
                activationObj[i].SetActive(true);
            }

            Destroy(this);
        }


    }

}
