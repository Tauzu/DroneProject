using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScatter : MonoBehaviour
{
    private Rigidbody rbody;

    public string targetTag = "bullet";

    public GameObject Obj;
    public int numObj = 10;
    public float range = 1f;

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
        // var random = new System.Random();
        float delta = range / numObj;

        if (other.gameObject.tag == targetTag)
        {
            Destroy(other.gameObject);

            for(int i=0; i<numObj; i++)
            {
                GameObject clone = Instantiate(Obj);
                clone.transform.position = this.transform.position + new Vector3(Random.Range(-1f, 1f), delta*i, Random.Range(-1f, 1f));
                rbody = clone.GetComponent<Rigidbody>();
                rbody.isKinematic = false;
                var vect = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 2f), Random.Range(-1f, 1f));
                rbody.AddForce(10f*vect, ForceMode.VelocityChange);
                rbody.AddTorque(1000f*vect, ForceMode.VelocityChange);

                Destroy(clone, 5.0f);
            }

            Destroy(this.gameObject);

        }
    }
}
