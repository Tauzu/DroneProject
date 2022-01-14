using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject missionObj;
    public Light directionalLight;

    // Start is called before the first frame update
    void Start()
    {
        directionalLight.color = Color.red;
        Destroy(missionObj);
        Camera.main.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
