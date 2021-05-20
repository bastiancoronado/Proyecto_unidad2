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
    [SerializeField] GameObject rain;
    [SerializeField] float factor = 0;
    float variant = 0;

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
            if (hmy > 65)
            {
                rain.SetActive(true);
                rain.GetComponent<ParticleSystem>().Play();
            }
            else if (rain.GetComponent<ParticleSystem>().isPlaying) variant = 0.2f;
            else variant = 0;


        }

        if (inf[0] != "-")
        {
            string[] mode = inf[0].Split(':');
            int h = int.Parse(mode[0]);

            float[] operation = {0, 0.1f, 0.15f, 0.25f, 0.3f, 0.5f, 0.55f, 0.75f, 0.8f, 0.85f, 0.95f, 1};

            string[] m = inf[0].Split(' ', ',');
            factor = operation[h == 12 ? 0 : h];
            if (m[1].Substring(0, 2) == "Pm")
            {
                lt.color = Color.Lerp(Sky_light[0], Sky_light[1], factor);
                cm.backgroundColor = Color.Lerp(Sky[0], Sky[1], factor);
                lt.intensity = Mathf.Lerp(1.5f - variant, 0, factor);
            }
            else if (m[1].Substring(0, 2) == "Am")
            {
                lt.color = Color.Lerp(Sky_light[1], Sky_light[0], factor);
                cm.backgroundColor = Color.Lerp(Sky[1], Sky[0], factor);
                lt.intensity = Mathf.Lerp(0, 1.5f - variant, factor);
            }
        }



    }
}
