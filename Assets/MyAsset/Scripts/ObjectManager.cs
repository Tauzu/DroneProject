using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    List<Transform> children;
    public float lowerLimit = -30f;

    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトを返却する配列作成
        children = new List<Transform>();

        // 0～個数-1までの子を順番に配列に格納
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            children.Add( this.transform.GetChild(i) );
        }
    }

    // Update is called once per frame
    void Update()
    {
        //foreach(Transform childTf in children)
        //{
        //    if(childTf.position.y < lowerLimit)
        //    {
        //        Destroy(childTf.gameObject);
        //    }
        //}
    }
    
}
