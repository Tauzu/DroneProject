using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private string sceneName;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            sceneName = SceneManager.GetActiveScene().name;

            if(sceneName == "stage01")
            {
                SceneManager.LoadScene("stage02");
            }
            else if(sceneName == "stage02")
            {
                SceneManager.LoadScene("stage03");
            }
            else if(sceneName == "stage03")
            {
                SceneManager.LoadScene("town");
            }
            

        }
        
    }
}
