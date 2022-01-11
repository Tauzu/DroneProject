using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraMotion : MonoBehaviour
{
    public GameObject centerObj;
    Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        center = centerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.RotateAround(center, Vector3.down, -0.1f);

    }
}
