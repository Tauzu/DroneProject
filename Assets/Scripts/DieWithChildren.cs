using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�q�I�u�W�F�N�g���Ȃ��Ȃ����玩����������

public class DieWithChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount == 0) Destroy(this.gameObject);
    }

}
