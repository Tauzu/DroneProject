using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeCollider : MonoBehaviour
{
    DragonBehavior DB;
    //int count=0;
    float hitLimit = 0f;

    //Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {
        DB = this.GetComponentInParent<DragonBehavior>();
        //rbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        hitLimit -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //rbody.AddForce(this.transform.forward);
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "SpecialBullet" && hitLimit < 0f)
        //{
        //    //count++;
        //    //Debug.Log(count);
        //    float speed = other.attachedRigidbody.velocity.magnitude;
        //    DB.HitDamage(Mathf.Max((int)(speed*speed*0.0001f), 1));
        //    hitLimit = 0.5f;
        //}
    }
}
