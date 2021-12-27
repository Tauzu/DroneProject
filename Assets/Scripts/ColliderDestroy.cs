using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroy : MonoBehaviour
{
    public string tagName;
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
			Destroy(this.gameObject);
		}
	}
}
