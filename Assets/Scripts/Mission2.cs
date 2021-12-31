using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission2 : MonoBehaviour
{
    Text message;
    public GameObject goalObj;
    ClearProcess CP;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "空中で[C]を押し、ホバリング。\n [WASD]で移動してコインを集めよ！";

        CP = this.GetComponent<ClearProcess>();

        goalObj.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("coin").Length == 0){
            CP.ClearNotify();
        }
    }
}
