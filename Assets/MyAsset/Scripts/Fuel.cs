using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public float energy = 3000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        DroneEnergy dEnergy = other.GetComponent<DroneEnergy>();
        if (dEnergy != null)
        {
            dEnergy.addFuel(energy);
            Destroy(this.gameObject);
        }
    }
}
