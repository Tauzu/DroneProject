using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraMotion : MonoBehaviour
{
    //GameObject mainCameraObj;
    public Transform lookingTf;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        //mainCameraObj = GameObject.Find("Main Camera");
        //mainCameraObj.SetActive(false);

        if(lookingTf==null) lookingTf = GameObject.Find("Player").transform;

        //this.transform.position = mainCameraObj.transform.position;
        //this.transform.rotation = mainCameraObj.transform.rotation;

        //Invoke("Deactivate", 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 direction = targetPosition - this.transform.position;
        this.transform.position += 0.05f * direction;

        this.transform.LookAt(lookingTf);
    }

    void OnDestroy()
    {
        //if(mainCameraObj != null) mainCameraObj.SetActive(true);
    }
}
