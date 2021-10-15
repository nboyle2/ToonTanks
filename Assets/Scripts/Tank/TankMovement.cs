using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [Header("Tank")]
    [SerializeField] float tankSpeed;
    [SerializeField] float tankRotationSpeed;
    float moveMagnitude;
    Quaternion tankDeltaRotation;

    [SerializeField] Rigidbody rb;

    [Header("Turret")]
    [SerializeField] float turretRotationSpeed;
    [SerializeField] GameObject turret;
    Quaternion turretDeltaRotation;

    [Header("Audio")]
    [SerializeField] float pitchRange;
    [SerializeField] AudioSource movementAudio;
    [SerializeField] AudioClip engineIdling;
    [SerializeField] AudioClip engineDriving;
    float originalPitch;

    void Start()
    {
        originalPitch = movementAudio.pitch;
    }

    void Update()
    {
        if (transform.Find("TankRenderers").GetComponentInChildren<MeshRenderer>().enabled)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;

            turret.transform.rotation = rb.rotation;
        }

        PlayEngineAudio();
    }

    void FixedUpdate()
    {
        MoveTank();

        MoveTurret();
    }

    void MoveTank()
    {
        rb.MoveRotation(rb.rotation * tankDeltaRotation.normalized);

        rb.MovePosition(rb.position + transform.forward * moveMagnitude);
    }

    void MoveTurret()
    {
        turret.transform.rotation *= turretDeltaRotation;
    }

    public void TankInput(Vector2 direction)
    {
        tankDeltaRotation = Quaternion.Euler(0f, Mathf.Round(direction.x) * tankRotationSpeed * Time.fixedDeltaTime, 0f);

        moveMagnitude = Mathf.Round(direction.y) * tankSpeed * Time.fixedDeltaTime;
    }

    public void TurretInput(Vector2 direction)
    {
        turretDeltaRotation = Quaternion.Euler(0f, Mathf.Round(direction.x) * turretRotationSpeed * Time.fixedDeltaTime, 0f);
    }

    void PlayEngineAudio()
    {
        if (tankDeltaRotation.y == 0f && moveMagnitude == 0f)
        {
            if (movementAudio.clip == engineDriving)
            {
                movementAudio.clip = engineIdling;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            if (movementAudio.clip == engineIdling)
            {
                movementAudio.clip = engineDriving;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
    }
}
