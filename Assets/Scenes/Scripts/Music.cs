using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("Music Player").Length <= 0)
            DontDestroyOnLoad(transform.gameObject);
    }
}
