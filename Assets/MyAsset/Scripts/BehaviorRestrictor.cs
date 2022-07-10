using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorRestrictor : MonoBehaviour
{
    [SerializeField]
    Vector3 positionMin = new Vector3(-100f, -10f, -100f);  //行動範囲の下限
    [SerializeField]
    Vector3 positionMax = new Vector3(200f, 900f, 300f);  //行動範囲の上限

    Rigidbody rbody;
    Transform tf;
    Drone drone;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        tf = this.transform;
        drone = this.GetComponent<Drone>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rbody.position.y < -1){
            drone.SwitchHovering(false);
            rbody.AddForce(Vector3.up * 1000f);
        }

        PositionClamp();
    }

    /// <summary>
    /// ドローンを行動範囲内にとどめる処理。
    /// 行動限界に到達した場合、速度をゼロにする。
    /// </summary>
    void PositionClamp()
    {
        Vector3 modifiedPosition;
        modifiedPosition.x = Mathf.Clamp(tf.position.x, positionMin.x, positionMax.x);
        modifiedPosition.y = Mathf.Clamp(tf.position.y, positionMin.y, positionMax.y);
        modifiedPosition.z = Mathf.Clamp(tf.position.z, positionMin.z, positionMax.z);

        if (modifiedPosition != tf.position)
        {
            tf.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }
    }
}
