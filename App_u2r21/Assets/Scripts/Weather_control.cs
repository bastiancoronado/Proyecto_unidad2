using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weather_control : MonoBehaviour
{
    [SerializeField] Camera cm;
    [SerializeField] Light lt;
    [SerializeField] Color[] Sky;
    [SerializeField] Color[] Sky_light;
    [SerializeField] Sample spl;
    void Start()
    {
        
    }


    void Update()
    {

        string[] inf = spl.tx_hmd.text.Split(' ', 'f');
        inf[0] = spl.tx_clk.text;

        if (inf[1] != "-")
        {
            float hmy = float.Parse(inf[1]);
        }

        if (inf[0] != "-")
        {
            string[] mode = inf[0].Split(':');
            float h = float.Parse(mode[0]);
            string[] m = inf[0].Split(' ', ',');

            if (m[1].Substring(0, 2) == "Pm")
            {
                lt.color = Color.Lerp(Sky_light[0], Sky_light[1], (h - 1) / 11);
                cm.backgroundColor = Color.Lerp(Sky[0], Sky[1], (h - 1) / 11);
                lt.intensity = Mathf.Lerp(1.5f, 0.1f, (h - 1) / 11);
                //Debug.Log(h + " " + m[1].Substring(0, 2));
            }
            else if (m[1].Substring(0, 2) == "Am")
            {

            }
        }

        //lt.color = Sky;
        //cm.backgroundColor = Sky_light;
        //Color.Lerp
    }
}
