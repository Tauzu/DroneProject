using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraMotion : MonoBehaviour
{
    GameObject mainCameraObj;
    GameObject playerObj;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");

        mainCameraObj = GameObject.Find("Main Camera");
        mainCameraObj.SetActive(false);

        this.transform.position = mainCameraObj.transform.position;
        this.transform.rotation = mainCameraObj.transform.rotation;

        //Invoke("Deactivate", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = targetPosition - this.transform.position;
        this.transform.position += 0.05f * direction;

        this.transform.LookAt(playerObj.transform);
    }

    void OnDestroy()
    {
        if(mainCameraObj != null) mainCameraObj.SetActive(true);
    }
}
