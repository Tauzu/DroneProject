using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class WarningUI : MonoBehaviour
{
    Text warningText;
    public ScoreManager scoreManager;
    int[] numBuilding = new int[2];
    float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        warningText = GetComponent<Text>();
        numBuilding[1] = scoreManager.numBuilding;
        numBuilding[0] = numBuilding[1];
    }

    // Update is called once per frame
    void Update()
    {
        numBuilding[1] = scoreManager.numBuilding;
        if (numBuilding[1] < numBuilding[0])
        {
            warningText.text = "建物が破壊された！\n残り：" + numBuilding[1].ToString();
            timeCounter = 5f;
        }

        if (timeCounter <= 0f) warningText.text = "";
        timeCounter -= Time.deltaTime;

        numBuilding[0] = numBuilding[1];

    }
}
