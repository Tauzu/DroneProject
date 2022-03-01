using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���͏�N���X�B
//���͈͓��̃^�O�t���I�u�W�F�N�g���擾���A�����ɉ��������͂�^����B

public class ElectroMagnet : MonoBehaviour
{
    Rigidbody rbody;
    List<Rigidbody> magneticList;
    public float coeff = 100;

    void OnEnable()
    {
        magneticList = new List<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rbody = this.transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(magneticList.Count);

    }

    void FixedUpdate()
    {
        magneticList.RemoveAll(item => item == null);
        float power;
        float distance;
        Vector3 direction;
        Vector3 force;
        foreach (Rigidbody magneticRbody in magneticList)
        {
            direction = magneticRbody.transform.position - this.transform.position;
            distance = direction.magnitude;
            power = Mathf.Clamp(coeff / (distance*distance), 1f, 50f);
            force = direction.normalized * power;
            magneticRbody.AddForce(-force);
            rbody.AddForce(force);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Magnetic")
        {
            magneticList.Add(other.GetComponent<Rigidbody>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(magneticList.Contains(rb))
        {
            magneticList.Remove(rb);
        }
    }

}
