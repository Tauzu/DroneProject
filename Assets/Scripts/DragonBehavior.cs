using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;    //Rendering用
using UnityEngine.UI;   //Slider用

public class DragonBehavior : MonoBehaviour
{
    Animator animator;
    public GameObject firePrefab;
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

    Vector3 relativePos;

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
    }

    // Update is called once per frame
    void Update()
    {
        isFloating = animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Float");

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
            relativePos = targetObj.transform.position - this.transform.position;
            relativePos = new Vector3(relativePos.x, 0f, relativePos.z);    //鉛直方向は無視

            //this.transform.LookAt(targetObj.transform);    //向きベクトルを与えて回転

        }
    }

    void FixedUpdate()
    {
        if (isFloating)
        {

            //this.transform.Rotate(new Vector3(0f, 0.1f, 0f));

            // 補完スピードを決める
            float speed = 0.1f;
            // 方向を、回転情報に変換
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);

            //Debug.Log(relativePos.magnitude);
            this.transform.position += this.transform.forward * Mathf.Clamp(relativePos.magnitude - 30f, -0.1f, 0.1f);

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
            //clone.transform.parent = this.gameObject.transform;
            clone.transform.position = standardPosition + direction*(float)(5*i+5) 
                + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));

            // clone.GetComponent<Rigidbody>().velocity = direction * 20f;
            Destroy(clone, 30f);
        }

    }

    public void HitDamage(int damage)
    {
        HP -= damage;
        slider.value = (float)HP / (float)100;
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

}
