using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission3 : MonoBehaviour
{
    public GameObject messageObj;
    Text message;
    ClearProcess CP;
    public GameObject amezonPrefab;
    GameObject amezonClone;

    Transform playerTf;

    // Start is called before the first frame update
    void Start()
    {
        message = messageObj.GetComponent<Text>();
        message.text = "[M]を押しすと磁力化。\n 宅配物を届けよう！";

        CP = this.GetComponent<ClearProcess>();

        playerTf = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(amezonClone == null){
            amezonClone = Instantiate(amezonPrefab);
            amezonClone.transform.position
                = playerTf.position + 2f*playerTf.forward + 10f*Vector3.up;

        }else if(amezonClone.transform.position.y < -1f){
            Destroy(amezonClone);
        }

        // if(GameObject.FindGameObjectsWithTag("coin").Length == 0){
        //     CP.ClearNotify();
        // }
    }
}
