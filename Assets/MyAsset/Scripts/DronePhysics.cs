using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドローン物理演算クラス
/// ドローンクラスに継承されたのち、継承先のFixedUpdate内で物理演算メソッドが呼び出される
/// </summary>
public class DronePhysics : MonoBehaviour
{
    float targetHeight;  //目標高度
    bool isHovering; //ホバリング中か否か
    bool isBoosting; //高速モードか否か

    Transform tf;       //ドローン本体のTransform
    Rigidbody rbody;       //ドローン本体のRigidBody

    float pitch_pre;    //前ステップにおけるピッチ角
    float roll_pre;    //前ステップにおけるロール角

    const float baseSpeed = 10f;    //基準スピード
    const float coeff = 10f;        //プロペラの出力係数
    const float Kp = 4f;            //応答の速さ（PD制御におけるPゲイン）
    const float decay = 1f;         //振動減衰の強さ（PD制御におけるDゲイン）
    const float maxAngle = 70f;     //目標角度の上限

    float standardHeight = 0f;
    bool isTerrainMode = true;

    /// <summary>
    /// プロペラブレード構造体。
    /// プロペラの位置を格納するほか、位置ごとに必要な出力を計算する。
    /// </summary>
    struct Blade
    {
        Transform tf;       //Transform
        int pitchSign;      //ピッチ方向に傾けたいときの出力向き
        int rollSign;       //ロール方向に傾けたいときの出力向き

        /// <Summary>
        /// コンストラクタ。
        /// </Summary>
        /// <param name="tf">ブレードのTransform</param>
        /// <param name="pitchSign">ブレードの出力向き（ピッチ）</param>
        /// <param name="rollSign">ブレードの出力向き（ロール）</param>
        public Blade(Transform tf, int pitchSign, int rollSign)
        {
            this.tf = tf;
            this.pitchSign = pitchSign;
            this.rollSign = rollSign;
        }

        /// <Summary>
        /// ブレードの回転を行います。
        /// </Summary>
        /// <param name="power">回転強さ</param>
        public void Rotate(float power)
        {
            tf.Rotate(new Vector3(0, 100 * power * this.pitchSign, 0));
        }

        /// <summary>
        /// ブレードの座標を返します。
        /// </summary>
        /// <returns>ブレード座標</returns>
        public Vector3 Position()
        {
            return tf.position;
        }

        /// <summary>
        /// ブレードの出力を算出します。
        /// </summary>
        /// <param name="hoveringPow">ホバリング要求量</param>
        /// <param name="pitchCtrlPow">ピッチ回転要求量</param>
        /// <param name="rollCtrlPow">ロール回転要求量</param>
        /// <returns>ブレード出力</returns>
        public float GetPower(float hoveringPow, float pitchCtrlPow, float rollCtrlPow)
        {
            return hoveringPow + pitchSign * pitchCtrlPow + rollSign * rollCtrlPow;
        }


    };

    const int numBlade = 4;     //ブレード数
    Blade[] bladeArray = new Blade[numBlade];   //ブレード構造体配列

    float totalPower; //4枚のブレードの出力の合計値

