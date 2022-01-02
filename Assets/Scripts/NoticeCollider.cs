using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeCollider : MonoBehaviour
{
    DragonBehavior DB;
    // Start is called before the first frame update
    void Start()
    {
        DB = this.GetComponentInParent<DragonBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpecialBullet")
        {
            DB.HitDamege();
        }
    }
}
