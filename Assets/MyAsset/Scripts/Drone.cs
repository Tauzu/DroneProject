using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ドローンクラス（ドローン物理演算クラスを継承）

//操縦は外部スクリプト（DroneController.cs）から、public変数にアクセスして行う。
//物理演算はFixedUpdateメソッド内で行う


public class Drone : DronePhysics
{
    //publicだがインスペクター上には表示しない
    [System.NonSerialized]
    public Vector2 targetVector;    //目標ベクトル（水平面における）
    [System.NonSerialized]
    public float inputVertical;     //鉛直方向の入力（上下どちらへ行きたいか）

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
    //public AudioClip bladeSE;

    // Start is called before the first frame update
    void Start()
    {
        InitPhysics();

        //rend = tf.Find("BodyMesh").gameObject.GetComponent<Renderer>();
        defaultColor = bodyRenderer.material.color;

        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        PositionClamp();
        BoostingChange();
        hoveringRenderer.enabled = isHovering;
    }

    void FixedUpdate()
    {
        PhysicalCalculation(targetVector, inputVertical); //物理演算

        //ブレード効果音
        float bladePower = GetMaxPower();
        audioSource.volume = Mathf.Clamp(2f * bladePower, 0f, 1f);
        audioSource.pitch = (bladePower - 1f) * 0.1f + 1f;

    }

    void BoostingChange()
    {
        if (isBoosting)    //加速時色変化
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

}
