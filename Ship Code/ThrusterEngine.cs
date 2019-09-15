using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterEngine : MonoBehaviour
{
    public ParticleSystem thruster1, thruster2, turbo1,turbo2,leftRamThruster,rightRamThruster;
    public AudioSource engineNoise,engineClank,thrusterNoise,turboSound;
    private const float LowPitch = 0.2f;
    private const float HighPitch = 2f;
    private bool turboPlaying = false;
    private int ramCounter = 0;

    private void FixedUpdate() {
        if(ramCounter > 0){
            ramCounter--;
        }   
        if(ramCounter == 1){
            var em = leftRamThruster.emission;
            //turboSound.Play(0);
            em.enabled = false;
            em = rightRamThruster.emission;
            em.enabled = false;
        } 
    }

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

    public void triggerRamThruster(string side){
        if(side == "left"){
            var em = leftRamThruster.emission;
            em.enabled = true;
            ramCounter = 100;
        }else if(side == "right"){
            var em = rightRamThruster.emission;
            em.enabled = true;
            ramCounter = 100;
        }else{
            Debug.Log("invalid side for ram");
        }
    }

    public void adjustNoise(float gearmod,float engineRevs){
        engineNoise.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
        thrusterNoise.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
        engineClank.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);
    }
}
