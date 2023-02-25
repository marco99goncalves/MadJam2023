using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFollower : MonoBehaviour
{

    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(car.transform.position.x, car.transform.position.y, -10);
    }
}
