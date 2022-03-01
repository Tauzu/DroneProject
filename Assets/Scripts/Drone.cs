using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ドローンクラス

//操縦は外部スクリプト（DroneController.cs）から、public変数にアクセスして行う。
//FixedUpdateメソッド内で、public変数（コントローラーからの入力）に応じた物理演算を行って移動する。

public class Drone : MonoBehaviour
{
    //publicだがインスペクター上には表示しない
    [System.NonSerialized]
    public Vector2 targetVector;
    [System.NonSerialized]
    public float targetHeight;
    [System.NonSerialized]
    public float inputVertical;
    [System.NonSerialized]
    public bool isHovering = false;
    [System.NonSerialized]
    public bool isBoosting = false;

    //=================================================================

    //privateだがインスペクター上に表示
    [SerializeField]
    TextMesh faceText;

    [SerializeField]
    GameObject hoveringTextObj;

    [SerializeField]
    Renderer bodyRenderer;

    [SerializeField]
    GameObject barrierObj;

    Transform tf;
    Rigidbody rbody;

    struct Blade
    {
        Transform tf;
        int pitchSign;
        int rollSign;

        public Blade(Transform tf, int pitchSign, int rollSign)
        {
            this.tf = tf;
            this.pitchSign = pitchSign;
            this.rollSign = rollSign;
        }

        public void Rotate(float power)
        {
            this.tf.Rotate(new Vector3(0, 100 * power * this.pitchSign, 0));
        }

        public Vector3 Position()
        {
            return this.tf.position;
        }

        public float GetPower(float hoveringPow, float pitchCtrlPow, float rollCtrlPow)
        {
            return hoveringPow + this.pitchSign * pitchCtrlPow + this.rollSign * rollCtrlPow;
        }


    };

    const int numBlade = 4;

    Blade[] bladeArray = new Blade[numBlade];

    float pitch_pre;
    float roll_pre;

    Color defaultColor;

    AudioSource audioSource;
    //public AudioClip bladeSE;

    const float baseSpeed = 10f;
    const float coeff = 10f;
    const float Kp = 5f;
    const float decay = 1f;

    Vector3 positionMin = new Vector3(-100f, -10f, -100f);
    Vector3 positionMax = new Vector3(200f, 900f, 300f);

    // Start is called before the first frame update
    void Start()
    {
        tf = this.transform;
        
        rbody = this.GetComponent<Rigidbody>();
        rbody.centerOfMass = Vector3.zero; //BodyBoxの中心を全体の重心とする（ブレードの質量は無視）
        // rbody = tf.Find("BodyMesh").gameObject.GetComponent<Rigidbody>();

        bladeArray[0] = new Blade(tf.Find("blade1"), -1, -1);
        bladeArray[1] = new Blade(tf.Find("blade2"), 1, -1);
        bladeArray[2] = new Blade(tf.Find("blade3"), 1, 1);
        bladeArray[3] = new Blade(tf.Find("blade4"), -1, 1);

        //faceText = tf.Find("PlayerTextFace").gameObject.GetComponent<TextMesh>();

        (pitch_pre, roll_pre) = this.GetAttitude();

        //rend = tf.Find("BodyMesh").gameObject.GetComponent<Renderer>();
        defaultColor = bodyRenderer.material.color;

        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(rbody.centerOfMass);

        //float inputMagnitude = targetVector.magnitude;

        //(bool isBacking, float directionCosine) = CalculateYaw(inputMagnitude);

        //float hoveringPower = HoveringPower();

        //(float pitchCtrlPower, float rollCtrlPower) = AttitudeControl(
        //    inputMagnitude, hoveringPower, isBacking, directionCosine
        //    );

        //foreach (Blade blade in bladeArray)   //これだとコピーになる（Arrayの中身を操作できない）
        //for (int i = 0; i < numBlade; i++)
        //{
        //    bladeArray[i].SetPower(hoveringPower, pitchCtrlPower, rollCtrlPower);
        //}

        barrierObj.SetActive(Input.GetKey(KeyCode.X));

        PositionClamp();

        BoostingChange();
    }

