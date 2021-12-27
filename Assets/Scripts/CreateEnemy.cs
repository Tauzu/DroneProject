using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float period = 10;
    public Vector3 offset = new Vector3(0f,20f,0f);

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine("waitCoroutine");
        StartCoroutine("waitCoroutine");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator waitCoroutine()
    {
        //待機
        yield return new WaitForSeconds(60f);

        StartCoroutine("mainCoroutine");
    }
 
    private IEnumerator mainCoroutine()
    {
        while (true) {
            //待機
            yield return new WaitForSeconds(period);
            
            GameObject enemy = Instantiate(EnemyPrefab);
            enemy.transform.position = this.transform.position + offset;
    
            
        }

        
    }
}
