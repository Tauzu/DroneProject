using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵に弾丸が当たった時の処理。
//スコア加算と破片飛散演出


public class DestroyEnemy : DestroyScatter
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)//  他のオブジェクトに触れた時の処理
    {
        foreach(string tag in destroyTag)
        {
            if (other.gameObject.tag == tag)
            {
                Destroy(other.gameObject);
                Defeat();
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        foreach(string tag in destroyTag)
        {
            if (other.gameObject.tag == tag)
            {
                Defeat();
            }
        }
    }

    void PopupScore()
    {
        GameObject cameraObj = GameObject.Find("Main Camera");
        Vector3 direction = this.transform.position - cameraObj.transform.position;
        float distance = direction.magnitude;
        int score = (int)(distance * distance / 1000f) * 100;
        score = Mathf.Clamp(score, 100, score);

        GameObject scoreObj = Instantiate((GameObject)Resources.Load("Score3DText"));
        scoreObj.GetComponent<PopupScore>().SetScore(score, this.transform.position + Vector3.up);

    }

    void Defeat()
    {
        PopupScore();
        ScatterAndDestroy();
    }


}
