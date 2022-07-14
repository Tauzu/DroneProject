using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//宅配物（AmezonBox）クラス
//町の建物の中からひとつランダムに選び、そこを目的地とする。

public class AmezonBoxBehavior : MonoBehaviour
{
    GameObject targetObj;

    GameObject effectClone;
    Transform playerTf;
    public GameObject scorePrefab;

    public bool special = false;

    // Start is called before the first frame update
    void Start()
    {
        //目的地にパーティクル（目印かつ当たり判定）を配置する。
        GameObject targetEffect = (GameObject)Resources.Load("TargetParticle");
        effectClone = Instantiate(targetEffect);
        // effectClone.transform.parent = this.transform;

        playerTf = GameObject.Find("PlayerDrone").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetObj == null)
        {
            // targetObj = GameObject.FindWithTag("Building");
            GameObject[] Buildings = GameObject.FindGameObjectsWithTag ("Building");

            if (Buildings.Length > 0)
            {
                targetObj = Buildings[Random.Range(0, Buildings.Length)];

                effectClone.transform.position = targetObj.transform.position + new Vector3(0f, 1f, 0f);

            }

        }

        Vector3 direction = this.transform.position - playerTf.position;
        if(direction.magnitude > 50f){
            Destroy(this.gameObject);
        }

    }

    //void OnCollisionEnter(Collision other)//  他のオブジェクトに触れた時の処理
    //{
    //    if (other.gameObject == effectClone)
    //    {
    //        Success();
    //    }

    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == effectClone)
        {
            Success();
        }

    }

    //目的地に触れた（宅配完了）時の処理
    void Success()
    {
        GameObject clone = Instantiate(scorePrefab) as GameObject;
        clone.GetComponent<PopupScore>().SetScore(1000, this.transform.position + Vector3.up);

        if (special)
        {
            Transform playerTf = GameObject.Find("PlayerDrone").transform;
            Transform SAParticleTf = this.transform.Find("SpecialAmezonParticle");
            SAParticleTf.parent = playerTf;
            SAParticleTf.localPosition = Vector3.zero;
        }

        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        Destroy(effectClone);
    }

}
