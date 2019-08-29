using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterEngine : MonoBehaviour
{
    public ParticleSystem thruster1, thruster2, turbo1,turbo2;
    public AudioSource engineNoise,engineClank,thrusterNoise,turboSound;
    private const float LowPitch = 0.2f;
    private const float HighPitch = 2f;
    private bool turboPlaying = false;
    public void enableThrusters(bool select){
        var em = thruster1.emission;
        em.enabled = select;
        em = thruster2.emission;
        em.enabled = select;
    }   

    public void enableTurbo(bool select){
        var em = turbo1.emission;
        //turboSound.Play(0);
        em.enabled = select;
        em = turbo2.emission;
        em.enabled = select;
        if(select){
            if(!turboPlaying){
                turboSound.Play(0);
                turboPlaying = true;
            }
        }else{
            turboSound.Stop();
            turboPlaying = false;
        }
    }

    public void adjustNoise(float gearmod,float engineRevs){
        engineNoise.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
        thrusterNoise.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
        engineClank.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);
    }
}
