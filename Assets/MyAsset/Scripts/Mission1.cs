using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミッション1
//ただ上昇するだけ

public class Mission1 : Mission
{
    GameObject coinObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        message.text = "[Shit(左)]長押しで上昇せよ！";

        coinObj = this.transform.Find("Coin").gameObject;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (coinObj == null){ complete = true; }
    }
    
}
