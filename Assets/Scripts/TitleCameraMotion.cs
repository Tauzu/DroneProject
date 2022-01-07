using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraMotion : MonoBehaviour
{
    Vector3 center = new Vector3(42f, 0f, 35f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.RotateAround(center, Vector3.down, 0.1f);

    }
}
