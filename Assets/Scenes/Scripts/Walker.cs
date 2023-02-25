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
    public double respawn_timeout;
    public bool dead = false;
    public int points_bonus;
    public double time_bonus;

    public double respawn_timer;
    int cur_target;
    // Start is called before the first frame update
    void Start()
    {
        respawn_timer = respawn_timeout;
        transform.position = targets[0].position; // Set the starting position to be the same as the first target
        cur_target = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            respawn_timer -= Time.deltaTime;
            if(respawn_timer <= 0)
            {
                Respawn();
            }
        }else
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

        
    }

    void Respawn()
    {
        dead = false;
        transform.position = targets[0].position; // Set the starting position to be the same as the first target
        cur_target = 0;
        respawn_timer = respawn_timeout;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Kill()
    {
        dead = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
    public int GetPoints()
    {
        return points_bonus;
    }

    public double GetTime()
    {
        return time_bonus;
    }
}
