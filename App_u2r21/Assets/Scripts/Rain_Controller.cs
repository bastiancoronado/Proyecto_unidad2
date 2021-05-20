using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_Controller : MonoBehaviour
{
    [SerializeField] AnimationCurve controlCurve;
    [SerializeField] float totalDuration;
    ParticleSystem rain;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.MainModule main;
    AudioSource audiosource;
    float duration;

    private void Awake()
    {
        rain = GetComponent<ParticleSystem>();
        audiosource = GetComponent<AudioSource>();
        emission = rain.emission;
        main = rain.main;
        duration = 0;
        main.duration = totalDuration - 2f;
        
    }


    private void Update()
    {
        float evaluateCurve = controlCurve.Evaluate(duration / totalDuration);
        emission.rateOverTime = (evaluateCurve * 1000) + 100;
        main.startSpeed = 4 * evaluateCurve + 1;
        //audiosource.volume = evaluateCurve;
        duration += Time.deltaTime;
    }
}
