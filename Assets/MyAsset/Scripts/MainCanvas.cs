using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject howToUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainUI.SetActive(!Input.GetKey(KeyCode.Escape));
        howToUI.SetActive(Input.GetKey(KeyCode.Escape));
    }
}
