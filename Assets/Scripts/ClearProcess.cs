using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearProcess : MonoBehaviour
{
    public GameObject fireworkPrefab;
    GameObject subCameraObj;
    public GameObject NextMission;
    Transform playerTf;
    bool clearFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTf = GameObject.Find("Player").transform;
        // subCameraObj = GameObject.Find("SubCamera"); //これだと非アクティブなオブジェクトは取得できない
        subCameraObj = this.transform.Find("SubCamera").gameObject; //これだと非アクティブなオブジェクトは取得できない
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearNotify()
    {
        if(!clearFlag){
            StartCoroutine(CelebrateAndSetNextMission());
            clearFlag = true;
        }

    }

    IEnumerator CelebrateAndSetNextMission()
    {
        Celebration();

        yield return new WaitForSeconds(3f);                    //待機

        if(NextMission != null) NextMission.SetActive(true);

        Destroy(this.gameObject);   //Destroy(this)だと、このスクリプト(class)を削除するだけで、ゲームオブジェクトは消えない

    }

    void Celebration()
    {
        subCameraObj.SetActive(true);

        Vector3[] posisionArray = {
            new Vector3(-10f, -30f, -10f) ,
            new Vector3(10f, -30f, -10f) ,
            new Vector3(-10f, -30f, 10f) ,
            new Vector3(10f, -30, 10f)
        };

        // Vector3 standard = new Vector3(playerTf.position.x, 0f, playerTf.position.z);
        Vector3 standard = playerTf.position;

        for(int i=0; i<posisionArray.Length; i++)
        {
            GameObject clone = Instantiate(fireworkPrefab);
            clone.transform.position = standard + posisionArray[i];

        }

    }
}
