using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    DroneMove DM;

    // Start is called before the first frame update
    void Start()
    {
        DM = this.GetComponent<DroneMove>();
    }

    // Update is called once per frame
    void Update()
    {
        DM.inputForward = Input.GetAxis("W-S");
        DM.inputSide = Input.GetAxis("D-A");
        DM.isBoosting = Input.GetKey(KeyCode.Q);   //â¡ë¨ÉtÉâÉO

        if (Input.GetKey(KeyCode.LeftShift)) DM.Up();   //è„è∏
        if (Input.GetKey(KeyCode.LeftControl)) DM.Down();   //â∫ç~

        if (Input.GetKeyDown(KeyCode.C)) DM.SwitchHovering();
        if (Input.GetKey(KeyCode.X)) DM.HeavyRotate();
    }
}
