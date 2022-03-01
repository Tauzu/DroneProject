using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Slider用

//弾丸のチャージ具合をスライダーで表現する。

public class ChargeSliderManager : MonoBehaviour
{
    Shooting shootingScript;

    public GameObject chargeSlider;
    Image backgroundImg;
    Color defaultColor;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        shootingScript = GameObject.Find("Player").GetComponent<Shooting>();
        slider = chargeSlider.GetComponent<Slider>();

        backgroundImg = chargeSlider.transform.Find("Background").gameObject.GetComponent<Image>();
        defaultColor = backgroundImg.color;
    }

    // Update is called once per frame
    void Update()
    {
        chargeSlider.SetActive(shootingScript.isCharging);
        slider.value = shootingScript.gradLocation;

        backgroundImg.color = (shootingScript.isSpecial) ? Color.magenta : defaultColor;
    }
}
