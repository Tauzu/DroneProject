using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    private const float speed = 10f;
    private const float coeff = 10f;
    private const float Kp = 4f;
    private const float decay = 50f;

    private Rigidbody rbody;//  Rigidbodyを使うための変数

    struct BladeParam
    {
        public Transform tf;
        public float rotDirection;
        public string key;
        public float power;

    };

    private const int BladeNum = 4;

    private BladeParam[] Blade = new BladeParam[BladeNum];

    private Vector3 forward;
    private Vector3 right;
    private Vector3 modifiedPosition;
    private Vector3 velocity;

    private float[] height = new float[2];
    private float[] FwdY = new float[2];
    private float[] RgtY = new float[2];

    private float inputForward;
    private float inputSide;
    private float inputMagnitude;

    public Vector3 minPosi = new Vector3(-1.0e2f, -1.0e1f, -2.0e1f);
    public Vector3 maxPosi = new Vector3(1.0e2f, 1.0e4f, 2.0e2f);

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool  isHovering;
    
    private float heightHOV;

    private float targetSPD;
    private float difference;
    private float yawDiff;
    private float[] forwardSPD = new float[2];
    private float rightSPD;
    private float controlPOW;

    private float targetFwdAngle;
    private float targetRgtAngle;

    private float targetFwd;
    private float targetRgt;

    private GameObject hovering_text;

    private GameObject cameraObj;
    private Vector3 cameraFwd;
    private Vector3 cameraRgt;
    private Vector2 forwardXZ;
    private Vector2 rightXZ;
    private Vector2 cameraFwdXZ;
    private Vector2 cameraRgtXZ;
    private Vector2 targetXZ;

    private bool  isBacking;

    private Material defaultMaterial;
    public Material powerMaterial;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定

        rbody = this.GetComponent<Rigidbody>();

        Blade[0].tf = this.transform.Find("blade1");
        Blade[0].key = "t";
        Blade[0].rotDirection = 1f;

        Blade[1].tf = this.transform.Find("blade2");
        Blade[1].key = "g";
        Blade[1].rotDirection = -1f;

        Blade[2].tf = this.transform.Find("blade3");
        Blade[2].key = "j";
        Blade[2].rotDirection = 1f;

        Blade[3].tf = this.transform.Find("blade4");
        Blade[3].key = "i";
        Blade[3].rotDirection = -1f;

        hovering_text = this.transform.Find("hovering_text").gameObject;

        height[0] = this.transform.position.y;
        FwdY[0] = this.transform.forward.y;
        RgtY[0] = this.transform.right.y;

        cameraObj = GameObject.Find("Main Camera");

        defaultMaterial = this.GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(this.transform.position);
        height[1] = this.transform.position.y;
        velocity = rbody.velocity;
        forward = this.transform.forward;
        right = this.transform.right;

        for (int i = 0; i < BladeNum; i++)   //対応キー出力
        {
            Blade[i].power = (Input.GetKey(Blade[i].key)) ? 1 : 0;//  もし、キーがおされているなら、 
        }

        if (Input.GetKey(KeyCode.LeftShift))   //上昇
        {
            for (int i = 0; i < BladeNum; i++)
            {
                Blade[i].power = 1;
            }

            heightHOV = height[1] + 1.0f;

        }
        else if (Input.GetKey(KeyCode.LeftControl))   //下降
        {
            for (int i = 0; i < BladeNum; i++)
            {
                Blade[i].power = -1;
            }

            heightHOV = height[1] - 0.5f;

        }


        inputForward = Input.GetAxis("W-S");
        inputSide = Input.GetAxis("D-A");
        inputMagnitude = Mathf.Clamp(Mathf.Abs(inputForward) + Mathf.Abs(inputSide), 0f, 1f);

        cameraFwd = cameraObj.transform.forward;
        cameraRgt = cameraObj.transform.right;
        forwardXZ = new Vector2(forward.x, forward.z).normalized;
        rightXZ = new Vector2(right.x, right.z).normalized;
        cameraFwdXZ = new Vector2(cameraFwd.x, cameraFwd.z).normalized;
        cameraRgtXZ = new Vector2(cameraRgt.x, cameraRgt.z).normalized;
        targetXZ = (inputForward * cameraFwdXZ + inputSide * cameraRgtXZ).normalized;
        isBacking = (Vector2.Dot(forwardXZ, targetXZ) < -0.5f) ? true : false;
        targetXZ *= (isBacking) ? -1 : 1;
        yawDiff = inputMagnitude * Mathf.Abs(1f - Vector2.Dot(forwardXZ, targetXZ));
        yawDiff *= (Vector2.Dot(rightXZ, targetXZ) > 0) ? 1 : -1;    //回転方向


        forwardSPD[1] = Vector3.Dot(velocity, forward);
        rightSPD = Vector3.Dot(velocity, right);

        // if(Vector2.Dot(forwardXZ, targetXZ) > 0.0f)
        // {
        targetSPD = (Input.GetKey(KeyCode.P)) ? 2 * speed * inputMagnitude : speed * inputMagnitude;
        targetSPD *= (isBacking) ? -1 : 1;
        difference = (Mathf.Abs(targetSPD) > 0.1f) ? (forwardSPD[1] - targetSPD) / speed : 0f;
        float variable = Mathf.Abs(difference);
        difference = difference / variable * Mathf.Sqrt(variable);
        targetFwdAngle = 40 * Mathf.Deg2Rad * difference;
        // }

        if (inputMagnitude < 0.1f)  //入力がないときは水平姿勢を目指す
        {
            targetFwdAngle = 0f;
            targetRgtAngle = 0f;
        }

        // targetSPD = (Input.GetKey(KeyCode.P))? 3*speed*inputSide : speed*inputSide;
        // difference = (targetSPD - rightSPD)/speed * Mathf.Abs(inputSide);
        // targetRgtAngle =　-40*Mathf.Deg2Rad * difference;

        if (Input.GetKeyDown(KeyCode.F))   //z軸方向を向く
        {
            rbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }


        if (Input.GetKeyDown(KeyCode.H))   //ホバリングスイッチ
        {
            heightHOV = height[1];
            isHovering = (isHovering) ? false : true;
            hovering_text.SetActive(isHovering);
        }

        if (isHovering && (this.transform.up.y > 0.3f))   //ホバリング処理
        {
            for (int i = 0; i < BladeNum; i++)
            {
                // Blade[i].power -= decay*(height[1] - height[0]);
                Blade[i].power -= Kp * (height[1] - heightHOV) + decay * (height[1] - height[0]);

            }

            if (inputMagnitude < 0.1f)   //前後方向自動ブレーキ
            {
                targetFwdAngle = 30f * Mathf.Deg2Rad * forwardSPD[1] / speed;
            }

            targetRgtAngle = 60f * Mathf.Deg2Rad * rightSPD / speed;   //左右方向自動ブレーキ
            targetRgtAngle *= (Mathf.Abs(targetSPD) > 1.1f * speed) ? 0.1f : 1f; //加速時はブレーキ弱める

        }

        // targetFwdAngle = Mathf.Clamp(targetFwdAngle, -45f, 45f);
        // targetRgtAngle = Mathf.Clamp(targetRgtAngle, -45f, 45f);

        if (Input.GetKey(KeyCode.B))   //急ブレーキ
        {
            targetFwdAngle = 60f * Mathf.Deg2Rad * forwardSPD[1] / speed;
            targetRgtAngle = 60f * Mathf.Deg2Rad * rightSPD / speed;

        }

        targetFwd = Mathf.Sin(targetFwdAngle);    //ピッチ姿勢制御
        FwdY[1] = forward.y;
        controlPOW = Kp * (FwdY[1] - targetFwd) + 0.5f * decay * (FwdY[1] - FwdY[0]);
        Blade[0].power -= controlPOW;
        Blade[1].power += controlPOW;
        Blade[2].power += controlPOW;
        Blade[3].power -= controlPOW;

        targetRgt = Mathf.Sin(targetRgtAngle);    //ロール姿勢制御
        RgtY[1] = right.y;
        controlPOW = Kp * (RgtY[1] - targetRgt) + 0.5f * decay * (RgtY[1] - RgtY[0]);
        Blade[0].power += controlPOW;
        Blade[1].power += controlPOW;
        Blade[2].power -= controlPOW;
        Blade[3].power -= controlPOW;

        for (int i = 0; i < BladeNum; i++)
        {
            Blade[i].power = Mathf.Clamp(Blade[i].power, -4f, 4f);
        }

        forwardSPD[0] = forwardSPD[1];
        height[0] = height[1];
        FwdY[0] = FwdY[1];
        RgtY[0] = RgtY[1];

        modifiedPosition.x = Mathf.Clamp(this.transform.position.x, minPosi.x, maxPosi.x);
        modifiedPosition.y = Mathf.Clamp(this.transform.position.y, minPosi.y, maxPosi.y);
        modifiedPosition.z = Mathf.Clamp(this.transform.position.z, minPosi.z, maxPosi.z);

        if (modifiedPosition != this.transform.position)
        {
            this.transform.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this.GetComponent<Renderer>().material = powerMaterial;
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            this.GetComponent<Renderer>().material = defaultMaterial;
        }

        if (Input.GetKey(KeyCode.K)){
            Blade[0].power = -100f;
            Blade[1].power = 100f;
            Blade[2].power = 100f;
            Blade[3].power = -100f;
        }

}

    void FixedUpdate()
    {
        for(int i=0; i<BladeNum; i++)
        {
            rbody.AddForceAtPosition(this.transform.up * coeff * Blade[i].power, Blade[i].tf.position);
            Blade[i].tf.Rotate(new Vector3(0, 100 * Blade[i].power * Blade[i].rotDirection, 0));
            // Debug.Log(Blade[i].power);
        }

        // 指定軸まわりに回転させるQuaternionを作成
        Quaternion rot = Quaternion.AngleAxis(20f*yawDiff, this.transform.up);
        // 自身に乗じることで回転
        this.transform.rotation *= rot;

    }

    
}
