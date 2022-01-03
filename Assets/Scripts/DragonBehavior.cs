using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehavior : MonoBehaviour
{
    Animator animator;
    public GameObject firePrefab;
    Transform jawTf;

    Renderer dragonRend;
    Color defaultColor;
    int HP = 100;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jawTf = this.transform.Find("Root_Pelvis/Spine/Chest/Neck/Head/Jaw/JawTip");

        dragonRend = this.transform.Find("DragonSoulEater").GetComponent<Renderer>();
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
            clone.transform.position = standardPosition + direction*(float)(5*i+5) 
                + new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));

            // clone.GetComponent<Rigidbody>().velocity = direction * 20f;
            Destroy(clone, 30f);
        }

    }

    public void HitDamage(int damage)
    {
        StartCoroutine(GetRed());
        HP -= damage;
        if (HP > 0)
        {
            animator.SetTrigger("Damage");
        }
        else
        {
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
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);

    }


}
