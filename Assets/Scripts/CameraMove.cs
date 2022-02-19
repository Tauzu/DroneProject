using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform targetTf;

    public GameObject AimObj;

    private Camera cam;

    private Vector3 back_direction;
    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 relation;
    //private Vector3 relationIni;

    private float xzOffset;
    private float yOffset;
    //private float distanceIni;
    private float signSide;
    private float signTop;

    //int doubleDownCount = 0;

    struct PolarCoordinates
    {
        public float r;
        public float theta;
        public float phi;
    };

    private PolarCoordinates cameraPCDN;

    //private Vector3 axis;
    public float speed = 2;

    public bool Reverce;

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool isFPS;

    private float normalDistance;
    private float defaultFOV;


    // Start is called before the first frame update
    void Start()
    {
        targetTf = GameObject.Find("Player").transform;
        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;

        relation = this.transform.position - targetTf.position;
        xzOffset = new Vector2(relation.x, relation.z).magnitude;
        yOffset = relation.y;

        cameraPCDN = Rect2Polar(relation);

    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = targetTf.position;

        signSide = Input.GetAxis ("Horizontal");
        if(!isFPS){
            signSide *= (Reverce)? -1 : 1;
        }
        cameraPCDN.phi -= 0.01f * speed * signSide;

        signTop = Input.GetAxis ("Vertical");
        //axis = this.transform.right;
        cameraPCDN.theta -= 0.01f * speed * signTop;
        cameraPCDN.theta = Mathf.Clamp(cameraPCDN.theta, 0.1f, 0.9f*Mathf.PI);


        if(Input.GetKey(KeyCode.L))
        {
            if(!(isFPS))
            {
                back_direction = - targetTf.forward;
                relation = new Vector3(back_direction.x, 0, back_direction.z);
                relation = relation.normalized * xzOffset + new Vector3(0, yOffset, 0);
                cameraPCDN = Rect2Polar(relation);
                // this.transform.rotation = Quaternion.Euler(60,Target.transform.localEulerAngles.y,0);
            }
            else
            {
                relation = targetTf.forward;
                cameraPCDN = Rect2Polar(relation);
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
        }
        else{
            cameraPCDN.r -= 0.5f*Input.GetAxis("Ctrl-Shift_Right");
            cameraPCDN.r = Mathf.Clamp(cameraPCDN.r, 0.5f, 100f);
        }

        relation = Polar2Rect(cameraPCDN);
        // Debug.Log(relation);

        this.transform.position = targetPosition + relation;
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

        isFPS = (isFPS)? false : true;
        Vector3 lastRelation = this.transform.position - targetPosition;
        relation = (isFPS)? -lastRelation.normalized : -normalDistance*lastRelation;
        if(isFPS) normalDistance = lastRelation.magnitude;//FPS切替時、切替前の距離を記憶しておく
        cameraPCDN = Rect2Polar(relation);

        cam.fieldOfView = defaultFOV;

        AimObj.SetActive(isFPS);
    }
}
