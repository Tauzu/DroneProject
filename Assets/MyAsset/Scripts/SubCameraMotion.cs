using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraMotion : MonoBehaviour
{
    [System.NonSerialized]    //publicだがインスペクター上には表示しない
    public Transform lookingTf;
    [System.NonSerialized]    //publicだがインスペクター上には表示しない
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.LookAt(lookingTf);
    }

    private void FixedUpdate()
    {
        Vector3 direction = targetPosition - this.transform.position;
        this.transform.position += 0.05f * direction;

    }

}
