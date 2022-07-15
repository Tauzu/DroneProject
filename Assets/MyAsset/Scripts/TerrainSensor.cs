using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSensor : MonoBehaviour
{
    public float delta;

    DronePhysics drone;
    Transform tf;
    Rigidbody rb;

    Transform samplingTf1;
    Transform samplingTf2;


    // Start is called before the first frame update
    void Start()
    {
        drone = this.GetComponent<Drone>();
        tf = this.transform;
        rb = GetComponent<Rigidbody>();

        GameObject samplingObj = (GameObject)Resources.Load("Sampling");
        GameObject samplingClone1 = Instantiate(samplingObj);
        samplingTf1 = samplingClone1.transform;
        GameObject samplingClone2 = Instantiate(samplingObj);
        samplingTf2 = samplingClone2.transform;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = tf.position;
        float height1 = GetTerrainHeight(position.x, position.z);
        samplingTf1.position = new Vector3(position.x, height1, position.z);

        Vector3 position_pred = position + rb.velocity * delta;
        float height2 = GetTerrainHeight(position_pred.x, position_pred.z);
        samplingTf2.position = new Vector3(position_pred.x, height2, position_pred.z);

        drone.SetStandardHeight((height1+height2)*0.5f);

        //Debug.Log(height);
    }

    float GetTerrainHeight(float x, float z)
    {
        float height = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(
            x / Terrain.activeTerrain.terrainData.size.x,
            z / Terrain.activeTerrain.terrainData.size.z
        );

        return height;
    }
}
