using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class Mission5 : MonoBehaviour
{
    Text message;
    ClearProcess CP;
    public GameObject specialAmezonPrefab;
    GameObject specialAmezonClone;

    GameObject dragonObj;
    ScoreManager ScoreMan;
    Transform playerTf;

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "[E]を押すと磁力化。\n 宅配物を届けよう！\n目標SCORE:3000";

        CP = this.GetComponent<ClearProcess>();

        playerTf = GameObject.Find("Player").transform;

        ScoreMan = GameObject.Find("GameController").GetComponent<ScoreManager>();

        dragonObj = this.transform.Find("Dragon").gameObject;
        dragonObj.SetActive(true);

        StartCoroutine(MainCoroutine());

    }

    // Update is called once per frame
    void Update()
    {

        if (dragonObj == null)
        {
            CP.ClearNotify();
        }

        // if(GameObject.FindGameObjectsWithTag("coin").Length == 0){
        //     CP.ClearNotify();
        // }
    }

    IEnumerator MainCoroutine()
    {
        while (true) {

            if(specialAmezonClone == null)
            {
                specialAmezonClone = Instantiate(specialAmezonPrefab);
                specialAmezonClone.transform.position
                    = playerTf.position + 2f*playerTf.forward + 5f*Vector3.up;
            }
            else if (specialAmezonClone.transform.position.y < -1f)
            {
                Destroy(specialAmezonClone);
            }
            //待機
            yield return new WaitForSeconds(10f);

        }
        
    }
}
