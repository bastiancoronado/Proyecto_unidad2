using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_rotation : MonoBehaviour
{
    [SerializeField] float vel;
    void Start()
    {
        
    }


    void Update()
    {
        transform.Rotate(0,vel*Time.deltaTime,0,0);
    }
}
