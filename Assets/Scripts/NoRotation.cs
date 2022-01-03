using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotation : MonoBehaviour
{
    Quaternion initialQuat;
    // Start is called before the first frame update
    void Start()
    {
        initialQuat = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = initialQuat;
    }
}
