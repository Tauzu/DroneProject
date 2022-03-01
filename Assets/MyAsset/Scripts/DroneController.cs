using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�h���[���i���@�j�̑��c���s���X�N���v�g
//�L�[�{�[�h���͂��󂯕t���A����ɉ�����DroneMove�N���X�i�R���|�[�l���g�j��public�ϐ����X�V����

public class DroneController : MonoBehaviour
{
    Drone drone;

    //GameObject barrierObj;
    GameObject magnetObj;

    Transform cameraTf;

    // Start is called before the first frame update
    void Start()
    {
        drone = this.GetComponent<Drone>();

        //barrierObj = this.transform.Find("Barrier").gameObject;
        magnetObj = this.transform.Find("MagneticField").gameObject;

        cameraTf = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float inputForward = Input.GetAxis("W-S");
        //Debug.Log(inputForward);
        float inputSide = Input.GetAxis("D-A");
        Vector3 cameraFwd = cameraTf.forward;
        Vector3 cameraRgt = cameraTf.right;
        Vector2 cameraFwdXZ = new Vector2(cameraFwd.x, cameraFwd.z).normalized;
        Vector2 cameraRgtXZ = new Vector2(cameraRgt.x, cameraRgt.z).normalized;
        drone.targetVector = (inputForward * cameraFwdXZ + inputSide * cameraRgtXZ).normalized;

        drone.isBoosting = Input.GetKey(KeyCode.Q);   //�����t���O

        drone.inputVertical = Input.GetAxis("Ctrl-Shift_Left");

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    DM.Up();   //�㏸
        //}
        //else if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    DM.Down();   //���~
        //}

        if (Input.GetKeyDown(KeyCode.C)) drone.SwitchHovering(!drone.isHovering);

        if (Input.GetKeyDown(KeyCode.E)) magnetObj.SetActive(!magnetObj.activeSelf);

        //if (Input.GetKey(KeyCode.X)) DM.HeavyRotate();
        //barrierObj.SetActive(Input.GetKey(KeyCode.X));

    }

}
