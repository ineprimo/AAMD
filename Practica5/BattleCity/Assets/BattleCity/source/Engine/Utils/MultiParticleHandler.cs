using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiParticleHandler : MonoBehaviour
{
    public ParticleSystem[] particles;
    public bool activateAtFirst=false;
    public bool needInvokePlay = false;

    private void Start()
    {
        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].gameObject.SetActive(activateAtFirst);
        }
    }

    public void Init(float time = -1)
    {
        Activate();

        if (time >= 0)
        {
            StartCoroutine(Desactive(time));
        }
    }

    public void DefferredInit(float t,float time = -1)
    {
        if (time > 0 && t > time)
            Debug.LogError("Error el tiempo de esta particula para ser lanzada esta configurado mas tarde que cuando es destruida.");
 

        if (time >= 0)
        {
            StartCoroutine(StartDefferred(time));
        }

        if (time >= 0)
        {
            StartCoroutine(Desactive(time));
        }
    }

    public IEnumerator StartDefferred(float time)
    {
        yield return new WaitForSeconds(time);
        Activate();
    }

    public void Activate()
    {
        for (int i = 0; i < particles.Length; ++i)
        {
            if (particles[i].gameObject.activeInHierarchy)
                Debug.LogWarning("No tendra efecto la activación ya que el gameobject ya estaba activado");
            particles[i].gameObject.SetActive(true);
        }

        if (needInvokePlay)
            Play();
    }
    public IEnumerator Desactive(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].Play();
        }
    }
}
