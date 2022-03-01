using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームオーバー時の処理
public class GameOver : MonoBehaviour
{
    public GameObject missionObj;
    public Light directionalLight;
    public AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        directionalLight.color = Color.red;
        Destroy(missionObj);
        Camera.main.enabled = false;
        audioSrc.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
