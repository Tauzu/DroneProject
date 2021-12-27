using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveNoYaw : MonoBehaviour
{
    private Rigidbody rbody;//  Rigidbodyを使うための変数

    struct BladeParam
    {
        public Transform tf;
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

    private float inputVert;
    private float inputHori;

    public Vector3 minPosi = new Vector3(-1.0e2f, -1.0e1f, -2.0e1f);
    public Vector3 maxPosi = new Vector3(1.0e2f, 1.0e4f, 2.0e2f);

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool  isHovering;
    
    private float heightHOV;

    private float targetSPD;
    private float difference;
    private float forwardSPD;
    private float rightSPD;
    private float controlPOW;

    private float targetFwdAngle;
    private float targetRgtAngle;

    private float targetFwd;
    private float targetRgt;

    private float speed = 10f;
    private float coeff = 10f;
    private float Kp = 1f;
    private float decay = 10f;

    private GameObject hovering_text;

    // private GameObject cameraObj;
    // private Vector3 cameraFwd;
    // private Vector2 forwardXZ;
    // private Vector2 rightXZ;
    // private Vector2 cameraFwdXZ;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();

        Blade[0].tf = this.transform.Find("blade1");
        Blade[0].key = "t";

        Blade[1].tf = this.transform.Find("blade2");
        Blade[1].key = "g";

        Blade[2].tf = this.transform.Find("blade3");
        Blade[2].key = "j";

        Blade[3].tf = this.transform.Find("blade4");
        Blade[3].key = "i";

        hovering_text = this.transform.Find("hovering_text").gameObject;

        height[0] = this.transform.position.y;
        FwdY[0] = this.transform.forward.y;
        RgtY[0] = this.transform.right.y;

        // cameraObj = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(this.transform.position);
        height[1] = this.transform.position.y;
        velocity = rbody.velocity;
        forward = this.transform.forward;
        right = this.transform.right;

        for(int i=0; i<BladeNum; i++)   //対応キー出力
        {
            Blade[i].power = (Input.GetKey(Blade[i].key)) ? 1 : 0;//  もし、キーがおされているなら、 
        }

        inputVert = Input.GetAxis("Vertical");
        inputHori = Input.GetAxis("Horizontal");

        forwardSPD = Vector3.Dot(velocity, forward);
        targetSPD = (Input.GetKey(KeyCode.P))? 3*speed*inputVert : speed*inputVert;
        difference = (targetSPD - forwardSPD)/speed * Mathf.Abs(inputVert);
        targetFwdAngle = -40*Mathf.Deg2Rad * difference;

        rightSPD = Vector3.Dot(velocity, right);
        targetSPD = (Input.GetKey(KeyCode.P))? 3*speed*inputHori : speed*inputHori;
        difference = (targetSPD - rightSPD)/speed * Mathf.Abs(inputHori);
        targetRgtAngle =　-40*Mathf.Deg2Rad * difference;
        
        // Debug.Log(targetSPD);

        if(Input.GetKey(KeyCode.B))   //急ブレーキ
        {
            targetFwdAngle = 60f*Mathf.Deg2Rad *forwardSPD/speed;
            targetRgtAngle = 60f*Mathf.Deg2Rad *rightSPD/speed;

        }


        if(Input.GetKey(KeyCode.U))   //上昇
        {
            for(int i=0; i<BladeNum; i++)
            {
                Blade[i].power = 1;
            }

            heightHOV = height[1];

        }

        if(Input.GetKey(KeyCode.R))   //下降
        {
            for(int i=0; i<BladeNum; i++)
            {
                Blade[i].power = -1;
            }

            heightHOV = height[1];

        }

        // cameraFwd = cameraObj.transform.forward;
        // forwardXZ = new Vector2(forward.x, forward.z).normalized;
        // rightXZ = new Vector2(right.x, right.z).normalized;
        // cameraFwdXZ = new Vector2(cameraFwd.x, cameraFwd.z).normalized;
        // difference = Mathf.Abs(inputVert * (inputVert - Vector2.Dot(forwardXZ, cameraFwdXZ)));
        // difference *= (Vector2.Dot(rightXZ, cameraFwdXZ) > 0)? -1 : 1;

        // 指定軸まわりに回転させるQuaternionを作成
        Quaternion rot = Quaternion.AngleAxis(Input.GetAxis("V-C"), this.transform.up);
        // 自身に乗じることで回転
        this.transform.rotation *= rot;

        if(Input.GetKeyDown(KeyCode.F))   //z軸方向を向く
        {
            rbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }


        if(Input.GetKeyDown(KeyCode.H))   //ホバリングスイッチ
        {
            heightHOV = height[1];
            isHovering = (isHovering)? false : true;
            hovering_text.SetActive(isHovering);
        }

        if(isHovering)    //ホバリング処理
        {
            for(int i=0; i<BladeNum; i++)
            {
                // Blade[i].power -= decay*(height[1] - height[0]);
                Blade[i].power -= Kp*(height[1] - heightHOV) + decay*(height[1] - height[0]);

            }

            if(Mathf.Abs(inputVert) < 0.1f)   //前後方向自動ブレーキ
            {
                targetFwdAngle = 30f*Mathf.Deg2Rad *forwardSPD/speed;
            }

            if(Mathf.Abs(inputHori) < 0.1f)   //左右方向自動ブレーキ
            {
                targetRgtAngle = 60f*Mathf.Deg2Rad *rightSPD/speed;
            }

        }
     
        targetFwdAngle = Mathf.Clamp(targetFwdAngle, -45f, 45f);
        targetRgtAngle = Mathf.Clamp(targetRgtAngle, -45f, 45f);
        
        targetFwd = Mathf.Sin(targetFwdAngle);    //姿勢制御
        FwdY[1] = forward.y;
        controlPOW = Kp*(FwdY[1] - targetFwd) + decay*(FwdY[1] - FwdY[0]);
        Blade[0].power -= controlPOW;
        Blade[1].power += controlPOW;
        Blade[2].power += controlPOW;
        Blade[3].power -= controlPOW;    
        
        targetRgt = Mathf.Sin(targetRgtAngle);    //姿勢制御
        RgtY[1] = right.y;
        controlPOW = Kp*(RgtY[1] - targetRgt) + decay*(RgtY[1] - RgtY[0]);
        Blade[0].power += controlPOW;
        Blade[1].power += controlPOW;
        Blade[2].power -= controlPOW;
        Blade[3].power -= controlPOW;

        height[0] = height[1];
        FwdY[0] = FwdY[1];
        RgtY[0] = RgtY[1];

        modifiedPosition.x = Mathf.Clamp(this.transform.position.x, minPosi.x, maxPosi.x);
        modifiedPosition.y = Mathf.Clamp(this.transform.position.y, minPosi.y, maxPosi.y);
        modifiedPosition.z = Mathf.Clamp(this.transform.position.z, minPosi.z, maxPosi.z);

        if(modifiedPosition != this.transform.position)
        {
            this.transform.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }

    }

    void FixedUpdate()
    {
        for(int i=0; i<BladeNum; i++)
        {
            rbody.AddForceAtPosition(this.transform.up * coeff * Blade[i].power, Blade[i].tf.position);
            Blade[i].tf.Rotate(new Vector3(0, 100 * Blade[i].power, 0));
        }

    }

    
}
