using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceManager : MonoBehaviour
{
    public GameObject playerObj;
    FPS_counter FPS_script;

    bool firstSetting = false;

    void Start()
    {
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
                firstSetting = true;
            }

        }

    }

}
