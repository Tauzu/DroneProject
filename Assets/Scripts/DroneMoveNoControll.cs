using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveNoControll : MonoBehaviour
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
    private float[] forward_angle = new float[2];
    private float[] right_angle = new float[2];

    private float inputVert;
    private float inputHori;

    public Vector3 minPosi = new Vector3(-1.0e2f, -1.0e1f, -2.0e1f);
    public Vector3 maxPosi = new Vector3(1.0e2f, 1.0e4f, 2.0e2f);

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool  is_hovering;
    
    private float height_hov;

    private float target_spd;
    private float difference;
    private float forward_spd;
    private float right_spd;
    private float control_pow;

    private float speed = 10f;
    private float coeff = 10f;
    // private float Kp = 1f;
    // private float decay = 10f;

    private GameObject hovering_text;

    public GameObject winnerLabelObject;

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
        forward_angle[0] = Mathf.Asin(this.transform.forward.y);
        right_angle[0] = Mathf.Asin(this.transform.right.y);

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

        forward_spd = Vector3.Dot(velocity, forward);
        target_spd = (Input.GetKey(KeyCode.P))? 3*speed*inputVert : speed*inputVert;
        difference = (target_spd - forward_spd)/speed * Mathf.Abs(inputVert*(target_spd - forward_spd)/speed);
        difference /= (Mathf.Abs(inputHori*inputVert) < 0.1f)? 1f : 1.4f;
        Blade[0].power -= difference;
        Blade[1].power += difference;
        Blade[2].power += difference;
        Blade[3].power -= difference;

        right_spd = Vector3.Dot(velocity, right);
        target_spd = (Input.GetKey(KeyCode.P))? 3*speed*inputHori : speed*inputHori;
        difference = (target_spd - right_spd)/speed * Mathf.Abs(inputHori*(target_spd - right_spd)/speed);
        difference /= (Mathf.Abs(inputHori*inputVert) < 0.1f)? 1f : 1.4f;
        Blade[0].power += difference;
        Blade[1].power += difference;
        Blade[2].power -= difference;
        Blade[3].power -= difference;

        for(int i=0; i<BladeNum; i++)
        {
            Blade[i].power = Mathf.Clamp(Blade[i].power, 0f, 1.5f);
        }
        
        // Debug.Log(target_spd);

        if(Input.GetKey(KeyCode.B))   //急ブレーキ
        {
            Blade[0].power += (forward_spd - right_spd)/speed;
            Blade[1].power += (-forward_spd - right_spd)/speed;
            Blade[2].power += (-forward_spd + right_spd)/speed;
            Blade[3].power += (forward_spd + right_spd)/speed;

        }


        if(Input.GetKey(KeyCode.U))   //上昇
        {
            for(int i=0; i<BladeNum; i++)
            {
                Blade[i].power = 1;
            }

            height_hov = height[1];

        }

        if(Input.GetKey(KeyCode.R))   //下降
        {
            for(int i=0; i<BladeNum; i++)
            {
                Blade[i].power = -1;
            }

            height_hov = height[1];

        }

        // 指定軸まわりに毎秒2度、回転させるQuaternionを作成
        Quaternion rot = Quaternion.AngleAxis(2*Input.GetAxis("V-C"), this.transform.up);
        // 自身に乗じることで回転
        this.transform.rotation *= rot;

        if(Input.GetKeyDown(KeyCode.F))   //前を向く
        {
            rbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }


        // if(Input.GetKeyDown(KeyCode.H))   //ホバリングスイッチ
        // {
        //     height_hov = height[1];
        //     is_hovering = (is_hovering)? false : true;
        //     hovering_text.SetActive(is_hovering);
        // }

        // if(is_hovering)    //ホバリング処理
        // {
        //     for(int i=0; i<BladeNum; i++)
        //     {
        //         // Blade[i].power -= decay*(height[1] - height[0]);
        //         Blade[i].power -= Kp*(height[1] - height_hov) + decay*(height[1] - height[0]);

        //     }

        //     if(Mathf.Abs(inputVert) < 0.1f)   //前後方向自動ブレーキ
        //     {
        //         control_pow = 0.5f*forward_spd/speed;
        //         Blade[0].power += control_pow;
        //         Blade[1].power -= control_pow;
        //         Blade[2].power -= control_pow;
        //         Blade[3].power += control_pow;
        //     }

        //     if(Mathf.Abs(inputHori) < 0.1f)   //左右方向自動ブレーキ
        //     {
        //         control_pow = 0.5f*right_spd/speed;
        //         Blade[0].power -= control_pow;
        //         Blade[1].power -= control_pow;
        //         Blade[2].power += control_pow;
        //         Blade[3].power += control_pow;
        //     }

        // }
        
        // forward_angle[1] = Mathf.Asin(forward.y);    //姿勢制御
        // control_pow = Kp*forward_angle[1] + decay*(forward_angle[1] - forward_angle[0]);
        // Blade[0].power -= control_pow;
        // Blade[1].power += control_pow;
        // Blade[2].power += control_pow;
        // Blade[3].power -= control_pow;    

        
        // right_angle[1] = Mathf.Asin(right.y);    //姿勢制御
        // control_pow = Kp*right_angle[1] + decay*(right_angle[1] - right_angle[0]);
        // Blade[0].power += control_pow;
        // Blade[1].power += control_pow;
        // Blade[2].power -= control_pow;
        // Blade[3].power -= control_pow;

        height[0] = height[1];
        forward_angle[0] = forward_angle[1];
        right_angle[0] = right_angle[1];

        modifiedPosition.x = Mathf.Clamp(this.transform.position.x, -1.0e2f, 1.0e2f);
        modifiedPosition.y = Mathf.Clamp(this.transform.position.y, -1.0e1f, 1.0e4f);
        modifiedPosition.z = Mathf.Clamp(this.transform.position.z, -2.0e1f, 2.0e2f);

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
            Blade[i].tf.Rotate(new Vector3(0, 50 * Blade[i].power, 0));
        }

    }

    void OnCollisionEnter(Collision other)//  他のオブジェクトに触れた時の処理
    {
        if (other.gameObject.tag == "Goal")//  もしGoalというタグがついたオブジェクトに触れたら、
        {
            winnerLabelObject.SetActive(true);
        }
    }
}
