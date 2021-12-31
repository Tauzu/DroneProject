using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission4 : MonoBehaviour
{
    Text message;
    ClearProcess CP;
    GameObject firstEnemy;
    GameObject enemyShip;
    Transform playerTf;

    int phase = 0;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "上空に敵出現！\n[Shift(右)]を2連続押しで視点切替。\n[Z]で弾丸発射。";

        CP = this.GetComponent<ClearProcess>();

        playerTf = GameObject.Find("Player").transform;

        firstEnemy = transform.Find("FirstEnemy").gameObject;
        firstEnemy.SetActive(true);
        phase = 1;

        enemyShip = transform.Find("EnemyShip").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (firstEnemy == null && phase == 1)
        {
            enemyShip.SetActive(true);
            message.text = "敵艦隊出現！\n駆逐せよ！";
            phase = 2;

        }
        else if(enemyShip == null && phase == 2)
        {
            CP.ClearNotify();
        }

    }
}
