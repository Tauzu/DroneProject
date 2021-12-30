using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmezonBoxBehavior : MonoBehaviour
{
    private GameObject targetObj;

    public GameObject targetEffect;
    private GameObject effectClone;
    Transform playerTf;
    public GameObject scorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        effectClone = Instantiate(targetEffect);
        // effectClone.transform.parent = this.transform;

        playerTf = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetObj == null)
        {
            // targetObj = GameObject.FindWithTag("Building");
            GameObject[] Buildings = GameObject.FindGameObjectsWithTag ("Building");

            if (Buildings.Length > 0)
            {
                targetObj = Buildings[Random.Range(0, Buildings.Length)];

                effectClone.transform.position = targetObj.transform.position + new Vector3(0f, 1f, 0f);

            }

        }

        Vector3 direction = this.transform.position - playerTf.position;
        if(direction.magnitude > 50f){
            Destroy(this.gameObject);
        }

    }

    void OnCollisionEnter(Collision other)//  他のオブジェクトに触れた時の処理
    {
        if (other.gameObject == targetObj)
        {
            GameObject clone = Instantiate(scorePrefab) as GameObject;
            clone.GetComponent<PopupScore>().SetScore(1000, this.transform.position + Vector3.up);

            Destroy(this.gameObject);
        }

    }

    void OnDestroy()
    {
        Destroy(effectClone);
    }

}
