using UnityEngine;
using System.Collections;

// Quits the player when the user hits escape

public class Quit : MonoBehaviour
{
     void QuitGame () {
        Application.Quit ();
        Debug.Log("Game is exiting");
        //Just to make sure its working
 }
}