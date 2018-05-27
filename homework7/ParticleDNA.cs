using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDNA : MonoBehaviour {
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;
    public int resolution;
    private float space = 0.15f;
    public float gap = 2;
    public float width = 1;
    public float rotateSpeed = 0.03f;
    public Gradient colorGradient1;
    public Gradient colorGradient2;
    private float speed = 0;
    private float colorChange = 0;
    private bool up = true;
    // Use this for initialization
    void Start () {
        particlesArray = new ParticleSystem.Particle[2 * resolution];
        var main = particleSystem.main;
        main.maxParticles = 2 * resolution;
        particleSystem.Emit(2 * resolution);
        particleSystem.GetParticles(particlesArray);       
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < resolution; i++)
        {
            particlesArray[i].position = new Vector3((i - resolution / 2) * space,
                width * Mathf.Sin(i * space * gap + speed), width * Mathf.Cos(i * space * gap));
            particlesArray[i].color = colorGradient1.Evaluate(colorChange);
        }
        for (int i = 0; i < resolution; i++)
        {
            particlesArray[i + resolution].position =new Vector3((i - resolution / 2) * space,
                width * Mathf.Cos(i * space * gap + speed), width * Mathf.Sin(i * space * gap));
            particlesArray[i + resolution].color = colorGradient2.Evaluate(colorChange);
        }
        speed += rotateSpeed;
        if (up)
        {
            colorChange += 0.01f;
            if (colorChange > 1) up = false;
        }
        else
        {
            colorChange -= 0.01f;
            if (colorChange < 0) up = true;
        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }
    
}
