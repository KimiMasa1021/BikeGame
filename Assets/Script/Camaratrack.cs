using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camaratrack : MonoBehaviour
{
    
    private GameObject player;
    private Vector3 offsets;
    void Start()
    {
        this.player = GameObject.Find("BikeModel");

        offsets = transform.position - player.transform.position;
        
    }

    void Update()
    {
        transform.position = player.transform.position + offsets;
    }
}
