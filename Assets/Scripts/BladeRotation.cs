using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotation : MonoBehaviour
{
    public int ID;
    // public float coeff;
    private Rigidbody rbody;//  Rigidbodyを使うための変数
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((ID==1 && Input.GetKey(KeyCode.E))
            ||(ID==2 && Input.GetKey(KeyCode.D))
            ||(ID==3 && Input.GetKey(KeyCode.H))
            ||(ID==4 && Input.GetKey(KeyCode.U)))
        {
            // rbody.AddRelativeTorque(new Vector3(0, coeff, 0));
            transform.Rotate(new Vector3(0, 10, 0));
        }
    }
}
