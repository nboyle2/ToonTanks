using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject main;

    [SerializeField] GameObject gamemodes;

    [SerializeField] GameObject tankSetup;

    [SerializeField] GameController gameController;

    public void PlayButton()
    {
        main.SetActive(false);

        gamemodes.SetActive(true);
    }

    public void KingOfTheHillButton()
    {
        gameController.SetGamemode("KingOfTheHill");

        gamemodes.SetActive(false);

        tankSetup.SetActive(true);

        gameController.GetComponent<PlayerInputManager>().EnableJoining();
    }

    public void LastManStandingButton()
    {
        gameController.SetGamemode("LastManStanding");

        gamemodes.SetActive(false);

        tankSetup.SetActive(true);

        gameController.GetComponent<PlayerInputManager>().EnableJoining();
    }
}
