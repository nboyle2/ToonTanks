using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplode : MonoBehaviour
{
    [Header("Shell")]
    [SerializeField] float damage;
    [SerializeField] float lifeTime;

    [Header("Explosion")]
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioSource explosionAudio;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tank")
        {
            TankHealth tankHealth = other.GetComponent<TankHealth>();
            tankHealth.TakeDamage(damage);
        }

        explosionParticles.transform.parent = null;

        explosionParticles.Play();
        explosionAudio.Play();

        ParticleSystem.MainModule mainModule = explosionParticles.main;
        Destroy(explosionParticles.gameObject, mainModule.duration);

        Destroy(gameObject);
    }
}
