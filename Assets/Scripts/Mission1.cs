using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission1 : MonoBehaviour
{
    Text message;
    public GameObject coinPrefab;
    GameObject goalObj;
    ClearProcess CP;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "[Shit(左)]長押しで上昇せよ！";

        CP = this.GetComponent<ClearProcess>();

        goalObj = Instantiate(coinPrefab);
        goalObj.transform.position = new Vector3(0f, 30f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(goalObj == null){
            CP.ClearNotify();
        }
    }
    
}
