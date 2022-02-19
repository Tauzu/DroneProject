using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDroneController : MonoBehaviour
{
    List<DroneMove> dmList = new List<DroneMove>();
    DroneMove mainDM;
    GameObject dronePrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainDM = this.GetComponent<DroneMove>();
        dronePrefab = (GameObject)Resources.Load("Drone");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject clone = Instantiate(dronePrefab);
            clone.transform.position = this.transform.position + 4f * new Vector3(
                Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)
                );
            dmList.Add(clone.GetComponent<DroneMove>());
        }

        foreach (DroneMove drone in dmList)
        {
            Vector2 targetVector = new Vector2(
                mainDM.transform.position.x - drone.transform.position.x,
                mainDM.transform.position.z - drone.transform.position.z
                );
            if (targetVector.magnitude < 4f)
            {
                targetVector = Vector2.zero;
            }
            else
            {
                targetVector = targetVector.normalized;
            }
            
            drone.targetVector = targetVector;
            drone.targetHeight = mainDM.transform.position.y;
            drone.isHovering = mainDM.isHovering;
            drone.isBoosting = mainDM.isBoosting;
        }
        
    }
}
