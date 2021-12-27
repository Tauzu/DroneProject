using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroMagnet : MonoBehaviour
{
    private GameObject[] magneticObjs = {};
    Rigidbody[] magneticRbodys = {};
    Rigidbody rbody;
    Vector3[] relation = {};

    public float coeff = 100;

    public bool magnetOn;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < magneticObjs.Length; i++)
        {
            if(magneticObjs[i] != null)
            {
                relation[i] = magneticObjs[i].transform.position - this.transform.position;
            }

        }
        
        if(Input.GetKeyDown(KeyCode.M))   //ONOFFスイッチ
        {
            magnetOn = (magnetOn)? false : true;

            GameObject magnetLight = transform.Find("Point Light").gameObject;
            magnetLight.SetActive(magnetOn);
        }

        count += 1;
        if(magnetOn && (count % 10 == 0))
        {
            resetArray();
        }
        
    }

    void FixedUpdate()
    {
        if(magnetOn)
        {
            for (int i = 0; i < magneticRbodys.Length; i++)
            {
                if(magneticObjs[i] != null)
                {
                    Vector3 power = relation[i].normalized * coeff / Mathf.Pow(relation[i].magnitude,2);
                    magneticRbodys[i].AddForce(-power);
                    rbody.AddForce(power);
                }

            }
        }
    }

    void resetArray()
    {
        magneticObjs = GameObject.FindGameObjectsWithTag ("Magnetic");
        magneticRbodys = new Rigidbody[magneticObjs.Length];
        relation = new Vector3[magneticObjs.Length];
        for (int i = 0; i < magneticObjs.Length; i++)
        {
            magneticRbodys[i] = magneticObjs[i].GetComponent<Rigidbody>();
            relation[i] = magneticObjs[i].transform.position - this.transform.position;
        }
    }
}
