using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

//ミッションクリア時の演出を行う。

public class ClearProcess : MonoBehaviour
{
    public GameObject fireworkPrefab;
    GameObject mainCameraObj;
    GameObject subCameraObj;
    public GameObject NextMission;
    Transform playerTf;
    bool clearFlag = false;
    Text clearText;

    // Start is called before the first frame update
    void Start()
    {
        playerTf = GameObject.Find("Player").transform;
        // subCameraObj = GameObject.Find("SubCamera"); //これだと非アクティブなオブジェクトは取得できない
        subCameraObj = this.transform.Find("SubCamera").gameObject;
        mainCameraObj = GameObject.Find("Main Camera");

        clearText = this.transform.Find("SubCamCanvas/ClearText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearNotify()
    {
        if(!clearFlag){
            if (NextMission != null) { StartCoroutine(CelebrateAndSetNextMission()); }
            else
            {

                StartCoroutine(FinalCelebration());
            }

            clearFlag = true;
        }

    }

    IEnumerator CelebrateAndSetNextMission()
    {
        Celebration();

        yield return new WaitForSeconds(3f);                    //待機

        CameraShift();
        clearText.text = "";

        //subCameraObj.GetComponent<SubCameraMotion>().Deactivate();
        NextMission.SetActive(true);    //次のミッション起動前にはカメラはメインに戻っていなくてはならない

        Destroy(this.gameObject);   //Destroy(this)だと、このスクリプト(class)を削除するだけで、ゲームオブジェクトは消えない

    }

    IEnumerator FinalCelebration()
    {
        Celebration();
        //subCameraObj.GetComponent<SubCameraMotion>().lookingTf = GameObject.Find("Center").transform;

        yield return new WaitForSeconds(3f);                    //待機

        GameObject[] buildingArray = GameObject.FindGameObjectsWithTag("Building");
        GameObject scoreObj = (GameObject)Resources.Load("Score3DText");

        foreach (GameObject building in buildingArray)
        {
            if(building != null)
            {
                GameObject clone = Instantiate(scoreObj) as GameObject;
                clone.GetComponent<PopupScore>().SetScore(2000, building.transform.position + 15f * Vector3.up);
                yield return new WaitForSeconds(0.2f);                    //待機
            }

        }

        CameraShift();
        clearText.text = "";

        Text message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "オールクリアおめでとう！\nあとはご自由にどうぞ。\nあなたが守った町です。";
        Destroy(GameObject.Find("MessageWindow"), 10f);

        Destroy(this.gameObject);   //Destroy(this)だと、このスクリプト(class)を削除するだけで、ゲームオブジェクトは消えない

    }

    void Celebration()
    {
        CameraShift();
        StartCoroutine(ClearMessage());

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

            Destroy(clone, 3f);
        }

    }

    IEnumerator ClearMessage()
    {
        string message = "Mission CLEAR!";

        for (int i = 0; i < message.Length; i++)
        {
            clearText.text = "<i>" + message.Substring(0, i) + "</i>";
            yield return new WaitForSeconds(0.05f);                    //待機
        }

    }

    void CameraShift()
    {
        mainCameraObj.SetActive(!mainCameraObj.activeSelf);
        subCameraObj.SetActive(!subCameraObj.activeSelf);

        if (subCameraObj.activeSelf)
        {
            subCameraObj.transform.position = mainCameraObj.transform.position;
            subCameraObj.transform.rotation = mainCameraObj.transform.rotation;
        }
    }
}
