using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour
{
    public GameObject BombPrefab;
    public float speed = 10f;
    Vector3 targetVelocity;

    private GameObject targetObj;
    private int dropCounter;

    Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("DropFlag=" + dropCounter);
        if(targetObj == null)
        {
            // targetObj = GameObject.FindWithTag("Building");
            GameObject[] Buildings = GameObject.FindGameObjectsWithTag ("Building");

            if (Buildings.Length > 0)
            {
                targetObj = Buildings[Random.Range(0, Buildings.Length)];
            }
            
            dropCounter = 0;

        }
        else
        {
            Vector3 relation = targetObj.transform.position - this.transform.position;
            Vector3 direction = new Vector3(relation.x, 20f - this.transform.position.y, relation.z);

            float targetSpeed = Mathf.Clamp(direction.magnitude, 0f, speed);
            targetVelocity = targetSpeed * direction.normalized;
            
            if((direction.magnitude < 2f) && (dropCounter <= 0))
            {
                GameObject bomb = Instantiate(BombPrefab);
                bomb.transform.position = this.transform.position + 3f*Vector3.down;
                Destroy(bomb, 3.0f);
                dropCounter = 120;
            }

            this.transform.rotation = Quaternion.LookRotation(direction);    //向きベクトルを与えて回転

            dropCounter -= 1;
        }

    }

    void FixedUpdate()
    {
        rbody.AddForce(0.1f*(targetVelocity - rbody.velocity));
    }
}
