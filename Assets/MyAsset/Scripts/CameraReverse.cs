using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�J�����̃��o�[�X��؂�ւ���B

public class CameraReverse : MonoBehaviour
{
    PlayerCamera playerCamera;
    public Text noticeText;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = this.GetComponent<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerCamera.reverce = !playerCamera.reverce;

            noticeText.text = (playerCamera.reverce) ? 
                "�J�������o�[�X�� ON �ɂȂ�܂����I" : "�J�������o�[�X�� OFF �ɂȂ�܂����I";

            timer = 3f;

        }

        if (timer > 0f) { timer -= Time.deltaTime; } else { noticeReset(); }
    }

    void noticeReset()
    {
        noticeText.text = "";
    }


}
