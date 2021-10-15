using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRotation : MonoBehaviour
{
    Quaternion rotation;

    void Start()
    {
        rotation = transform.parent.localRotation;
    }

    void Update()
    {
        transform.rotation = rotation;
    }
}
