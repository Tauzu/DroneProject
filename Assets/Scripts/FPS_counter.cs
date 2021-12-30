using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_counter : MonoBehaviour
{
    // 変数
    int frameCount;
    float prevTime;
    
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public float fps;

    // 初期化処理
    void Start()
    {
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

    // 表示処理
    // private void OnGUI()
    // {
    //     GUILayout.Label(fps.ToString());
    // }

}