    void FixedUpdate()
    {
        float inputMagnitude = targetVector.magnitude;

        (float yaw, bool isBacking, float directionCosine) = CalculateYaw(inputMagnitude);

        float hoveringPower = HoveringPower();

        (float pitchCtrlPower, float rollCtrlPower) = AttitudeControl(
            inputMagnitude, hoveringPower, isBacking, directionCosine
        );

        float power;
        float maxPow = 0f;
        foreach (Blade blade in bladeArray)
        {
            //Debug.Log(blade.GetPower());
            power = blade.GetPower(hoveringPower, pitchCtrlPower, rollCtrlPower);

            rbody.AddForceAtPosition(tf.up * coeff * power, blade.Position());

            blade.Rotate(power);

            //audioSource.PlayOneShot(bladeSE, Blade[i].power);
            maxPow = Mathf.Max(maxPow, Mathf.Abs(power));
            // Debug.Log(Blade[i].power);
        }

        //ブレード効果音
        audioSource.volume = Mathf.Clamp(2f * maxPow, 0f, 1f);
        audioSource.pitch = (maxPow - 1f) * 0.1f + 1f;

        // 機体ヨー回転
        Quaternion rot = Quaternion.AngleAxis(10f * yaw, tf.up);
        tf.rotation *= rot;

        if (Input.GetKey(KeyCode.X)) { rbody.AddTorque(-1000f*tf.right); }

        targetHeight += 0.01f * (tf.position.y - targetHeight); //目標高さを、自分の高さに少し近づける（安定化のため）

    }

    (float, bool, float) CalculateYaw(float inputMagnitude)
    {
        Vector2 forwardXZ = new Vector2(tf.forward.x, tf.forward.z).normalized;
        Vector2 rightXZ = new Vector2(tf.right.x, tf.right.z).normalized;

        float inner = Vector2.Dot(forwardXZ, targetVector.normalized); //現在の方向ベクトルと、目標方向ベクトルとの内積
        bool isBacking = (inner < -0.1f) ? true : false;

        //ヨー回転要求量
        float yaw = (isBacking) ? inputMagnitude * Mathf.Abs(-1f - inner)      //バック時(内積が-1になることを目指す)
                             : inputMagnitude * Mathf.Abs(1f - inner);     //前進時
        if (Vector2.Dot(rightXZ, targetVector) < 0f) yaw *= -1f;    //回転方向
        if (isBacking) yaw *= -1f;    //バック時は回転方向がさらに逆になる

        return (yaw, isBacking, inner);

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

    float HoveringPower()
    {
        float power;
        if (isHovering && (tf.up.y > 0f))   //ホバリング処理
        {
            targetHeight += 0.2f*inputVertical;
            //targetHeight += Mathf.Max(0.5f*inputVertical, -0.2f);

            power = Kp * (targetHeight - tf.position.y) - decay * this.rbody.velocity.y;

        }
        else    //ホバリングOFF時
        {
            power = inputVertical;

        }

        power = Mathf.Clamp(power, -10f, 10f);

        return power;

    }

    void PositionClamp()
    {
        Vector3 modifiedPosition;
        modifiedPosition.x = Mathf.Clamp(tf.position.x, positionMin.x, positionMax.x);
        modifiedPosition.y = Mathf.Clamp(tf.position.y, positionMin.y, positionMax.y);
        modifiedPosition.z = Mathf.Clamp(tf.position.z, positionMin.z, positionMax.z);

        if (modifiedPosition != tf.position)
        {
            tf.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }
    }

    float TargetPitchAngle(float inputMagnitude, bool isBacking, float inner)
    {
        float horizontalForwardSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.forward);
        float targetAngle;
        if (inputMagnitude > 0.1f)
        {
            float targetSPD = baseSpeed * inputMagnitude;
            if (isBoosting) targetSPD *= 2f;
            if (isBacking) targetSPD *= -1;
            float difference = (targetSPD - horizontalForwardSPD) / baseSpeed;//目標速度までの差の指標
            targetAngle = 90f * difference * Mathf.Abs(inner);
            //Debug.Log("ON");
        }
        else
        {
            //前後方向自動ブレーキ
            targetAngle = (isHovering)? - 45f * horizontalForwardSPD / baseSpeed : 0f;
            //Debug.Log("OFF");
        }

        return targetAngle;
    }

