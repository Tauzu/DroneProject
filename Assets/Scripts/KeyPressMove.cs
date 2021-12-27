using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressMove : MonoBehaviour
{
    private Rigidbody rbody;//  Rigidbodyを使うための変数
    float fx = 0;

    float fy = 0;

    float fz = 0;
    public float coeff = 10;


    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        fx = Input.GetAxisRaw("Horizontal") * coeff;
        fz = Input.GetAxisRaw("Vertical") * coeff;
        if (Input.GetKey(KeyCode.Z))//  もし、Zキーがおされているなら、  
        {
            fy = 2*coeff;
        }else{
            fy = 0;
        }

        rbody.AddForce(new Vector3(fx,fy,fz));

    }
}
