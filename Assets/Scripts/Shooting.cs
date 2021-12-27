using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    // bullet prefab
    public GameObject bullet;
 
    // 弾丸の速度
    public float defaultSpeed = 50;
    float targetSpeed;
    private GameObject cameraObj;
    CameraMove CMscript;

    private Rigidbody thisRbody;

    public GameObject simulatorObj; // 弾道予測オブジェクト
    private BallSimulator _ballSimurator; // 弾道予測線
 
	// Use this for initialization
	void Start () {
		cameraObj = GameObject.Find("Main Camera");
        CMscript = cameraObj.GetComponent<CameraMove>();
        thisRbody = this.GetComponent<Rigidbody>();

        _ballSimurator = simulatorObj.GetComponent<BallSimulator>();

        targetSpeed = defaultSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 direction = (CMscript.isFPS)? cameraObj.transform.forward : this.transform.forward;
        Vector3 shotVelocity = direction * targetSpeed;
        // if(CMscript.isFPS) _ballSimurator.Simulate(this.transform.position , shotVelocity);
        // _ballSimurator.Simulate(this.transform.position , shotVelocity);

        if (Input.GetKey(KeyCode.C)){
            targetSpeed += 1f;
            targetSpeed = Mathf.Min(targetSpeed, 500f);
            _ballSimurator.Simulate(this.transform.position , shotVelocity);
        }

        // キーが押された時
        if (Input.GetKeyUp(KeyCode.C)){
            // 弾丸の複製
            GameObject clone = Instantiate(bullet) as GameObject;

            clone.transform.position = this.transform.position + direction;

            clone.GetComponent<Rigidbody>().velocity = shotVelocity;

            //反動
            thisRbody.AddForce(-direction, ForceMode.Impulse);
 
            //発射されてから3秒後に銃弾のオブジェクトを破壊する.
            Destroy(clone, 5.0f);

            targetSpeed = defaultSpeed;

        }
		
	}
}
