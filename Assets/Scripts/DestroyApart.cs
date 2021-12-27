using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyApart : MonoBehaviour
{
    private Rigidbody[] rbodys;

    public string targetTag;

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
        var random = new System.Random();
        var min = -10;
        var max = 10;

        if (other.gameObject.tag == targetTag)
        {
            foreach(Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }

            //　全ての子のコンポーネントを取得
		    rbodys = GetComponentsInChildren<Rigidbody>();
            
            //　取得したコンポーネント全てに対してループ
            foreach(var rbody in rbodys)
            {
                rbody.isKinematic = false;
                rbody.transform.SetParent(null);
                var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
                rbody.AddForce(vect, ForceMode.Impulse);
                rbody.AddTorque(100f*vect, ForceMode.Impulse);
            }
            Destroy(this.gameObject);

            // gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            //     r.isKinematic = false;
            //     r.transform.SetParent(null);
            //     r.gameObject.AddComponent<AutoDestroy>().time = 2f;
            //     var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            //     r.AddForce(vect, ForceMode.Impulse);
            //     r.AddTorque(vect, ForceMode.Impulse);
            // });
            // Destroy(gameObject);
        }
    }

}
