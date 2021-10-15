using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] float cooldownLength;
    float canShootTime;
    bool canShoot;

    [Header("Shell")]
    [SerializeField] float shellSpeed;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] Transform shellSpawn;

    [Header("Audio")]
    [SerializeField] AudioSource shootAudio;

    void Update()
    {
        if (canShootTime <= Time.time)
        {
            canShoot = true;
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;

            canShootTime = Time.time + cooldownLength;

            GameObject shell = Instantiate(shellPrefab, shellSpawn.position, shellSpawn.rotation);

            Rigidbody rb = shell.GetComponent<Rigidbody>();
            rb.velocity = shellSpeed * shellSpawn.forward;

            shootAudio.Play();
        }
    }
}
