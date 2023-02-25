using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float steering;
    public float drag;

    public bool dead = false;

    public int cur_points = 0;
    public double cur_time = 10;

    private Rigidbody2D rb;
    private float currentSpeed;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get input
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate speed from input and acceleration (transform.up is forward)
        Vector2 speed = transform.up * (v * acceleration) * drag;
        if(!dead) rb.AddForce(speed);

        // Create car rotation
        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / maxSpeed);
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / maxSpeed);
        }

        // Change velocity based on rotation
        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * 2.0f;
        Vector2 relativeForce = Vector2.right * driftForce;
        Debug.DrawLine(rb.position, rb.GetRelativePoint(relativeForce), Color.green);
        if (!dead) rb.AddForce(rb.GetRelativeVector(relativeForce));

        // Force max speed limit
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        currentSpeed = rb.velocity.magnitude;
    }

    private void Update()
    {
        if (!dead)
        {
            // cur_time -= Time.deltaTime;
            if (cur_time <= 0)
            {
                LoseGame();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Walker")
        {
            Walker w = collision.gameObject.GetComponent<Walker>();
            cur_points += w.GetPoints();
            cur_time += w.GetTime();
            w.Kill();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Loser")
        {
            LoseGame();
        }
    }

    private void LoseGame()
    {
        // Cenas para a leticia fazer 
        dead = true;
    }
}
