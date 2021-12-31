using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScore : MonoBehaviour
{
    public Gradient grad;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score, Vector3 position)
    {
        this.transform.position = position;
        GameObject cameraObj = GameObject.Find("Main Camera");
        Vector3 direction = position - cameraObj.transform.position;
        this.transform.rotation = Quaternion.LookRotation(direction);    //向きベクトルを与えて回転
        this.transform.localScale = Vector3.one * direction.magnitude / 20f;

        TextMesh tMesh = this.GetComponent<TextMesh>();
        tMesh.text = score.ToString();
        tMesh.color = grad.Evaluate(Mathf.Min((Mathf.Log10((float)score/100f))/2f, 1f));

        GameObject controlObj = GameObject.Find("GameController");
        controlObj.GetComponent<ScoreManager>().score += score;

        Destroy(this.gameObject, 3f);   //Destroy(this)だと、このスクリプト(class)を削除するだけで、ゲームオブジェクトは消えない
    }
}
