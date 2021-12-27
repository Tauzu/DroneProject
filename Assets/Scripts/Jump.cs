using System.Collections.Generic;
using UnityEngine;
 
public class Jump : MonoBehaviour
{
    private Rigidbody rb;//  Rigidbodyを使うための変数
    private bool Grounded;//  地面に着地しているか判定する変数
    public float Jumppower;//  ジャンプ力
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();//  rbにRigidbodyの値を代入する
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Grounded == true)//  もし、Groundedがtrueなら、
        {
            if (Input.GetKeyDown(KeyCode.Z))//  もし、キーがおされたなら、  
            {
                Grounded = false;//  Groundedをfalseにする
                rb.AddForce(Vector3.up * Jumppower);//  上にJumpPower分力をかける
            }
        }
    }
 
    void OnCollisionEnter(Collision other)//  地面に触れた時の処理
    {
        if (other.gameObject.tag == "Ground")//  もしGroundというタグがついたオブジェクトに触れたら、
        {
            Grounded = true;//  Groundedをtrueにする
        }
    }
}
