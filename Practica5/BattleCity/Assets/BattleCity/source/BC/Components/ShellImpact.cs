using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellImpact : MonoBehaviour
{
    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public string m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public string m_ExplosionAudioInvul;                // Reference to the audio that will play on explosion.
    public int damage;
    public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
    public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
    public float m_explosionForce = 2f;
    public GraphicUpdate graphicUpdate;

    private TankFire m_gameObjectFire;
    
    private void Start()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 2)
            return;
        if (other.gameObject.layer == BCGlobals.PHYSICS_ITEM)
            return;
        if (!m_gameObjectFire)
            return;
        if (other.gameObject == m_gameObjectFire.gameObject)
            return;
        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
        /*Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        // Go through all the colliders...
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == m_gameObjectFire.gameObject)
                continue;
            // Find the TankHealth script associated with the rigidbody.
            Health targetHealth = colliders[i].GetComponent<Health>();

            // If there is no TankHealth script attached to the gameobject, go on to the next collider.
            if (targetHealth)
            {
                // Calculate the amount of damage the target should take based on it's distance from the shell.
                int damage = CalculateDamage();

                // Deal this damage to the tank.
                targetHealth.TakeDamage(damage);
            }

        }*/

        // Unparent the particles from the shell.
        m_ExplosionParticles.transform.parent = null;

        // Play the particle system.
        m_ExplosionParticles.Play();



        // Once the particles have finished, destroy the gameobject they are on.
        ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
        Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

        bool invulnerable = false;

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth)
        {
            int damage = CalculateDamage();
            targetHealth.TakeDamage(damage, m_gameObjectFire.gameObject);
        }
        else
        {
            CommandCenter command = other.GetComponent<CommandCenter>();
            if (command)
            {
                command.InitDestroy(m_gameObjectFire.gameObject);
            }
            else if(other.gameObject.layer == Globals.GROUND_LAYER)
            {
                Breakable brekable = other.GetComponent<Breakable>();
                if (brekable != null)
                    brekable.Break(m_explosionForce, this.transform, m_ExplosionRadius);
                else
                    invulnerable = true;
                /*Collider[] grounds = Physics.OverlapSphere(transform.position, m_ExplosionRadius, 1 << Globals.GROUND_LAYER);
                for (int i = 0; i < grounds.Length; i++)
                {
                    // Find the TankHealth script associated with the rigidbody.
                    Breakable brekable = grounds[i].GetComponent<Breakable>();
                    if (brekable != null)
                        brekable.Break(m_explosionForce, this.transform, m_ExplosionRadius);
                    else
                        invulnerable = true;

                }*/
            }
        }

        // Play the explosion sound effect.
        if (!invulnerable)
            GameMgr.Instance.GetServer<SoundMgr>().PlaySound(m_ExplosionAudio);
        else
            GameMgr.Instance.GetServer<SoundMgr>().PlaySound(m_ExplosionAudioInvul);
        // Destroy the shell.
        Destroy(gameObject);
    }

    public void Configure(TankFire go, Vector3 lookDir)
    {
        m_gameObjectFire = go;
        graphicUpdate.Direction = lookDir;
    }

    public TankFire GetTankFire
    {
        get
        {
            return m_gameObjectFire;
        }
    }


    private int CalculateDamage()
    {
        return damage;
    }

    private void OnDestroy()
    {
        if (m_gameObjectFire != null)
            m_gameObjectFire.Release();
    }
}
