using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroy : MonoBehaviour
{
    public string tagName;
    public GameObject scorePrefab;
    int score;
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
            Vector3 direction = other.transform.position - this.transform.position;
            float y = Vector3.Dot(direction, this.transform.up);
            float r = Mathf.Sqrt(direction.magnitude * direction.magnitude - y * y);
            score = (r == 0f) ? 500 : (int)(200f*(2f-r)+150f);
            score = (score / 100) * 100;
            score = Mathf.Clamp(score, 100, 500);

            GameObject clone = Instantiate(scorePrefab) as GameObject;
            clone.GetComponent<PopupScore>().SetScore(score, this.transform.position + Vector3.up);

			Destroy(this.gameObject);
		}
	}
}
