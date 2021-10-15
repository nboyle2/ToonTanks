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
    [SerializeField] GameObject craterPrefab;
    GameObject crater;

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
        else if (other.gameObject.tag == "Ground")
        {
            crater = Instantiate(craterPrefab);
            crater.transform.position = new Vector3(transform.position.x, 0.0001f, transform.position.z);
        }

        explosionParticles.transform.parent = null;

        explosionParticles.Play();
        explosionAudio.Play();

        ParticleSystem.MainModule mainModule = explosionParticles.main;
        Destroy(explosionParticles.gameObject, mainModule.duration);

        Destroy(gameObject);
    }
}
