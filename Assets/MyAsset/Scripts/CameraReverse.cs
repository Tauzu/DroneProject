using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//カメラのリバースを切り替える。

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
                "カメラリバースが ON になりました！" : "カメラリバースが OFF になりました！";

            timer = 3f;

        }

        if (timer > 0f) { timer -= Time.deltaTime; } else { noticeReset(); }
    }

    void noticeReset()
    {
        noticeText.text = "";
    }


}
