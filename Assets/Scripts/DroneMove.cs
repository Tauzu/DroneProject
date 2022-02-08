using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    public Vector3 minPosi = new Vector3(-1.0e2f, -1.0e1f, -2.0e1f);
    public Vector3 maxPosi = new Vector3(1.0e2f, 1.0e4f, 2.0e2f);

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public float inputForward;
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public float inputSide;
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool isBoosting;

    //=================================================================

    Rigidbody rbody;//  Rigidbodyを使うための変数

    struct BladeParam
    {
        public Transform tf;
        public float sign;
        public float power;

    };

    const int BladeNum = 4;

    BladeParam[] Blade = new BladeParam[BladeNum];

    float inputMagnitude;

    float height;
    float[] FwdY = new float[2];
    float[] RgtY = new float[2];

    float horizontalForwardSPD;
    float horizontalRightSPD;

    bool isHovering;

    float heightHOV;
    float yawDiff;

    float targetFwdAngle;
    float targetRgtAngle;

    TextMesh faceText;
    GameObject hoveringTextObj;

    GameObject cameraObj;

    Color defaultColor;
    Renderer rend;

    AudioSource audioSource;
    //public AudioClip bladeSE;

    const float baseSpeed = 10f;
    const float coeff = 10f;
    const float Kp = 10f;
    const float decay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        rbody.centerOfMass = Vector3.zero;
        // rbody = this.transform.Find("BodyMesh").gameObject.GetComponent<Rigidbody>();

        Blade[0].tf = this.transform.Find("blade1");
        Blade[0].sign = -1f;

        Blade[1].tf = this.transform.Find("blade2");
        Blade[1].sign = 1f;

        Blade[2].tf = this.transform.Find("blade3");
        Blade[2].sign = 1f;

        Blade[3].tf = this.transform.Find("blade4");
        Blade[3].sign = -1f;

        hoveringTextObj = this.transform.Find("HoveringText").gameObject;

        faceText = this.transform.Find("PlayerTextFace").gameObject.GetComponent<TextMesh>();

        height = this.transform.position.y;
        FwdY[0] = this.transform.forward.y;
        RgtY[0] = this.transform.right.y;

        cameraObj = GameObject.Find("Main Camera");

        rend = this.transform.Find("BodyMesh").gameObject.GetComponent<Renderer>();
        defaultColor = rend.material.color;

        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(rbody.centerOfMass);

        height = this.transform.position.y;

        horizontalForwardSPD = CalcHorizontalSpeed(this.rbody.velocity, this.transform.forward);
        horizontalRightSPD = CalcHorizontalSpeed(this.rbody.velocity, this.transform.right);

        inputMagnitude = Mathf.Clamp(Mathf.Abs(inputForward) + Mathf.Abs(inputSide), 0f, 1f);

        HorizontalMoveProcess();

        float hoveringPower = HoveringProcess();

        // if (Input.GetKeyDown(KeyCode.F))   //z軸方向を向く
        // {
        //     rbody.angularVelocity = Vector3.zero;
        //     transform.rotation = Quaternion.LookRotation(Vector3.forward);
        // }

        // targetFwdAngle = Mathf.Clamp(targetFwdAngle, -45f, 45f);
        // targetRgtAngle = Mathf.Clamp(targetRgtAngle, -45f, 45f);

        float[] attitudeControlPower = AttitudeControl();

        for (int i = 0; i < BladeNum; i++)
        {
            Blade[i].power = attitudeControlPower[i] + hoveringPower;
            //Blade[i].power = Mathf.Clamp(Blade[i].power, -4f, 4f);
        }

        PositionClamp();


    }

    void FixedUpdate()
    {
        float maxPow = 0f;
        for (int i = 0; i < BladeNum; i++)
        {
            rbody.AddForceAtPosition(this.transform.up * coeff * Blade[i].power, Blade[i].tf.position);
            Blade[i].tf.Rotate(new Vector3(0, 100 * Blade[i].power * Blade[i].sign, 0));
            //audioSource.PlayOneShot(bladeSE, Blade[i].power);
            maxPow = Mathf.Max(maxPow, Mathf.Abs(Blade[i].power));
            // Debug.Log(Blade[i].power);
        }

        //Debug.Log(maxPow);
        audioSource.volume = Mathf.Clamp(2f * maxPow, 0f, 1f);
        audioSource.pitch = (maxPow - 1f) * 0.1f + 1f;

        // 指定軸まわりに回転させるQuaternionを作成
        Quaternion rot = Quaternion.AngleAxis(10f * yawDiff, this.transform.up);
        // 自身に乗じることで回転
        this.transform.rotation *= rot;

        heightHOV += 0.01f * (height - heightHOV); //目標高さを、自分の高さに少し近づける（安定化のため）

    }

    void HorizontalMoveProcess()
    {
        Vector3 cameraFwd = cameraObj.transform.forward;
        Vector3 cameraRgt = cameraObj.transform.right;
        Vector2 forwardXZ = new Vector2(this.transform.forward.x, this.transform.forward.z).normalized;
        Vector2 rightXZ = new Vector2(this.transform.right.x, this.transform.right.z).normalized;
        Vector2 cameraFwdXZ = new Vector2(cameraFwd.x, cameraFwd.z).normalized;
        Vector2 cameraRgtXZ = new Vector2(cameraRgt.x, cameraRgt.z).normalized;
        Vector2 targetXZ = (inputForward * cameraFwdXZ + inputSide * cameraRgtXZ).normalized;

        float inner = Vector2.Dot(forwardXZ, targetXZ); //現在の方向ベクトルと、目標方向ベクトルとの内積
        bool isBacking = (inner < -0.1f) ? true : false;

        //ヨー回転要求量
        yawDiff = (isBacking) ? inputMagnitude * Mathf.Abs(-1f - inner)      //バック時(内積が-1になることを目指す)
                             : inputMagnitude * Mathf.Abs(1f - inner);     //前進時
        if (Vector2.Dot(rightXZ, targetXZ) < 0f) yawDiff *= -1f;    //回転方向
        if (isBacking) yawDiff *= -1f;    //バック時は回転方向がさらに逆になる

        float targetSPD = baseSpeed * inputMagnitude;
        if (isBoosting) targetSPD *= 2f;
        if (isBacking) targetSPD *= -1;
        float difference = (targetSPD - horizontalForwardSPD) / baseSpeed;//目標速度までの差の指標
        targetFwdAngle = 90f * -difference * Mathf.Abs(inner);
        // Debug.Log(targetFwdAngle);

        if (isBoosting)    //加速時色変化
        {
            rend.material.color = Color.red;

            faceText.text = "`^´";
            faceText.color = Color.yellow;
        }
        else
        {
            rend.material.color = defaultColor;

            faceText.text = "'~'";
            faceText.color = Color.blue;
        }

    }

    // float maxPow = 0f;
    float HoveringProcess()
    {
        float HovPower;
        if (isHovering && (this.transform.up.y > 0f))   //ホバリング処理
        {
            HovPower = Mathf.Clamp(-Kp * (height - heightHOV) - decay * this.rbody.velocity.y, -10f, 10f);
            // if(Mathf.Abs(HovPower) > maxPow){
            //     maxPow = Mathf.Abs(HovPower);
            //     Debug.Log(HovPower);
            // }
            for (int i = 0; i < BladeNum; i++)
            {
                // Blade[i].power -= decay*(height[1] - height[0]);
                Blade[i].power = HovPower;

            }

            if (inputMagnitude < 0.1f)   //前後方向自動ブレーキ
            {
                targetFwdAngle = 45f * horizontalForwardSPD / baseSpeed;
            }

            targetRgtAngle = 60f * horizontalRightSPD / baseSpeed;   //左右方向自動ブレーキ
            if (isBoosting) targetRgtAngle *= 0.2f; //加速時はブレーキ弱める

        }
        else    //ホバリングOFF時
        {
            if (inputMagnitude < 0.1f)  //入力がないときは水平姿勢を目指す
            {
                targetFwdAngle = 0f;
                targetRgtAngle = 0f;
            }

            HovPower = 0;
        }

        return HovPower;

    }

    void PositionClamp()
    {
        Vector3 modifiedPosition;
        modifiedPosition.x = Mathf.Clamp(this.transform.position.x, minPosi.x, maxPosi.x);
        modifiedPosition.y = Mathf.Clamp(this.transform.position.y, minPosi.y, maxPosi.y);
        modifiedPosition.z = Mathf.Clamp(this.transform.position.z, minPosi.z, maxPosi.z);

        if (modifiedPosition != this.transform.position)
        {
            this.transform.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }
    }

    float[] AttitudeControl()
    {
        float[] controlPower = new float[BladeNum];

        FwdY[1] = this.transform.forward.y;
        RgtY[1] = this.transform.right.y;

        targetFwdAngle = Mathf.Clamp(targetFwdAngle, -60f, 60f);
        float targetFwd = Mathf.Sin(targetFwdAngle * Mathf.Deg2Rad);    //ピッチ姿勢制御
        float pitchControlPOW = Kp * (FwdY[1] - targetFwd) + 0.3f * decay * (FwdY[1] - FwdY[0]) / Time.deltaTime;

        targetRgtAngle = Mathf.Clamp(targetRgtAngle, -60f, 60f);
        float targetRgt = Mathf.Sin(targetRgtAngle * Mathf.Deg2Rad);    //ロール姿勢制御
        float rollControlPOW = Kp * (RgtY[1] - targetRgt) + 0.3f * decay * (RgtY[1] - RgtY[0]) / Time.deltaTime;

        controlPower[0] = - pitchControlPOW + rollControlPOW;
        controlPower[1] = pitchControlPOW + rollControlPOW;
        controlPower[2] = pitchControlPOW - rollControlPOW;
        controlPower[3] = - pitchControlPOW - rollControlPOW;

        FwdY[0] = FwdY[1];
        RgtY[0] = RgtY[1];

        return controlPower;

    }

    public void Up()
    {
        //if (Input.GetKey(KeyCode.LeftShift))   //上昇

        for (int i = 0; i < BladeNum; i++)
        {
            Blade[i].power += 1;
        }

        heightHOV = height + 1.0f;

    }

    public void Down()
    {
        //else if (Input.GetKey(KeyCode.LeftControl))   //下降
        for (int i = 0; i < BladeNum; i++)
        {
            Blade[i].power -= 1;
        }

        heightHOV = height - 0.5f;

    }

    public void Brake()
    {
        //ブレーキ
        targetFwdAngle = 60f * horizontalForwardSPD / baseSpeed;
        targetRgtAngle = 60f * horizontalRightSPD / baseSpeed;

    }

    float CalcHorizontalSpeed(Vector3 velocity3D, Vector3 direction)
    {
        Vector3 HorizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        float HorizontalSpeed = Vector3.Dot(velocity3D, HorizontalDirection);
        return HorizontalSpeed;
    }

    public void SwitchHovering()
    {
        heightHOV = height;
        isHovering = !isHovering;
        hoveringTextObj.SetActive(isHovering);
    }

    public void DisableHovering()
    {
        isHovering = false;
        hoveringTextObj.SetActive(false);
    }

    public void HeavyRotate()
    {
        for (int i = 0; i < BladeNum; i++)
        {
            Blade[i].power += 100f * Blade[i].sign;
        }
    }

}
