using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cop : MonoBehaviour
{
    public List<Transform> targets;
    public float speed;
    public double detection_radius;
    public Car player;

    int cur_target;
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Car>();
        transform.position = targets[0].position; // Set the starting position to be the same as the first target
        cur_target = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = -(targets[cur_target].position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, targets[cur_target].position, speed * Time.deltaTime);

        Vector3 diff_pos = transform.position - targets[cur_target].transform.position;
        if (Mathf.Abs(diff_pos.x) <= 0.05 && Mathf.Abs(diff_pos.y) <= 0.05)
        {
            //Reached the new target ish
            transform.position = targets[cur_target].position;
            cur_target++;
            if (cur_target >= targets.Count)
                cur_target = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, (float)detection_radius);
    }

    public void OnPlayerKill()
    {
        double dist = Vector2.Distance(transform.position, player.transform.position);
        if(dist <= detection_radius)
        {
            // Player got caught
            player.LoseGame();
        }
    }
}
