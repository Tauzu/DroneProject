using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Slider—p

public class ChargeSliderManager : MonoBehaviour
{
    Shooting shootingScript;
    public GameObject chargeSlider;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        shootingScript = GameObject.Find("Player").GetComponent<Shooting>();
        slider = chargeSlider.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        chargeSlider.SetActive(shootingScript.isCharging);
        slider.value = shootingScript.gradLocation;
    }
}
