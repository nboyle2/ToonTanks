using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject gamemodes;

    [SerializeField] GameObject tankSetup;

    [SerializeField] GameController gameController;

    public void QuitButton()
    {
        Application.Quit();
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
