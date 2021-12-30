using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroy : MonoBehaviour
{
    public string tagName;
    public GameObject scorePrefab;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) //OnTriggerEnterの引数はCollider
	{
		// 重なっている相手に指定タグが付いているとき
		if (other.gameObject.tag == tagName)
		{
            GameObject clone = Instantiate(scorePrefab) as GameObject;
            clone.GetComponent<PopupScore>().SetScore(score, this.transform.position + Vector3.up);

			Destroy(this.gameObject);
		}
	}
}
