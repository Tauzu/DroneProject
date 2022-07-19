using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Slider用

public class DroneEnergy : MonoBehaviour
{
    float energy;
    public float maxEnergy = 5000f;

    Drone drone;

    public GameObject energySlider;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        drone = this.GetComponent<Drone>();
        energy = maxEnergy;

        slider = energySlider.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(energy);
        slider.value = energy / maxEnergy;

        drone.enabled = (energy > 0f);
    }

    private void FixedUpdate()
    {
        energy -= drone.GetTotalPower();
    }

    public void addFuel(float addEnergy)
    {
        energy = Mathf.Min(energy + addEnergy, maxEnergy);
    }

}
