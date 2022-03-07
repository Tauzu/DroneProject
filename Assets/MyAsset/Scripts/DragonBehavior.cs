using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;    //Rendering用
using UnityEngine.UI;   //Slider用

//ドラゴンの挙動制御クラス
//Unityアセットストアの「FreeDragons」を借用している。
//プレハブ内のアニメーターを適宜切り替える。
//基本的にRigidBodyの物理演算で動く（重力無視）。

public class DragonBehavior : MonoBehaviour
{
    Animator animator;
    public GameObject firePrefab;
    public GameObject fireballParent;
    Transform jawTf;

    SkinnedMeshRenderer dragonRend;
    Color defaultColor;

    public GameObject HPSlider;
    Slider slider;
    int HP = 100;
    bool dead = false;

    GameObject targetObj;
    //GameObject particleObj;
    bool isFloating;

    public float speed = 10f;

    SphereCollider sphereCollider;
    Transform pelvisTf;

    Rigidbody rbody;

    float hitLimit = 0f;

    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jawTf = this.transform.Find("Root_Pelvis/Spine/Chest/Neck/Head/Jaw/JawTip");

        HPSlider.SetActive(true);
        slider = HPSlider.GetComponent<Slider>();

        dragonRend = this.transform.Find("DragonSoulEater").GetComponent<SkinnedMeshRenderer>();
        defaultColor = dragonRend.material.color;

        coroutine = breathCoroutine();
        StartCoroutine(coroutine);

        //particleObj = (GameObject)Resources.Load("TargetParticle");
        pelvisTf = this.transform.Find("Root_Pelvis");
        sphereCollider = this.GetComponent<SphereCollider>();
        rbody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        isFloating = animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Float");

        sphereCollider.center = pelvisTf.localPosition + Vector3.forward;

        hitLimit -= Time.deltaTime;

        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f));//回転をy軸に限定
    }

    void FixedUpdate()
    {
        if (isFloating)
        {
            if (targetObj == null)
            {
                // targetObj = GameObject.FindWithTag("Building");
                GameObject[] Buildings = GameObject.FindGameObjectsWithTag("Building");

                if (Buildings.Length > 0)
                {
                    targetObj = Buildings[Random.Range(0, Buildings.Length)];
                    //GameObject clone = Instantiate(particleObj);
                    //clone.transform.position = targetObj.transform.position;
                }


            }
            else
            {
                // ターゲット方向のベクトルを取得
                Vector3 relation = new Vector3(targetObj.transform.position.x, 10f, targetObj.transform.position.z)
                    - this.transform.position;
                Vector3 direction = relation - 30f * new Vector3(relation.x, 0f, relation.z).normalized;

                float targetSpeed = Mathf.Clamp(direction.magnitude, 0f, speed);
                Vector3 targetVelocity = targetSpeed * direction.normalized;

                //Debug.Log(relativePos.magnitude);
                //this.transform.position += this.transform.forward * Mathf.Clamp(relativePos.magnitude - 30f, -0.1f, 0.1f);
                rbody.AddForce(0.1f * (targetVelocity - rbody.velocity), ForceMode.Acceleration);

                // 方向を、回転情報に変換
                Quaternion rotation = Quaternion.LookRotation(relation.normalized);
                // 現在の回転情報と、ターゲット方向の回転情報を補完する
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 0.01f);

            }

        }

    }

    IEnumerator breathCoroutine()
    {
        while (true) {

            if (isFloating)
            {
                animator.SetTrigger("Fire");
                StartCoroutine(fireCoroutine());
            }

            //待機
            yield return new WaitForSeconds(10f);
            
        }

    }

    IEnumerator fireCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        Vector3 direction = new Vector3(jawTf.forward.x, 0f, jawTf.forward.z);
        Vector3 standardPosition = jawTf.position;
        standardPosition.y = Mathf.Max(40f, standardPosition.y);
        for (int i = 0; i < 5; i++)
        {
            GameObject clone = Instantiate(firePrefab) as GameObject;
            clone.transform.parent = fireballParent.transform;
            clone.transform.position = standardPosition + direction*(float)(5*i+5) 
                + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));

            // clone.GetComponent<Rigidbody>().velocity = direction * 20f;
            Destroy(clone, 30f);
        }

    }

    void HitDamage(int damage)
    {
        HP -= damage;
        slider.value = (float)HP / (float)100;
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;

        if (HP > 0)
        {
            StartCoroutine(GetRed());
            animator.SetTrigger("Damage");
        }
        else if(!dead)
        {
            dead = true;
            StopCoroutine(coroutine);
            animator.SetTrigger("Die");
            StartCoroutine(DestroyProcess());
        }

        //Debug.Log("HP=" + HP);


    }

    IEnumerator GetRed()
    {
        dragonRend.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        dragonRend.material.color = defaultColor;

    }

    IEnumerator DestroyProcess()
    {
        ToFadeMode(dragonRend.material);

        yield return new WaitForSeconds(1f);

        while(dragonRend.material.color.a > 0f){
            dragonRend.material.color -= new Color(0, 0, 0, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }

        HPSlider.SetActive(false);
        Destroy(fireballParent);
        Destroy(this.gameObject);

    }

    void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpecialBullet" && hitLimit < 0f)
        {
            float speed = other.attachedRigidbody.velocity.magnitude;
            HitDamage(Mathf.Max((int)(speed * speed * 0.0001f), 1));
            hitLimit = 0.5f;
        }
    }

}
