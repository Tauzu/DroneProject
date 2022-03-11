using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission4 : Mission
{
    GameObject firstEnemy;
    GameObject enemyShip;

    int phase = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        message.text = "上空に敵出現！\n[Enter]で視点切替。\n[Z]で弾丸発射。";

        firstEnemy = transform.Find("FirstEnemy").gameObject;
        firstEnemy.SetActive(true);
        phase = 1;

        enemyShip = transform.Find("EnemyShip").gameObject;

        audioSrc.Stop();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (firstEnemy == null && phase == 1)
        {
            enemyShip.SetActive(true);
            message.text = "敵発生装置出現！\n発見し破壊せよ！\n敵も駆逐せよ！";
            phase = 2;

        }
        else if(enemyShip == null && phase == 2){ complete = true; }

    }
}
