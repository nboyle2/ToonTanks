using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankInput : MonoBehaviour
{
    [SerializeField] TankMovement tankMovement;
    [SerializeField] TankShoot tankShoot;

    void OnMoveTank(InputValue value)
    {
        tankMovement.TankInput(value.Get<Vector2>());
    }

    void OnMoveTurret(InputValue value)
    {
        tankMovement.TurretInput(value.Get<Vector2>());
    }

    void OnShoot()
    {
        tankShoot.Shoot();
    }
}
