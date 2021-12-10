using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillController : MonoBehaviour
{
    [SerializeField] KingOfTheHill roundController;

    [SerializeField] GameObject[] hillLocations;
    int hillLocationsIndex;

    [SerializeField] float hillDuration;
    float moveHillTime;

    int fullHillRotations;

    List<Collider> tankColliders;

    void Start()
    {
        moveHillTime = Time.time + hillDuration;

        tankColliders = new List<Collider>();
    }

    void Update()
    {
        if (moveHillTime <= Time.time)
        {
            MoveHill();
        }

        if (tankColliders.Count == 1)
        {
            GetComponent<Renderer>().material.color = tankColliders[0].gameObject.GetComponent<TankController>().GetTankColor();

            roundController.AddHillTime(tankColliders[0].gameObject.GetComponent<TankController>(), Time.deltaTime);
        }
        else if (tankColliders.Count == 2 &&
                    tankColliders[0].gameObject.GetComponent<TankController>().GetTankColorStr().Equals(tankColliders[1].gameObject.GetComponent<TankController>().GetTankColorStr()))
        {
            GetComponent<Renderer>().material.color = tankColliders[0].gameObject.GetComponent<TankController>().GetTankColor();

            roundController.AddHillTime(tankColliders[0].gameObject.GetComponent<TankController>(), Time.deltaTime);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.grey;
        }
    }

    void MoveHill()
    {
        if (hillLocationsIndex >= hillLocations.Length)
        {
            hillLocationsIndex = 0;

            fullHillRotations++;
        }

        transform.position = hillLocations[hillLocationsIndex++].transform.position;

        moveHillTime = Time.time + hillDuration;
    }

    public int GetFullHillRotations()
    {
        return fullHillRotations;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tank")
        {
            tankColliders.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        tankColliders.Remove(other);
    }
}
