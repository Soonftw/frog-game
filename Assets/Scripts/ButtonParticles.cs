using UnityEngine;

public class ButtonParticles : MonoBehaviour
{
    public ParticleSystem particles;

    public void PlayParticles()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particles.Play();  // startar från början
    }
}
