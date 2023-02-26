using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float steering;
    public float drag;
    public List<Cop> cops;



    public bool dead = false;

    public int cur_points = 0;
    public double cur_time = 10;

    private Rigidbody2D rb;
    private float currentSpeed;

    public TextMeshProUGUI score;
    public TextMeshProUGUI time;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        cops = FindObjectsOfType<Cop>().ToList<Cop>();

        score = GameObject.FindGameObjectsWithTag("Score_Score")[0].GetComponent<TextMeshProUGUI>();
        time = GameObject.FindGameObjectsWithTag("Time_Score")[0].GetComponent<TextMeshProUGUI>();
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
            cur_time -= Time.deltaTime;
            time.text = ((int)cur_time).ToString();
            score.text = ((int)cur_points).ToString();

            if (cur_time <= 0)
            {
                LoseGame(1);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                dead = false;
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
            foreach(Cop cop in cops)
            {
                cop.OnPlayerKill();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Loser")
        {
            LoseGame(0);
        }

        if (collision.gameObject.tag == "Water")
        { 
            LoseGame(3);
        }
    }


    /*
     * 0 - Crashed into building
     * 1 - Ran out of time
     * 2 - Cops
     * 3 - Water
     * 4 - Animals (Not implemented)
     * */
    public void LoseGame(int reason)
    {
        // Cenas para a leticia fazer 
        dead = true;
        switch (reason)
        {
            case 0:
            {
                SceneManager.LoadScene("deathbuildings");
                break;
            }
            case 1:
            {
                SceneManager.LoadScene("deathbytime");
                break;
            }
            case 2:
            {
                SceneManager.LoadScene("deathpolice");
                break;
            }
            case 3:
            {
                SceneManager.LoadScene("deathbywater");
                break;
            }
            case 4:
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("deathbyanimals"));
                break;
            }
        }
    }
}
