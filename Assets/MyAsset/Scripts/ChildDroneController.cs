using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子ドローンを生成する。ただのおまけ要素です。
public class ChildDroneController : MonoBehaviour
{
    Drone mainDrone;
    Shooting mainShooting;
    GameObject dronePrefab;

    struct ChildDrone
    {
        public Drone drone;
        public Shooting shooting;
        Vector3 initialPosition;

        public ChildDrone(Drone drone, Shooting shooting, Vector3 initialPosition, Vector3 parentPosition)
        {
            this.drone = drone;
            this.shooting = shooting;
            this.shooting.freeDirection = false;
            this.initialPosition = initialPosition;
            this.drone.transform.position = initialPosition + parentPosition;
        }

        public Vector3 GetTargetVector3D(Vector3 parentPosition)
        {
            return this.initialPosition + parentPosition - this.drone.transform.position;
        }
    };

    List<ChildDrone> cdList = new List<ChildDrone>();

    Vector3[] initialArray = new Vector3[] {
        new Vector3(-1f, 0f, -1f),
        new Vector3(1f, 0f, -1f),
        new Vector3(-1f, 0f, 1f),
        new Vector3(1f, 0f, 1f)
    };

    // Start is called before the first frame update
    void Start()
    {
        mainDrone = this.GetComponent<Drone>();
        mainShooting = this.GetComponent<Shooting>();
        dronePrefab = (GameObject)Resources.Load("Drone");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject clone = Instantiate(dronePrefab);

            int numChild = cdList.Count;

            Vector3 initialPosition = 2 * (numChild / 4 + 1) * initialArray[numChild % 4];

            //dmList.Add(clone.GetComponent<Drone>());
            cdList.Add(
                new ChildDrone(
                    clone.GetComponent<Drone>(),
                    clone.GetComponent<Shooting>(), 
                    initialPosition, mainDrone.transform.position
                    )
                );

        }

        //foreach (Drone drone in dmList)
        foreach (ChildDrone child in cdList)
        {
            Vector3 targetVector3D = child.GetTargetVector3D(mainDrone.transform.position);
            Vector2 targetVector = new Vector2(targetVector3D.x, targetVector3D.z);
            float distance = targetVector.magnitude;

            targetVector = targetVector.normalized;
            if (distance < 5f)
            {
                targetVector = targetVector*(distance/5f);
            }
            
            child.drone.SetTargetVector(targetVector);
            child.drone.SpecifyStatus(mainDrone.transform.position.y, mainDrone.IsHovering(), mainDrone.IsBoosting());

            child.drone.barrierObj.SetActive(mainDrone.barrierObj.activeSelf);
            child.drone.magnetObj.SetActive(mainDrone.magnetObj.activeSelf);

            child.shooting.direction = mainShooting.direction;
            child.shooting.isSpecial = mainShooting.isSpecial;
        }
        
    }
}
