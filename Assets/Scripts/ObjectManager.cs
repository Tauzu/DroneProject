using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public float lowerLimit = -30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];   //as~がないとエラーになった
        foreach(var gameObj in gameObjects)
        {
            if(gameObj.transform.position.y < lowerLimit)
            {
                Destroy(gameObj);
            }
        }
    }
    
}
