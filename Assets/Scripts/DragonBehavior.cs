using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;    //Rendering用

public class DragonBehavior : MonoBehaviour
{
    Animator animator;
    public GameObject firePrefab;
    Transform jawTf;

    SkinnedMeshRenderer dragonRend;
    Color defaultColor;
    int HP = 100;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jawTf = this.transform.Find("Root_Pelvis/Spine/Chest/Neck/Head/Jaw/JawTip");

        dragonRend = this.transform.Find("DragonSoulEater").GetComponent<SkinnedMeshRenderer>();
        defaultColor = dragonRend.material.color;
        StartCoroutine(mainCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        AnimatorStateInfo ASInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (ASInfo.IsName("Fly Float"))
        {
            this.transform.position += this.transform.forward*0.1f;
            this.transform.Rotate(new Vector3(0f, 0.1f, 0f));
        }

    }

    IEnumerator mainCoroutine()
    {
        while (true) {

            animator.SetTrigger("Fire");

            StartCoroutine(fireCoroutine());

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
            clone.transform.parent = this.gameObject.transform;
            clone.transform.position = standardPosition + direction*(float)(5*i+5) 
                + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));

            // clone.GetComponent<Rigidbody>().velocity = direction * 20f;
            Destroy(clone, 30f);
        }

    }

    public void HitDamage(int damage)
    {
        HP -= damage;
        if (HP > 0)
        {
            StartCoroutine(GetRed());
            animator.SetTrigger("Damage");
        }
        else if(!dead)
        {
            dead = true;
            animator.SetTrigger("Die");
            StartCoroutine(DestroyProcess());
        }

        Debug.Log("HP=" + HP);


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
            // Debug.Log(dragonRend.material.color.a);
            dragonRend.material.color -= new Color(0, 0, 0, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }

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
