using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    GameObject barrierObj;
    GameObject magnetObj;
    // Start is called before the first frame update
    void Start()
    {
        barrierObj = this.transform.Find("Barrier").gameObject;
        magnetObj = this.transform.Find("MagneticField").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) magnetObj.SetActive(!magnetObj.activeSelf);

        barrierObj.SetActive(Input.GetKey(KeyCode.X));
        
    }


}
