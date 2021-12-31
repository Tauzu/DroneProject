using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Rigidbodyにvecを加えた時の弾道をシュミレート。
 */
public class BallSimulator : MonoBehaviour {

    // [SerializeField]
    // private GameObject _ballSimPrefab; // 何でもOK。予測位置を表示するオブジェクト

    private const int SIMULATE_COUNT = 25; // いくつ先までシュミレートするか

    private Vector3 _startPosition; // 発射開始位置
    // private List<GameObject> simuratePointArray; // シュミレートするゲームオブジェクトリスト
    Vector3[] simuratePointArray; // シュミレートするpoint_array

    LineRenderer lineRenderer;
    public Gradient grad;

    void Start () {
        Init();
    }
	
    void Update () {
        // デバッグ用に線を出してみる。必要無いなら無くても問題なし。
        // if (simuratePointArray != null && simuratePointArray.Count > 0)
        // {
        //     for (int i = 0; i < SIMULATE_COUNT; i++)
        //     {
        //         if (i == 0)
        //         {
        //             Debug.DrawLine(_startPosition, simuratePointArray[i].transform.position);
        //         }else
        //         if (i < SIMULATE_COUNT)
        //         {
        //             Debug.DrawLine(simuratePointArray[i - 1].transform.position, simuratePointArray[i].transform.position);
        //         }
        //     }
        // }

    }

    // 初期化
    private void Init()
    {
        // if (simuratePointArray != null && simuratePointArray.Count > 0)
        // {
        //     foreach (var go in simuratePointArray)
        //     {
        //         Destroy(go.gameObject);
        //     }
        // }

        // 位置を表示するオブジェクトを予め作っておく
        // if (_ballSimPrefab != null)
        // {
        //     simuratePointArray = new List<GameObject>();
        //     for (int i = 0; i < SIMULATE_COUNT; i++)
        //     {
        //         var go = Instantiate(_ballSimPrefab);
        //         go.transform.SetParent(this.transform);
        //         go.transform.position = Vector3.down*100f;
        //         simuratePointArray.Add(go);
        //     }
        // }

        simuratePointArray = new Vector3[SIMULATE_COUNT];
        for (int i = 0; i < SIMULATE_COUNT; i++)
        {
            simuratePointArray[i]  = Vector3.down*100f;
        }

        lineRenderer = this.GetComponent<LineRenderer>();
         // 点の数を指定する
        lineRenderer.positionCount = simuratePointArray.Length;
    }

    /**
     * 弾道を予測計算する。オブジェクトを再生成せず、位置だけ動かす。
     * targetにはRigidbodyが必須です
     **/
    public void Simulate(Vector3 initialPosition , Vector3 initialVelocity, float gradLocation)
    {

        //弾道予測の位置に点を移動
        // for (int i = 0; i < SIMULATE_COUNT; i++)
        // {
        //     float t = (i * 0.2f) + 0.2f; // 0.2秒ごとの位置を予測。
        //     Vector3 predictPosition = Vector3.zero;

        //     predictPosition.x = t * initialVelocity.x;
        //     predictPosition.y = (initialVelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);
        //     predictPosition.z = t * initialVelocity.z;
            
        //     simuratePointArray[i].transform.position = initialPosition + predictPosition;

        // }

                //弾道予測の位置に点を移動
        for (int i = 0; i < SIMULATE_COUNT; i++)
        {
            float t = i * 0.2f + 0.05f; // 0.2秒ごとの位置を予測。
            Vector3 predictPosition = Vector3.zero;

            predictPosition.x = t * initialVelocity.x;
            predictPosition.y = (initialVelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);
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

    }
}