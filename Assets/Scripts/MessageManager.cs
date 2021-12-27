using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageManager : MonoBehaviour
{
    public Text message;
    private GameObject Player;
    public GameObject goalOB;
    public GameObject winOB;
    private float height;
    DroneMove DMscript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        DMscript = Player.GetComponent<DroneMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "stage01")
        {
            height = Player.transform.position.y;
            if(height > 55)
            {
                message.text = "「Rキー」で下降";
            }
            else if(height < 45)
            {
                message.text = "「Uキー」で上昇";
            }
            else
            {
                message.text = "「Hキー」でホバリングON/OFF";
                
                if(DMscript.isHovering)
                {
                    message.text = "「Wキー」で前進";
                }
            }
        }
        else
        {
             message.text = "";
        }
        

        if(winOB.activeSelf)
        {
            message.text = "クリア！\n「Nキー」で次へ";
        }
        else if(goalOB.activeSelf)
        {
            message.text = "ゴールせよ！";
        }

    }
}
