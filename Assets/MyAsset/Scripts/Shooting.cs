using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾丸発射クラス

public class Shooting : MonoBehaviour
{
    // bullet prefab
    public GameObject bullet;
    public GameObject specialBullet;
 
    // 弾丸の速度
    public float defaultSpeed = 50;
    public float maxSpeed = 300;
    float targetSpeed;

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool isCharging;
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public float gradLocation;

    Rigidbody thisRbody;

    public BallSimulator ballSimurator; // 弾道予測線

    Renderer rend;
    Color defaultColor;
    Gradient grad = new Gradient();

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool isSpecial = false;

    GameObject SAParticle;
    float specialTimeLimit;

    AudioSource audioSrc;
    public AudioClip shotSE;

    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public bool freeDirection = true;
    [System.NonSerialized]  //publicだがインスペクター上には表示しない
    public Vector3 direction;

    // Use this for initialization
    void Start () {
        thisRbody = this.GetComponent<Rigidbody>();

        audioSrc = this.GetComponent<AudioSource>();

        targetSpeed = defaultSpeed;

        rend = this.transform.Find("BodyMesh").gameObject.GetComponent<Renderer>();
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

        if (freeDirection) { direction = this.transform.forward; }

        Vector3 shotVelocity = (! isSpecial)? direction * targetSpeed : direction * targetSpeed*2f;
        gradLocation = (targetSpeed - defaultSpeed) / (maxSpeed - defaultSpeed);
        // if(CMscript.isFPS) _ballSimurator.Simulate(this.transform.position , shotVelocity);
        // _ballSimurator.Simulate(this.transform.position , shotVelocity);

        // キーから指を離した時
        if (Input.GetKeyUp(KeyCode.Space)){ Shot(shotVelocity); }

        if (Input.GetKey(KeyCode.Space))
        {
            isCharging = true;

            rend.material.color = grad.Evaluate(gradLocation);
            ballSimurator.Simulate(this.transform.position + direction, shotVelocity, gradLocation);

            targetSpeed += 1f;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);

        }

        if (isSpecial){
            specialTimeLimit -= Time.deltaTime;
            // Debug.Log(specialTimeLimit);
            if (specialTimeLimit < 0f)
            {
                isSpecial = false;
                Destroy(SAParticle);
            }
        }
		
	}

    void Shot(Vector3 shotVelocity)
    {
        // 弾丸の複製
        GameObject clone = (!isSpecial) ? Instantiate(bullet) : Instantiate(specialBullet);

        clone.transform.position = this.transform.position + direction;

        clone.GetComponent<Rigidbody>().velocity = shotVelocity;

        //反動
        thisRbody.AddForce(-direction * targetSpeed * targetSpeed * 0.0002f, ForceMode.Impulse);

        //発射されてから3秒後に銃弾のオブジェクトを破壊する.
        Destroy(clone, 5.0f);

        targetSpeed = defaultSpeed;

        audioSrc.PlayOneShot(shotSE, Mathf.Max(Mathf.Sqrt(gradLocation), 0.3f));

        isCharging = false;
    }

    //SpecialAmezonParticleが子オブジェクトにあるかどうか1秒ごとにチェックする
    IEnumerator CheckSpecial()
    {
        while (true) {

            Transform SAParticleTf = this.transform.Find("SpecialAmezonParticle");
            if (SAParticleTf != null && SAParticle == null)//検索に成功し、かつ現在Particleを参照できていない場合
            {
                SAParticle = SAParticleTf.gameObject;
                isSpecial = true;
                specialTimeLimit = 30f;
            }

            //1秒ごとにチェック
            yield return new WaitForSeconds(1f);

        }
        
    }
}
