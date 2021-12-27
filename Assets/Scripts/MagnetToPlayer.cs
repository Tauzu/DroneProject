using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetToPlayer : MonoBehaviour
{
    private GameObject targetObj;
    Rigidbody rbody;
    Vector3 relation;

    public float coeff = 100;

    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        rbody = targetObj.GetComponent<Rigidbody>();
        relation = this.transform.position - targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        relation = this.transform.position - targetObj.transform.position;
    }

    void FixedUpdate()
    {
        rbody.AddForce(relation.normalized * coeff / Mathf.Pow(relation.magnitude,2));
    }
}
