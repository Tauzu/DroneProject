using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2 : Mission
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        message.text = "空中で[C]を押し、ホバリング。\n [WASD]で移動してコインを\n集めよう！";

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (GameObject.FindGameObjectsWithTag("coin").Length == 0){ complete = true; }
    }
}
