using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDroneController : MonoBehaviour
{
    List<Drone> dmList = new List<Drone>();
    Drone mainDrone;
    GameObject dronePrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainDrone = this.GetComponent<Drone>();
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
            dmList.Add(clone.GetComponent<Drone>());
        }

        foreach (Drone drone in dmList)
        {
            Vector2 targetVector = new Vector2(
                mainDrone.transform.position.x - drone.transform.position.x,
                mainDrone.transform.position.z - drone.transform.position.z
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
            drone.targetHeight = mainDrone.transform.position.y;
            drone.isHovering = mainDrone.isHovering;
            drone.isBoosting = mainDrone.isBoosting;
        }
        
    }
}
