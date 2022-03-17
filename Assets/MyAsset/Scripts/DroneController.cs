using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ドローン（自機）の操縦を行うスクリプト
//キーボード入力を受け付け、それに応じてDroneクラス（コンポーネント）のpublic関数を呼び出す

public class DroneController : MonoBehaviour
{
    Drone drone;

    Transform cameraTf;

    // Start is called before the first frame update
    void Start()
    {
        drone = this.GetComponent<Drone>();

        cameraTf = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float inputForward = Input.GetAxis("W-S");
        float inputSide = Input.GetAxis("D-A");
        Vector3 cameraFwd = cameraTf.forward;
        Vector3 cameraRgt = cameraTf.right;
        Vector2 cameraFwdXZ = new Vector2(cameraFwd.x, cameraFwd.z).normalized;
        Vector2 cameraRgtXZ = new Vector2(cameraRgt.x, cameraRgt.z).normalized;
        drone.SetTargetVector((inputForward * cameraFwdXZ + inputSide * cameraRgtXZ).normalized);
        //Debug.Log(drone.targetVector);

        drone.SetBoosting(Input.GetKey(KeyCode.Q));

        drone.SetTargetVeritical(Input.GetAxis("Ctrl-Shift_Left"));

        if (Input.GetKeyDown(KeyCode.C)) drone.SwitchHovering(!drone.IsHovering());

        if (Input.GetKeyDown(KeyCode.E)) drone.magnetObj.SetActive(!drone.magnetObj.activeSelf);

        drone.barrierObj.SetActive(Input.GetKey(KeyCode.X));

    }

}
