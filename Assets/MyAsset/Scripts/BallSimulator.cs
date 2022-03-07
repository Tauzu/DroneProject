using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//弾道シミュレータ。
//予測軌道をLineRendererで描画する。

public class BallSimulator : MonoBehaviour {

    private const int SIMULATE_COUNT = 25; // シュミレートする点の個数

    Vector3[] simuratePointArray; // シュミレートするpoint_array

    LineRenderer lineRenderer;
    public Gradient grad;

    float timer = 0f;

    void Start () {

        simuratePointArray = new Vector3[SIMULATE_COUNT];
        for (int i = 0; i < SIMULATE_COUNT; i++)
        {
            simuratePointArray[i] = Vector3.down * 100f;
        }

        lineRenderer = this.GetComponent<LineRenderer>();
        // 点の数を指定する
        lineRenderer.positionCount = simuratePointArray.Length;
        lineRenderer.SetPositions(simuratePointArray);
    }
	
    void Update () {
        if(timer > 0f) { timer -= Time.deltaTime; } else { lineRenderer.enabled = false; }
    }

    //外部スクリプト（Shooting.cs）からこの関数を呼び出して使う
    //引数は、弾の初期位置と初速度、およびレンダリングの色の度合い
    public void Simulate(Vector3 initialPosition , Vector3 initialVelocity, float gradLocation)
    {
        lineRenderer.enabled = true;

        //軌道を2次曲線として描く。
        for (int i = 0; i < SIMULATE_COUNT; i++)
        {
            float t = i * 0.2f + 0.05f; // 0.2秒ごとの位置を予測。
            Vector3 predictPosition = Vector3.zero;

            predictPosition.x = t * initialVelocity.x;
            predictPosition.y = (initialVelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2f);
            predictPosition.z = t * initialVelocity.z;
            
            simuratePointArray[i] = initialPosition + predictPosition;

        }

        // 線を引く場所を指定する
        lineRenderer.SetPositions(simuratePointArray);

        // lineRenderer.startColor = grad.Evaluate(gradLocation);
        // lineRenderer.endColor = grad.Evaluate(gradLocation);
        lineRenderer.material.color = grad.Evaluate(gradLocation);

        // float speed = initialVelocity.magnitude;
        // lineRenderer.startWidth = speed * 0.01f;
        // lineRenderer.endWidth = speed*speed * 0.0001f;

        timer = 10f;

    }
}