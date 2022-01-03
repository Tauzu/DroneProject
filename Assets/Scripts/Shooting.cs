using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    // bullet prefab
    public GameObject bullet;
    public GameObject specialBullet;
 
    // 弾丸の速度
    public float defaultSpeed = 50;
    public float maxSpeed = 300;
    float targetSpeed;
    private GameObject cameraObj;
    CameraMove CMscript;

    private Rigidbody thisRbody;

    public GameObject simulatorObj; // 弾道予測オブジェクト
    private BallSimulator _ballSimurator; // 弾道予測線

    Renderer rend;
    Color defaultColor;
    Gradient grad = new Gradient();

    GameObject SAParticle;
    float specialTimeLimit;

    // Use this for initialization
    void Start () {
		cameraObj = GameObject.Find("Main Camera");
        CMscript = cameraObj.GetComponent<CameraMove>();
        thisRbody = this.GetComponent<Rigidbody>();

        _ballSimurator = simulatorObj.GetComponent<BallSimulator>();

        targetSpeed = defaultSpeed;

        rend = this.transform.Find("BoxBody").gameObject.GetComponent<Renderer>();
        defaultColor = rend.material.color;

        var colorKey = new GradientColorKey[2];
        colorKey[0].color = defaultColor;
        colorKey[0].time = 0f;
        colorKey[1].color = Color.red;
        colorKey[1].time = 1f;
        var alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 0f;
        alphaKey[1].time = 1f;
        grad.SetKeys(colorKey, alphaKey);

        StartCoroutine(CheckSpecial());
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 direction = (CMscript.isFPS)? cameraObj.transform.forward : this.transform.forward;
        Vector3 shotVelocity = (SAParticle == null)? direction * targetSpeed : direction * targetSpeed*2f;
        // if(CMscript.isFPS) _ballSimurator.Simulate(this.transform.position , shotVelocity);
        // _ballSimurator.Simulate(this.transform.position , shotVelocity);

        if (Input.GetKey(KeyCode.Z)){
            targetSpeed += 1f;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);
            float gradLocation = (targetSpeed - defaultSpeed) / (maxSpeed - defaultSpeed);
            rend.material.color = grad.Evaluate(gradLocation);
            _ballSimurator.Simulate(this.transform.position , shotVelocity, gradLocation);

        }

        // キーから指を離した時
        if (Input.GetKeyUp(KeyCode.Z)){
            // 弾丸の複製
            GameObject clone = (SAParticle == null)?  Instantiate(bullet) as GameObject
                 : Instantiate(specialBullet) as GameObject;

            clone.transform.position = this.transform.position + direction;

            clone.GetComponent<Rigidbody>().velocity = shotVelocity;

            //反動
            thisRbody.AddForce(-direction*targetSpeed*targetSpeed*0.0002f, ForceMode.Impulse);
 
            //発射されてから3秒後に銃弾のオブジェクトを破壊する.
            Destroy(clone, 5.0f);

            targetSpeed = defaultSpeed;

        }

        if(SAParticle != null){
            specialTimeLimit -= Time.deltaTime;
            // Debug.Log(specialTimeLimit);
            if(specialTimeLimit < 0f) Destroy(SAParticle);
        }
		
	}

    IEnumerator CheckSpecial()
    {
        while (true) {

            Transform SAParticleTf = this.transform.Find("SpecialAmezonParticle");
            if (SAParticleTf != null && SAParticle == null)//検索に成功し、かつ現在Particleを参照できていない場合
            {
                SAParticle = SAParticleTf.gameObject;
                specialTimeLimit = 20f;
            }

            //待機
            yield return new WaitForSeconds(1f);

        }
        
    }
}
