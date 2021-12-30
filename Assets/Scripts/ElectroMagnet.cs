using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroMagnet : MonoBehaviour
{
    // private GameObject[] magneticObjs = {};
    Rigidbody[] magneticRBodys = {};
    Rigidbody rbody;
    Vector3[] relation = {};

    public float coeff = 100;

    public bool magnetOn;
    GameObject magnetLight;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        magnetLight = this.transform.Find("Point Light").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < magneticRBodys.Length; i++)
        {
            if(magneticRBodys[i] != null)
            {
                relation[i] = magneticRBodys[i].transform.position - this.transform.position;
            }

        }
        
        if(Input.GetKeyDown(KeyCode.E))   //ONOFFスイッチ
        {
            magnetOn = (magnetOn)? false : true;
            magnetLight.SetActive(magnetOn);
        }

        count += 1;
        if(magnetOn && (count % 20 == 0))
        {
            resetArray();
        }
        
    }

    void FixedUpdate()
    {
        if(magnetOn)
        {
            float power;
            Vector3 force;
            for (int i = 0; i < magneticRBodys.Length; i++)
            {
                if(magneticRBodys[i] != null)
                {
                    power = Mathf.Clamp(coeff / Mathf.Pow(relation[i].magnitude, 2), 0f, 50f);
                    // if(power>100f) Debug.Log(power);
                    force = relation[i].normalized * power;
                    magneticRBodys[i].AddForce(-force);
                    rbody.AddForce(force);
                }

            }
        }
    }

    void resetArray()
    {
        GameObject[] magneticObjs = GameObject.FindGameObjectsWithTag ("Magnetic");
        magneticRBodys = new Rigidbody[magneticObjs.Length];
        relation = new Vector3[magneticObjs.Length];
        for (int i = 0; i < magneticObjs.Length; i++)
        {
            magneticRBodys[i] = magneticObjs[i].GetComponent<Rigidbody>();
            relation[i] = magneticObjs[i].transform.position - this.transform.position;
        }
    }
}
