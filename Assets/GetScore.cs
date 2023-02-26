using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.SetText(FindAnyObjectByType<Music>().Score.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
