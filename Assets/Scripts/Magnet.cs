using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public GameObject targetObj;
    Rigidbody rbody;
    Rigidbody targetRbody;
    Vector3 relation;

    public float coeff = 100;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        targetRbody = targetObj.GetComponent<Rigidbody>();
        relation = this.transform.position - targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        relation = this.transform.position - targetObj.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 power = relation.normalized * coeff / Mathf.Pow(relation.magnitude,2);
        rbody.AddForce(-power);
        targetRbody.AddForce(power);
    }
}
