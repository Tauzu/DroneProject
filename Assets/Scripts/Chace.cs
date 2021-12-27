using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chace : MonoBehaviour
{
    private GameObject targetOB;
    public float distance = 4f;
    public float speed = 0.2f;
    private Vector3 relation;

    // Start is called before the first frame update
    void Start()
    {
        targetOB = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        relation = targetOB.transform.position - this.transform.position;

        if(relation.magnitude < distance)
        {
            this.transform.position += relation.normalized * speed;
            this.transform.rotation = Quaternion.LookRotation(relation);    //向きベクトルを与えて回転
        }
        
    }
}
