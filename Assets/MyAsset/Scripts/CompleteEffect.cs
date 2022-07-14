using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--これがないとTextクラスを扱えない

//ミッションクリア時の演出を行う。

public class CompleteEffect : MonoBehaviour
{
    public GameObject fireworkPrefab;
    GameObject mainCameraObj;
    GameObject subCameraObj;

    Transform playerTf;

    [System.NonSerialized]    //publicだがインスペクター上には表示しない
    public bool start = false;
    [System.NonSerialized]    //publicだがインスペクター上には表示しない
    public bool finish = false;

    Text clearText;

    // Start is called before the first frame update
    void Start()
    {
        playerTf = GameObject.Find("PlayerDrone").transform;
        // subCameraObj = GameObject.Find("SubCamera"); //これだと非アクティブなオブジェクトは取得できない
        subCameraObj = this.transform.Find("SubCamera").gameObject;
        mainCameraObj = Camera.main.gameObject;

        clearText = this.transform.Find("SubCamCanvas/ClearText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEffect()
    {
        StartCoroutine(CelebrateProcess());

        start = true;

    }

    public void StartFinalEffect()
    {
        StartCoroutine(FinalCelebrateProcess());

        start = true;

    }

    IEnumerator CelebrateProcess()
    {
        SetSubCameraTarget(new Vector3(-40f, 30f, -50f), GameObject.Find("PlayerDrone").transform);

        Celebration();

        yield return new WaitForSeconds(3f);                    //待機

        SubCameraSwitch(false);

        clearText.text = "";

        finish = true;

    }

    IEnumerator FinalCelebrateProcess()
    {
        SetSubCameraTarget(new Vector3(42f, 100f, -100f), GameObject.Find("Center").transform);

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

        SubCameraSwitch(false);
        clearText.text = "";

        Text message = GameObject.Find("MessageText").GetComponent<Text>();
        message.text = "オールクリアおめでとう！\nあとはご自由にどうぞ。\nあなたが守った町です。";
        Destroy(GameObject.Find("MessageWindow"), 10f);

        finish = true;

    }

    void Celebration()
    {
        SubCameraSwitch(true);
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

    void SubCameraSwitch(bool flag)
    {
        mainCameraObj.SetActive(!flag);
        subCameraObj.SetActive(flag);

        if (flag)
        {
            subCameraObj.transform.position = mainCameraObj.transform.position;
            subCameraObj.transform.rotation = mainCameraObj.transform.rotation;
        }
    }

    void SetSubCameraTarget(Vector3 targetPosition, Transform lookingTf)
    {
        SubCameraMotion scMotion = subCameraObj.GetComponent<SubCameraMotion>();
        scMotion.lookingTf = lookingTf;
        scMotion.targetPosition = targetPosition;
    }
}