    float TargetRollAngle()
    {
        float horizontalRightSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.right);
        //左右方向自動ブレーキ
        float targetAngle = (isHovering) ? 60f * horizontalRightSPD / baseSpeed : 0f;

        if (isBoosting) targetAngle *= 0.2f; //加速時はブレーキ弱める

        return targetAngle;
    }

    (float, float) AttitudeControl(float inputMagnitude, float hoveringPower, bool isBacking, float inner)
    {
        //pitch[1] = tf.forward.y;
        //roll[1] = tf.right.y;
        (float pitch, float roll) = this.GetAttitude();

        float targetPitchAngle = Mathf.Clamp(TargetPitchAngle(inputMagnitude, isBacking, inner), -60f, 60f);
        targetPitchAngle *= Mathf.Sign(hoveringPower); // 上昇中と下降中で傾けるべき向きが逆になる
        float targetPitch = targetPitchAngle * Mathf.Deg2Rad;    //ピッチ姿勢制御
        float pitchControlPOW = Kp * (targetPitch - pitch) - 0.3f * decay * (pitch - pitch_pre) / Time.deltaTime;
        //pitchControlPOW *= 0.1f;
        //Debug.Log(pitch);

        float targetRollAngle = Mathf.Clamp(TargetRollAngle(), -60f, 60f);
        targetRollAngle *= Mathf.Sign(hoveringPower); // 上昇中と下降中で傾けるべき向きが逆になる
        float targetRoll = targetRollAngle * Mathf.Deg2Rad;    //ロール姿勢制御
        float rollControlPOW = Kp * (targetRoll - roll) - 0.3f * decay * (roll - roll_pre) / Time.deltaTime;

        pitch_pre = pitch;
        roll_pre = roll;

        return (pitchControlPOW, rollControlPOW);

    }


    (float, float) GetAttitude()
    {
        //0～360°のオイラー角を、-180～+180°に換算する
        float pitch = (Mathf.Repeat(tf.rotation.eulerAngles.x + 180, 360) - 180) * Mathf.Deg2Rad;
        float roll = (Mathf.Repeat(tf.rotation.eulerAngles.z + 180, 360) - 180) * Mathf.Deg2Rad;

        return (pitch, roll);
    }

    //public void Up()   //上昇
    //{
    //    for (int i = 0; i < BladeNum; i++)
    //    {
    //        Blade[i].power += 1;
    //    }
        
    //    targetHeight = height + 1.0f;

    //}

    //public void Down()   //下降
    //{
    //    for (int i = 0; i < BladeNum; i++)
    //    {
    //        Blade[i].power -= 1;
    //    }

    //    targetHeight = height - 0.5f;

    //}

    //public void Brake()
    //{
    //    //ブレーキ
    //    targetFwdAngle = 60f * horizontalForwardSPD / baseSpeed;
    //    targetRgtAngle = 60f * horizontalRightSPD / baseSpeed;

    //}

    float CalcHorizontalSpeed(Vector3 velocity3D, Vector3 direction)
    {
        Vector3 HorizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        float HorizontalSpeed = Vector3.Dot(velocity3D, HorizontalDirection);
        return HorizontalSpeed;
    }

    public void SwitchHovering(bool flag)
    {
        targetHeight = tf.position.y;
        isHovering = flag;
        hoveringTextObj.SetActive(flag);
    }

    //public void HeavyRotate()
    //{
    //    foreach (Blade blade in bladeArray)
    //    {
    //        blade.HeavyRotate();
    //    }
    //}

}