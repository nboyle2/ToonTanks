using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float initialHealth;
    [SerializeField] Color maxHealthColor;
    [SerializeField] Color minHealthColor;
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    float currentHealth;
    bool dead;

    [Header("Explosion")]
    [SerializeField] GameObject explosionPrefab;
    AudioSource explosionAudio;

    MeshRenderer[] renderers;

    void Awake()
    {
        renderers = transform.Find("TankRenderers").GetComponentsInChildren<MeshRenderer>();

        currentHealth = initialHealth;

        UpdateHealth();
    }

    public void Respawn()
    {
        dead = false;

        currentHealth = initialHealth;

        UpdateHealth();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }

        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        UpdateHealth();

        if (currentHealth <= 0f && !dead)
        {
            Die();
        }
    }

    void UpdateHealth()
    {
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(minHealthColor, maxHealthColor, currentHealth / initialHealth);
    }

    void Die()
    {
        dead = true;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }

        transform.Find("Canvas").gameObject.SetActive(false);

        ParticleSystem explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();

        explosionParticles.transform.position = transform.position;

        explosionParticles.Play();
        explosionAudio.Play();
    }
}
