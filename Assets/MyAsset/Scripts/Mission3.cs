using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//宅配物を届けるミッション
//ひとつ届けると1000点獲得。累計スコア6000点でクリア
//ミッション中、宅配物を定期的に発生させる

public class Mission3 : Mission
{
    public GameObject amezonPrefab;
    GameObject amezonClone;
    ScoreManager ScoreMan;
    Transform playerTf;

    IEnumerator enumerator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        message.text = "[E]を押すと磁場発生。\n 宅配物を届けよう！\n目標SCORE:6000";

        playerTf = GameObject.Find("PlayerDrone").transform;

        ScoreMan = GameObject.Find("GameController").GetComponent<ScoreManager>();

        enumerator = MainCoroutine();
        StartCoroutine(enumerator);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (ScoreMan.score >= 6000) {
            StopCoroutine(enumerator);
            complete = true;
        }

    }

    //宅配物を定期的に発生させるコルーチン
    IEnumerator MainCoroutine()
    {
        while (true)
        {
            if (amezonClone == null)
            {
                yield return new WaitForSeconds(1f);
                amezonClone = Instantiate(amezonPrefab);
                amezonClone.transform.position
                    = playerTf.position + 2f * playerTf.forward + 5f * Vector3.up;

            }
            else if (amezonClone.transform.position.y < -1f)
            {
                Destroy(amezonClone);
            }

            yield return new WaitForSeconds(1f);

        }

    }

}
