using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレーヤー追従カメラの挙動。
//極座標を用いてプレーヤーの周囲の座標を決定する。

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerObj;
    Transform playerTf;
    Shooting shooting;

    public GameObject AimObj;

    Camera cam;

    float xzOffset;
    float yOffset;

    //極座標構造体
    struct PolarCoordinates
    {
        float theta;
        float phi;
        float r;

        public PolarCoordinates(Vector3 rectangular)
        {
            this.theta = Mathf.Acos(rectangular.y / rectangular.magnitude);
            this.phi = Mathf.Atan2(rectangular.z, rectangular.x); //xがゼロの場合も考慮するArcTangent
            this.r = rectangular.magnitude;
        }

        public PolarCoordinates Delta(float dtheta, float dphi, float dr)
        {
            PolarCoordinates polar;

            polar.theta = Mathf.Clamp(this.theta + dtheta, 0.1f, 0.9f * Mathf.PI);
            polar.phi = this.phi + dphi;
            polar.r = Mathf.Clamp(this.r + dr, 0.5f, 100f); 

            return polar;
        }

        public Vector3 Rectagular()
        {
            Vector3 rectangular;

            rectangular.y = this.r * Mathf.Cos(this.theta);
            rectangular.x = this.r * Mathf.Sin(this.theta) * Mathf.Cos(this.phi);
            rectangular.z = this.r * Mathf.Sin(this.theta) * Mathf.Sin(this.phi);

            return rectangular;
        }
    };

    PolarCoordinates PCDN;

    //private Vector3 axis;
    public float speed = 2;

    public bool reverce = false;

    bool isFPS = false;

    float lastDistance;
    float defaultFOV;

    // Start is called before the first frame update
    void Start()
    {
        playerTf = playerObj.transform;
        shooting = playerObj.GetComponent<Shooting>();

        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;

        Vector3 relation = this.transform.position - playerTf.position;
        xzOffset = new Vector2(relation.x, relation.z).magnitude;
        yOffset = relation.y;

        PCDN = new PolarCoordinates(relation);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Vector3 relation;

            if (!(isFPS))
            {
                Vector3 back_direction = - playerTf.forward;
                relation = new Vector3(back_direction.x, 0, back_direction.z);
                relation = relation.normalized * xzOffset + new Vector3(0, yOffset, 0);

                // this.transform.rotation = Quaternion.Euler(60,Target.transform.localEulerAngles.y,0);
            }
            else
            {
                relation = playerTf.forward;
            }

            PCDN = new PolarCoordinates(relation);

        }
        else
        {
            float dphi = -0.01f * speed * Input.GetAxis("Horizontal");
            if (!isFPS && reverce) dphi *= -1f;

            float dtheta = -0.01f * speed * Input.GetAxis("Vertical");

            float dr;
            float inputZoom = Input.GetAxis("Ctrl-Shift_Right");
            if (isFPS)
            {
                float view = cam.fieldOfView - 0.5f * inputZoom;
                cam.fieldOfView = Mathf.Clamp(value: view, min: 1f, max: 90f);

                shooting.direction = this.transform.forward;

                dr = 0f;
            }
            else
            {
                dr = -0.5f * inputZoom;
            }

            PCDN = PCDN.Delta(dtheta, dphi, dr);

        }

        if (Input.GetKeyDown(KeyCode.Return)) SwitchFPS();  //Return:EnterKey

        //if (Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    doubleDownCount++;
        //    Invoke("SwitchFPS",0.3f);
        //}

        // Debug.Log(relation);

    }

    // カメラの制御なのでLateUpdate
    private void LateUpdate()
    {
        Vector3 relation = PCDN.Rectagular();
        this.transform.position = playerTf.position + relation;
        Vector3 direction = (isFPS) ? relation : -relation;
        this.transform.rotation = Quaternion.LookRotation(direction);
    }


    void SwitchFPS()
    {
        //bool switchFlag = (doubleDownCount == 2)? true : false;
        //doubleDownCount = 0;
        //if(!switchFlag) { return; }

        isFPS = ! isFPS;
        Vector3 lastRelation = this.transform.position - playerTf.position;

        Vector3 relation;
        if (isFPS)
        {
            relation = - lastRelation.normalized;
            lastDistance = lastRelation.magnitude;//FPS切替時、切替前の距離を記憶しておく
            shooting.freeDirection = false;
            AimObj.SetActive(true);
        }
        else
        {
            relation = - lastDistance * lastRelation;
            shooting.freeDirection = true;
            AimObj.SetActive(false);
        }
        
        PCDN = new PolarCoordinates(relation);

        cam.fieldOfView = defaultFOV;

        
    }
}
