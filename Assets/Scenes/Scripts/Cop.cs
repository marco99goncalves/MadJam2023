using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Cop : MonoBehaviour
{
    public List<Transform> targets;
    public float speed;
    public double detection_radius;
    public Car player;

    LineRenderer line;

    int segments = 50;

    int cur_target;
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Car>();
        transform.position = targets[0].position; // Set the starting position to be the same as the first target
        cur_target = 0;

        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
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
            player.LoseGame(2);
        }
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * (float)detection_radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * (float)detection_radius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}