    // Start is called before the first frame update
    /// <summary>
    /// ドローン物理演算用の初期化。
    /// Rigidbody、４つのプロペラを取得。
    /// </summary>
    protected virtual void Start()
    {
        tf = this.transform;

        rbody = this.GetComponent<Rigidbody>();
        rbody.centerOfMass = Vector3.zero; //BodyBoxの中心を全体の重心とする（ブレードの質量は無視）
        // rbody = tf.Find("BodyMesh").gameObject.GetComponent<Rigidbody>();

        bladeArray[0] = new Blade(tf.Find("blade1"), -1, -1);
        bladeArray[1] = new Blade(tf.Find("blade2"), 1, -1);
        bladeArray[2] = new Blade(tf.Find("blade3"), 1, 1);
        bladeArray[3] = new Blade(tf.Find("blade4"), -1, 1);

        (pitch_pre, roll_pre) = this.GetAttitude();

    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    /// <summary>
    /// 物理演算メソッド。
    /// 継承先のFixedUpdateから呼ばれることを想定。
    /// ホバリングおよび姿勢制御に必要な力を算出し、ドローン本体のRigidBodyに加える。
    /// </summary>
    /// <param name="targetVector">目標方向ベクトル</param>
    /// <param name="targetVertical">目標上下移動量</param>
    protected void PhysicalCalculation(Vector2 targetVector, float targetVertical)
    {
        float height = tf.position.y - standardHeight;

        (float yaw, bool isBacking, float directionCosine) = CalculateYaw(targetVector);

        float hoveringPower = HoveringPower(targetVertical, height);

        //Debug.Log(targetVector);
        (float pitchCtrlPower, float rollCtrlPower) = AttitudeControl(
            targetVector.magnitude, hoveringPower, isBacking, directionCosine
        );

        totalPower = 0f;
        foreach (Blade blade in bladeArray)
        {
            //Debug.Log(blade.GetPower());
            float power = blade.GetPower(hoveringPower, pitchCtrlPower, rollCtrlPower);

            rbody.AddForceAtPosition(tf.up * coeff * power, blade.Position());

            blade.Rotate(power);

            totalPower += Mathf.Abs(power);   //ブレードの出力値を加算

        }

        // 機体ヨー回転は、クオータニオンを直接操作
        Quaternion rot = Quaternion.AngleAxis(15f * yaw, tf.up);
        tf.rotation *= rot;

        if (Input.GetKey(KeyCode.X)) { rbody.AddTorque(-1000f * tf.right); }

        targetHeight += 0.01f * (height - targetHeight); //目標高さを、自分の高さに少し近づける（安定化のため）

    }

    /// <summary>
    /// キー入力の向きと、自分との向きから、回転すべき量を算出します。
    /// </summary>
    /// <param name="targetVector">目標方向ベクトル</param>
    /// <returns>ヨー回転要求量、バックフラグ、目標ベクトルと現在ベクトルの内積</returns>
    (float, bool, float) CalculateYaw(Vector2 targetVector)
    {
        Vector2 forwardXZ = new Vector2(tf.forward.x, tf.forward.z).normalized;
        Vector2 rightXZ = new Vector2(tf.right.x, tf.right.z).normalized;

        float inner = Vector2.Dot(forwardXZ, targetVector.normalized); //現在の方向ベクトルと、目標方向ベクトルとの内積
        bool isBacking = (inner < -0.1f);

        float inputMagnitude = targetVector.magnitude;
        //ヨー回転要求量
        float yaw = (isBacking) ? inputMagnitude * Mathf.Abs(-1f - inner)      //バック時(内積が-1になることを目指す)
                             : inputMagnitude * Mathf.Abs(1f - inner);     //前進時(内積が1になることを目指す)

        //目標方向に応じて回転向きを変える。バック時は回転方向がさらに逆になる
        if ((Vector2.Dot(rightXZ, targetVector)) < 0f ^ isBacking) yaw *= -1f;

        return (yaw, isBacking, inner);

    }

    /// <summary>
    /// ホバリングに必要なパワーを算出します。
    /// 目標高さに追従するようにPD制御を行います。
    /// </summary>
    /// <param name="targetVertical">目標方向ベクトル</param>
    /// <returns>ホバリング要求量</returns>
    float HoveringPower(float targetVertical, float height)
    {
        float power;
        if (isHovering && (tf.up.y > 0f))   //ホバリング処理
        {
            targetHeight += 0.2f * targetVertical;
            //targetHeight += Mathf.Max(0.5f*inputVertical, -0.2f);

            power = Kp * (targetHeight - height) - decay * this.rbody.velocity.y;

            //Debug.Log((targetHeight, height));
        }
        else    //ホバリングOFF時
        {
            power = targetVertical;

        }

        power = Mathf.Clamp(power, -10f, 10f);

        return power;

    }

    /// <summary>
    /// ピッチ目標角度を算出します。
    /// キーボード入力がある場合は行きたい方向に傾けようとするが、ない場合はブレーキがかかるように計算。
    /// </summary>
    /// <param name="inputMagnitude">キーボード入力の強さ</param>
    /// <param name="isBacking">バック中か否か</param>
    /// <param name="inner">目標ベクトルと現在ベクトルの内積</param>
    /// <returns>ピッチ目標角度</returns>
    float TargetPitchAngle(float inputMagnitude, bool isBacking, float inner)
    {
        //後退中であればマイナスが返る
        float horizontalForwardSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.forward);

        //Debug.Log(inputMagnitude);

        float targetAngle;
        if (inputMagnitude > 0.2f)
        {
            float targetSPD = baseSpeed * inputMagnitude;
            if (isBoosting) targetSPD *= 2f;
            if (isBacking) targetSPD *= -1f;
            float difference = (targetSPD - horizontalForwardSPD) / baseSpeed;//目標速度までの差の指標
            targetAngle = 90f * difference * Mathf.Abs(inner);
            //Debug.Log("ON");
        }
        else
        {
            //前後方向自動ブレーキ
            targetAngle = (isHovering) ? -90f * horizontalForwardSPD / baseSpeed : 0f;
            //Debug.Log(targetAngle);
        }

        //Debug.Log(targetAngle);
        return targetAngle;
    }

    /// <summary>
    /// ロー目標角度を算出します。
    /// 左右方向にはブレーキがかかるように計算。
    /// </summary>
    /// <returns>ロール目標角度</returns>
    float TargetRollAngle()
    {
        float horizontalRightSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.right);
        //左右方向自動ブレーキ
        float targetAngle = (isHovering) ? 90f * horizontalRightSPD / baseSpeed : 0f;

        if (isBoosting) targetAngle *= 0.2f; //加速時はブレーキ弱める

