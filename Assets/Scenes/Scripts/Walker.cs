using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Walker : MonoBehaviour
{
    public List<Transform> targets;
    public float speed;

    int cur_target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = targets[0].position; // Set the starting position to be the same as the first target
        cur_target = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targets[cur_target].position, speed * Time.deltaTime);

        Vector3 diff_pos = transform.position - targets[cur_target].transform.position;
        if(Mathf.Abs(diff_pos.x) <= 0.05 && Mathf.Abs(diff_pos.y) <= 0.05)
        {
            //Reached the new target ish
            transform.position = targets[cur_target].position;
            cur_target++;
            if (cur_target >= targets.Count)
                cur_target = 0;
        }
    }
}
