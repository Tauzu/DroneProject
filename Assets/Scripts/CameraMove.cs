using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject Target;

    public GameObject AimObj;

    private Camera cam;

    private Vector3 back_direction;
    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 relation;
    private Vector3 relationIni;

    private float xzOffset;
    private float yOffset;
    private float distanceIni;
    private float signSide;
    private float signTop;

    struct PolarCoordinates
    {
        public float r;
        public float theta;
        public float phi;
    };

    private PolarCoordinates cameraPCDN;

    private Vector3 axis;
    public float speed = 2;

    public bool Reverce;

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool isFPS;

    private float normalDistance;
    private float defaultFOV;


    // Start is called before the first frame update
    void Start()
    {
        // Target = GameObject.Find("Player");
        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;

        relation = this.transform.position - Target.transform.position;
        xzOffset = new Vector2(relation.x, relation.z).magnitude;
        yOffset = relation.y;

        cameraPCDN = Rect2Polar(relation);

    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = Target.transform.position;

        signSide = Input.GetAxis ("Horizontal");
        if(!isFPS){
            signSide *= (Reverce)? -1 : 1;
        }
        cameraPCDN.phi -= 0.01f * speed * signSide;

        signTop = Input.GetAxis ("Vertical");
        axis = this.transform.right;
        cameraPCDN.theta -= 0.01f * speed * signTop;
        cameraPCDN.theta = Mathf.Clamp(cameraPCDN.theta, 0.1f, 0.9f*Mathf.PI);


        if(Input.GetKey(KeyCode.L))
        {
            if(!(isFPS))
            {
                back_direction = - Target.transform.forward;
                relation = new Vector3(back_direction.x, 0, back_direction.z);
                relation = relation.normalized * xzOffset + new Vector3(0, yOffset, 0);
                cameraPCDN = Rect2Polar(relation);
                // this.transform.rotation = Quaternion.Euler(60,Target.transform.localEulerAngles.y,0);
            }
            else
            {
                relation = Target.transform.forward;
                cameraPCDN = Rect2Polar(relation);
            }
            
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            isFPS = (isFPS)? false : true;
            Vector3 lastRelation = this.transform.position - targetPosition;
            relation = (isFPS)? -lastRelation.normalized : -normalDistance*lastRelation;
            if(isFPS) normalDistance = lastRelation.magnitude;//FPS切替時、切替前の距離を記憶しておく
            cameraPCDN = Rect2Polar(relation);

            cam.fieldOfView = defaultFOV;

            AimObj.SetActive(isFPS);

        }

        if(isFPS){
            float view = cam.fieldOfView - 0.5f*Input.GetAxis("Ctrl-Shift_Right");
            cam.fieldOfView = Mathf.Clamp(value : view, min : 0.1f, max : 45f);
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
        polar.phi = Mathf.Atan2(rectangular.z, rectangular.x); //xがゼロの場合も考慮する関数
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
}
