using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_counter : MonoBehaviour
{
    // 変数
    int frameCount;
    float prevTime;
    
    float fps;

    // 初期化処理
    void Start()
    {
        Application.targetFrameRate = 60; //目標FPSを60に設定
        frameCount = 0;
        prevTime = 0.0f;
    }
    // 更新処理
    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;
            // Debug.Log(fps);

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }

    }

    public float get_FPS()
    {
        return fps;
    }

    // 表示処理
    // private void OnGUI()
    // {
    //     GUILayout.Label(fps.ToString());
    // }

}
