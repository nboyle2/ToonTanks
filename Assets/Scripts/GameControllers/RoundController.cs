using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] KingOfTheHill kingOfTheHill;

    [SerializeField] LastManStanding lastManStanding;

    void Awake()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

        switch (gameController.GetGamemode())
        {
            case "KingOfTheHill":
                kingOfTheHill.enabled = true;
                break;
            case "LastManStanding":
                lastManStanding.enabled = true;
                break;
        }
    }
}
