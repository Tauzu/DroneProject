using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    private float coefficient = 1;
    public float windSpeed;    // 風速
    private Vector3 relativeVelocity;    // 相対速度

    public GameObject gameObj;
    private Rigidbody rbody;//  Rigidbodyを使うための変数
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObj.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 相対速度計算
        // relativeVelocity = windSpeed*this.transform.forward - rbody.velocity;
        //rbodyは既にコンポーネント変数であり、gameObj.rbodyとすると誤り
    }

    void FixedUpdate()
    {
        // 空気抵抗を与える
        // rbody.AddForce(coefficient * relativeVelocity);
        rbody.AddForce(coefficient * windSpeed*this.transform.forward); //ゲームらしい風
    }
}
