using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission1 : MonoBehaviour
{
    Text message;
    GameObject coinObj;
    ClearProcess CP;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "[Shit(左)]長押しで上昇せよ！";

        coinObj = this.transform.Find("Coin").gameObject;

        CP = this.GetComponent<ClearProcess>();

    }

    // Update is called once per frame
    void Update()
    {
        if(coinObj == null){
            CP.ClearNotify();
        }
    }
    
}
