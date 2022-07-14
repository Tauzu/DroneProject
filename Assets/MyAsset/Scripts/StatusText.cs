using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class StatusText : MonoBehaviour
{
    GameObject gameOB;
    private float height;
    private float speed;
    private Rigidbody rbody;
    private Text statusText;
    // DroneMove drone;

    // Start is called before the first frame update
    void Start()
    {
        gameOB = GameObject.Find("PlayerDrone");
        rbody = gameOB.GetComponent<Rigidbody>();
        statusText = this.GetComponent<Text>();
        // drone = gameOB.GetComponent<DroneMove>();
    }

    // Update is called once per frame
    void Update()
    {
        height = gameOB.transform.position.y;
        statusText.text = height.ToString("0.0") + "  m  \n";
        // statusText.text = "高度" + string.Format("{0,6D1}", height) + " m\n";

        speed = 3.6f * rbody.velocity.magnitude;
        statusText.text += speed.ToString("0.0") + " km/h\n";
        // statusText.text += "時速" + string.Format("{0,6D1}", speed) + " km/h\n";

        // for(int i=0; i<drone.BladeNum; i++)
        // {
        //     statusText.text += drone.Blade[i].power.ToString("0.00") + "\n";
        // }
        
    }
}