        return targetAngle;
    }

    /// <summary>
    /// ピッチおよびローの目標角度へ追従するようPD制御を行い、必要な力を算出。
    /// </summary>
    /// <param name="inputMagnitude">キーボード入力の強さ</param>
    /// <param name="hoveringPower">ホバリング要求量</param>
    /// <param name="isBacking">バック中か否か</param>
    /// <param name="inner">目標ベクトルと現在ベクトルの内積</param>
    /// <returns>ピッチ回転要求量、ロール回転要求量</returns>
    (float, float) AttitudeControl(float inputMagnitude, float hoveringPower, bool isBacking, float inner)
    {
        //pitch[1] = tf.forward.y;
        //roll[1] = tf.right.y;
        (float pitch, float roll) = this.GetAttitude();

        float targetPitchAngle = Mathf.Clamp(TargetPitchAngle(inputMagnitude, isBacking, inner), -maxAngle, maxAngle);
        targetPitchAngle *= Mathf.Sign(hoveringPower); // 上昇中と下降中で傾けるべき向きが逆になる
        float targetPitch = targetPitchAngle * Mathf.Deg2Rad;    //ピッチ姿勢制御
        float pitchControlPOW = Kp * (targetPitch - pitch) - 0.3f * decay * (pitch - pitch_pre) / Time.deltaTime;
        //pitchControlPOW *= 0.1f;
        //Debug.Log(pitch);

        float targetRollAngle = Mathf.Clamp(TargetRollAngle(), -maxAngle, maxAngle);
        targetRollAngle *= Mathf.Sign(hoveringPower); // 上昇中と下降中で傾けるべき向きが逆になる
        float targetRoll = targetRollAngle * Mathf.Deg2Rad;    //ロール姿勢制御
        float rollControlPOW = Kp * (targetRoll - roll) - 0.3f * decay * (roll - roll_pre) / Time.deltaTime;

        pitch_pre = pitch;
        roll_pre = roll;

        return (pitchControlPOW, rollControlPOW);

    }

    /// <summary>
    /// 機体姿勢のオイラー角（0～360°）を、-180～+180°に換算し、ラジアンに変換して返す
    /// </summary>
    /// <returns>ピッチ角度、ロール角度</returns>
    (float, float) GetAttitude()
    {
        float pitch = (Mathf.Repeat(tf.rotation.eulerAngles.x + 180, 360) - 180) * Mathf.Deg2Rad;
        float roll = (Mathf.Repeat(tf.rotation.eulerAngles.z + 180, 360) - 180) * Mathf.Deg2Rad;

        return (pitch, roll);
    }

    /// <summary>
    /// 水平方向の移動速さを算出
    /// </summary>
    /// <param name="velocity3D">速度ベクトル</param>
    /// <param name="direction">任意の方向ベクトル</param>
    /// <returns>水平速さ</returns>
    float CalcHorizontalSpeed(Vector3 velocity, Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        float horizontalSpeed = Vector3.Dot(velocity, horizontalDirection);
        return horizontalSpeed;
    }

    /// <summary>
    /// ホバリング状態を切り替える。
    /// </summary>
    /// <param name="flag">切り替え先</param>
    public void SwitchHovering(bool flag)
    {
        targetHeight = tf.position.y - standardHeight;
        isHovering = flag;
    }

    /// <summary>
    /// ホバリング中か否かを取得
    /// </summary>
    /// <returns>ホバリング中か否か</returns>
    public bool IsHovering()
    {
        return isHovering;
    }

    /// <summary>
    /// 高速モードを指定して切り替える。
    /// </summary>
    /// <param name="isBoosting">切り替え先</param>
    public void SetBoosting(bool isBoosting)
    {
        this.isBoosting = isBoosting;
    }

    /// <summary>
    /// 高速モードか否かを取得
    /// </summary>
    /// <returns>高速モードか否か</returns>
    public bool IsBoosting()
    {
        return isBoosting;
    }

    /// <summary>
    /// ドローンの状態を強制的に指定する。
    /// ChildDroneを操作する時にしか使わない。
    /// </summary>
    /// <param name="targetHeight">目標高度</param>
    /// <param name="isHovering">ホバリング状態</param>
    /// <param name="isBoosting">高速モード</param>
    public void SpecifyStatus(float targetHeight, bool isHovering, bool isBoosting)
    {
        this.targetHeight = targetHeight;
        this.isHovering = isHovering;
        this.isBoosting = isBoosting;
    }

    /// <summary>
    /// ブレードの出力の最大値を取得
    /// </summary>
    /// <returns>ブレードの出力の最大値</returns>
    public float GetTotalPower()
    {
        return totalPower;
    }

    /// <summary>
    /// 飛行情報の通知。
    /// </summary>
    /// <param name="terrainHeight">真下の地形の標高</param>
    /// <param name="flyingHeight">飛行高度（地表距離）</param>
    public void NoticeFlyingHeight(float terrainHeight, float flyingHeight)
    {
        bool isCloseToGround = (flyingHeight < 10f);
        standardHeight = (isCloseToGround) ? terrainHeight : 0f;

        if (isCloseToGround != isTerrainMode)
        {
            isTerrainMode = isCloseToGround;

            //モードが変わると基準高さが急変するので、それに順応するために目標高度を上書き
            targetHeight = tf.position.y - standardHeight;
        }

    }

}
