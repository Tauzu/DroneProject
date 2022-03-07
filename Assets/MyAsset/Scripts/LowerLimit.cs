using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLimit : MonoBehaviour
{
    Rigidbody rbody;
    Drone drone;
    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        drone = this.GetComponent<Drone>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rbody.position.y < -1){
            drone.SwitchHovering(false);
            rbody.AddForce(Vector3.up * 1000f);
        }
    }
}
