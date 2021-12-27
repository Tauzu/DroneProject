using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollider : MonoBehaviour
{
    public GameObject winnerLabelObject;

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
        if (other.gameObject.tag == "Player")//  もしプレイヤーブジェクトに触れたら、
        {
            winnerLabelObject.SetActive(true);
        }
    }

}
