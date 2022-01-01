using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehavior : MonoBehaviour
{
    Animator animator;
    public GameObject bulletPrefab;
    Transform jawTf;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        jawTf = this.transform.Find("Root_Pelvis/Spine/Chest/Neck/Head/Jaw/JawTip");
        StartCoroutine(mainCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        this.transform.position += this.transform.forward*0.1f;
        this.transform.Rotate(new Vector3(0f, 0.1f, 0f));
    }

    IEnumerator mainCoroutine()
    {
        while (true) {

            animator.SetTrigger("Fire");

            StartCoroutine(fireCoroutine());

            //待機
            yield return new WaitForSeconds(5f);
            
        }

    }

    IEnumerator fireCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        Vector3 direction = jawTf.forward;
        GameObject clone = Instantiate(bulletPrefab) as GameObject;
        clone.transform.position = jawTf.position + direction;
        clone.GetComponent<Rigidbody>().velocity = direction*20f;
        Destroy(clone, 5f);
    }
}
