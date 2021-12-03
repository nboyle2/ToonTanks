using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankSetupMenu : MonoBehaviour
{
    [SerializeField] Button readyButton;

    GameController gameController;

    int tankIndex;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void SetTankIndex(int index)
    {
        tankIndex = index;
    }

    public void SetTankColor(Material color)
    {
        gameController.SetTankColor(tankIndex, color);
    }

    public void ReadyButton()
    {
        if (readyButton.GetComponentInChildren<TMP_Text>().text.Equals("Ready"))
        {
            gameController.ReadyTank();
            readyButton.GetComponentInChildren<TMP_Text>().text = "Unready";
        }
        else
        {
            gameController.UnreadyTank();
            readyButton.GetComponentInChildren<TMP_Text>().text = "Ready";
        }
    }
}
