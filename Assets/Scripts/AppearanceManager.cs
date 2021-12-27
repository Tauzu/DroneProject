using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceManager : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject amezonPrefab;

    GameObject amezonClone;

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
                amezonClone = Instantiate(amezonPrefab);
                amezonClone.transform.position = playerObj.transform.position + new Vector3(0f, 10f, 2f);
                firstSetting = true;

            }

        }else{

            if(amezonClone == null){
                amezonClone = Instantiate(amezonPrefab);
                amezonClone.transform.position
                    = playerObj.transform.position + 2f*playerObj.transform.forward + 10f*Vector3.up;

            }else if(amezonClone.transform.position.y < -1f){
                Destroy(amezonClone);
            }


        }
        

    }

}
