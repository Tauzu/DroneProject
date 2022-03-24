using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドローンクラス（ドローン物理演算クラスを継承）。
/// 操縦は外部スクリプト（DroneController.cs）から、public変数にアクセスして行う。
/// 物理演算はFixedUpdateメソッド内で行う。
/// </summary>
public class Drone : DronePhysics
{
    Vector2 targetVector;    //目標ベクトル（水平面における）
    float targetVertical;     //上下どちらへ行きたいか

    public GameObject barrierObj;
    public GameObject magnetObj;

    //privateだがインスペクター上に表示
    [SerializeField]
    TextMesh faceText;
    [SerializeField]
    MeshRenderer hoveringRenderer;
    [SerializeField]
    Renderer bodyRenderer;

    Color defaultColor;

    AudioSource audioSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();    // 継承元のStart関数

        //rend = tf.Find("BodyMesh").gameObject.GetComponent<Renderer>();
        defaultColor = bodyRenderer.material.color;

        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        PositionClamp();
        BoostingChange();
        hoveringRenderer.enabled = IsHovering();
    }

    void FixedUpdate()
    {
        PhysicalCalculation(targetVector, targetVertical); //物理演算

        //ブレード効果音
        float bladePower = GetMaxPower();
        audioSource.volume = Mathf.Clamp(2f * bladePower, 0f, 1f);
        audioSource.pitch = (bladePower - 1f) * 0.1f + 1f;

    }

    /// <summary>
    /// 加速モードかどうかチェックし、それに応じてドローンの顔文字を変更する
    /// </summary>
    void BoostingChange()
    {
        if (IsBoosting())    //加速時色変化
        {
            bodyRenderer.material.color = Color.red;

            faceText.text = "`^´";
            faceText.color = Color.yellow;

        }
        else
        {
            bodyRenderer.material.color = defaultColor;

            faceText.text = "'~'";
            faceText.color = Color.blue;
        }
    }

    /// <summary>
    /// 目標方向ベクトルをセットする。
    /// </summary>
    /// <param name="targetVector">目標方向ベクトル</param>
    public void SetTargetVector(Vector2 targetVector)
    {
        this.targetVector = targetVector;
    }

    /// <summary>
    /// 目標上下方向（上昇したいか下降したいか）をセットする。
    /// </summary>
    /// <param name="inputVertical">目標上下方向</param>
    public void SetTargetVeritical(float inputVertical)
    {
        this.targetVertical = inputVertical;
    }

}
