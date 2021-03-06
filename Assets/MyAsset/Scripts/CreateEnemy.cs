using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵を周期的に出現させる（コルーチン使用）

public class CreateEnemy : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float period = 10;
    public float standbyWait = 60;
    public Vector3 offset = new Vector3(0f,20f,0f);

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine("waitCoroutine");
        StartCoroutine(waitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator waitCoroutine()
    {
        //待機
        yield return new WaitForSeconds(standbyWait);

        StartCoroutine(mainCoroutine());
    }
 
    IEnumerator mainCoroutine()
    {
        while (true) {
            
            GameObject enemy = Instantiate(EnemyPrefab);
            enemy.transform.parent = this.transform.parent; //自分と同じ親を持つ
            enemy.transform.position = this.transform.position + offset;

                        //待機
            yield return new WaitForSeconds(period);
            
        }

        
    }
}
