using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class BuildingCounter : MonoBehaviour
{
    int[] numBuilding = new int[2];
    ReloadScene reload;
    Text warningText;
    float timeCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        reload = GetComponent<ReloadScene>();
        warningText = GameObject.Find("WarningText").GetComponent<Text>();
        warningText.text = "";

        numBuilding[1] = GameObject.FindGameObjectsWithTag("Building").Length;
        numBuilding[0] = numBuilding[1];
    }

    // Update is called once per frame
    void Update()
    {
        numBuilding[1] = GameObject.FindGameObjectsWithTag("Building").Length;
        if (numBuilding[1] < numBuilding[0])
        {
            warningText.text = "建物が破壊された！\n残り：" + numBuilding[1].ToString();
            timeCounter = 5f;
        }

        if(numBuilding[1]==0) reload.ReloadNowScene();
        numBuilding[0] = numBuilding[1];
        
        timeCounter -= Time.deltaTime;
        if(timeCounter <= 0f) warningText.text = "";
    }

}
