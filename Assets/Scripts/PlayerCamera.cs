using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerObj;
    Transform playerTf;
    Shooting shooting;

    public GameObject AimObj;

    Camera cam;

    Vector3 back_direction;
    Vector3 direction;
    Vector3 relation;

    float xzOffset;
    float yOffset;
    float signSide;
    float signTop;

    struct PolarCoordinates
    {
        public float r;
        public float theta;
        public float phi;
    };

    PolarCoordinates PCDN;

    //private Vector3 axis;
    public float speed = 2;

    public bool reverce = false;

    bool isFPS;

    float lastDistance;
    float defaultFOV;


    // Start is called before the first frame update
    void Start()
    {
        playerTf = playerObj.transform;
        shooting = playerObj.GetComponent<Shooting>();

        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;

        relation = this.transform.position - playerTf.position;
        xzOffset = new Vector2(relation.x, relation.z).magnitude;
        yOffset = relation.y;

        PCDN = Rect2Polar(relation);

    }

    // Update is called once per frame
    void Update()
    {
        signSide = Input.GetAxis ("Horizontal");
        if(!isFPS){
            signSide *= (reverce)? -1 : 1;
        }
        PCDN.phi -= 0.01f * speed * signSide;

        signTop = Input.GetAxis ("Vertical");
        //axis = this.transform.right;
        PCDN.theta -= 0.01f * speed * signTop;
        PCDN.theta = Mathf.Clamp(PCDN.theta, 0.1f, 0.9f*Mathf.PI);


        if(Input.GetKey(KeyCode.L))
        {
            if(!(isFPS))
            {
                back_direction = - playerTf.forward;
                relation = new Vector3(back_direction.x, 0, back_direction.z);
                relation = relation.normalized * xzOffset + new Vector3(0, yOffset, 0);
                PCDN = Rect2Polar(relation);
                // this.transform.rotation = Quaternion.Euler(60,Target.transform.localEulerAngles.y,0);
            }
            else
            {
                relation = playerTf.forward;
                PCDN = Rect2Polar(relation);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Return)) SwitchFPS();  //Return:EnterKey

        //if (Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    doubleDownCount++;
        //    Invoke("SwitchFPS",0.3f);
        //}

        if(isFPS){
            float view = cam.fieldOfView - 0.5f*Input.GetAxis("Ctrl-Shift_Right");
            cam.fieldOfView = Mathf.Clamp(value : view, min : 1f, max : 90f);

            shooting.direction = this.transform.forward;
        }
        else{
            PCDN.r -= 0.5f*Input.GetAxis("Ctrl-Shift_Right");
            PCDN.r = Mathf.Clamp(PCDN.r, 0.5f, 100f);
        }

        relation = Polar2Rect(PCDN);
        // Debug.Log(relation);

        this.transform.position = playerTf.position + relation;
        direction = (isFPS)? relation : -relation;
        this.transform.rotation = Quaternion.LookRotation(direction);    //向きベクトルを与えて回転

    }

    PolarCoordinates Rect2Polar(Vector3 rectangular)
    {
        PolarCoordinates polar;

        polar.theta = Mathf.Acos(rectangular.y / rectangular.magnitude);
        polar.phi = Mathf.Atan2(rectangular.z, rectangular.x); //xがゼロの場合も考慮するArcTangent
        polar.r = rectangular.magnitude;

        return polar;
    }

    Vector3 Polar2Rect(PolarCoordinates polar)
    {
        Vector3 rectangular;

        rectangular.y = polar.r * Mathf.Cos(polar.theta);
        rectangular.x = polar.r * Mathf.Sin(polar.theta) * Mathf.Cos(polar.phi);
        rectangular.z = polar.r * Mathf.Sin(polar.theta) * Mathf.Sin(polar.phi);

        return rectangular;
    }

    void SwitchFPS()
    {
        //bool switchFlag = (doubleDownCount == 2)? true : false;
        //doubleDownCount = 0;
        //if(!switchFlag) { return; }

        isFPS = ! isFPS;
        Vector3 lastRelation = this.transform.position - playerTf.position;

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
        
        PCDN = Rect2Polar(relation);

        cam.fieldOfView = defaultFOV;

        
    }
}
