using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LastManStanding : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] GameObject loadingScreen;

    [Header("Spawns")]
    [SerializeField] GameObject[] tankSpawns;

    [Header("Countdown")]
    [SerializeField] int countdownLength;
    [SerializeField] TMP_Text countdownText;

    [Header("Winner")]
    [SerializeField] TMP_Text winnerText;
    [SerializeField] TMP_Text winsText;
    [SerializeField] TMP_Text drawText;
    TankController winner;

    [Header("Scoreboard")]
    [SerializeField] TMP_Text scoreboardHeaderText;
    [SerializeField] TMP_Text[] scoreboardColorTexts;
    [SerializeField] TMP_Text[] scoreboardWinsTexts;

    [Header("End of Level Menu")]
    [SerializeField] GameObject endOfLevelMenu;

    TankController[] tanks;

    GameController gameController;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        yield return StartCoroutine(LoadingScreen());

        yield return StartCoroutine(StartGame());

        yield return StartCoroutine(Gameplay());

        yield return StartCoroutine(EndGame());

        yield return StartCoroutine(Menu());
    }

    IEnumerator LoadingScreen()
    {
        switch (gameController.GetGamemode())
        {
            case "KingOfTheHill":
                loadingScreen.transform.Find("KingOfTheHill").gameObject.SetActive(true);
                break;
            case "LastManStanding":
                loadingScreen.transform.Find("LastManStanding").gameObject.SetActive(true);
                break;
        }

        yield return new WaitForSeconds(5f);

        loadingScreen.gameObject.SetActive(false);
    }

    IEnumerator StartGame()
    {
        SpawnTanks();

        EnableTankHealth();

        EnableTankAudio();

        countdownText.gameObject.SetActive(true);

        while (countdownLength > 0)
        {
            countdownText.text = countdownLength.ToString();

            yield return new WaitForSeconds(1f);

            countdownLength--;
        }

        countdownText.text = "Fight!";

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }

    IEnumerator Gameplay()
    {
        EnableTankControls();

        while (!Winner())
        {
            yield return null;
        }
    }

    IEnumerator EndGame()
    {
        DisableTankControls();

        DisableTankAudio();

        winner = GetWinner();

        if (winner != null)
        {
            AddWin();
        }

        DisplayWinner();

        yield return new WaitForSeconds(3f);

        winnerText.gameObject.SetActive(false);
        winsText.gameObject.SetActive(false);
        drawText.gameObject.SetActive(false);

        DisplayScoreboard();

        yield return new WaitForSeconds(5f);
    }

    IEnumerator Menu()
    {
        scoreboardHeaderText.gameObject.SetActive(false);

        for (int i = 0; i < tanks.Length; i++)
        {
            if (i > 0 && tanks[i].GetTankColorStr().Equals(tanks[i - 1].GetTankColorStr()))
            {
                continue;
            }

            scoreboardColorTexts[i].gameObject.SetActive(false);
            scoreboardWinsTexts[i].gameObject.SetActive(false);
        }

        endOfLevelMenu.gameObject.SetActive(true);

        yield return null;
    }

    void EnableTankHealth()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableHealth();
        }
    }

    void EnableTankAudio()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableAudio();
        }
    }

    void EnableTankControls()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControls();
        }
    }

    void DisableTankControls()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControls();
        }
    }

    void DisableTankAudio()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableAudio();
        }
    }

    void SpawnTanks()
    {
        tanks = gameController.GetTanks().ToArray();

        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].GetComponent<TankHealth>().Respawn();

            tanks[i].transform.position = tankSpawns[i].transform.position;
            tanks[i].transform.rotation = tankSpawns[i].transform.rotation;
            tanks[i].transform.Find("TankRenderers").Find("TankTurret").transform.rotation = tanks[i].transform.rotation;
        }
    }

    bool Winner()
    {
        bool winner = true;

        for (int i = 0; i < tanks.Length - 1; i++)
        {
            if (!tanks[i].transform.Find("TankRenderers").GetComponentInChildren<MeshRenderer>().enabled)
            {
                continue;
            }

            for (int j = i + 1; j < tanks.Length; j++)
            {
                if (!tanks[j].transform.Find("TankRenderers").GetComponentInChildren<MeshRenderer>().enabled)
                {
                    continue;
                }

                if (!tanks[i].GetTankColorStr().Equals(tanks[j].GetTankColorStr()))
                {
                    winner = false;
                }
            }
        }

        return winner;
    }

    TankController GetWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].transform.Find("TankRenderers").GetComponentInChildren<MeshRenderer>().enabled)
            {
                return tanks[i];
            }
        }

        return null;
    }

    void DisplayWinner()
    {
        if (winner != null)
        {
            winnerText.text = winner.GetTankColorStr();

            if (winner.GetTankColorStr().Equals("Yellow"))
            {
                winnerText.outlineColor = winnerText.faceColor;
            }

            winnerText.faceColor = winner.GetTankColor();

            winnerText.gameObject.SetActive(true);
            winsText.gameObject.SetActive(true);
        }
        else
        {
            drawText.gameObject.SetActive(true);
        }
    }

    void AddWin()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].GetTankColorStr().Equals(winner.GetTankColorStr()))
            {
                tanks[i].AddWin();
            }
        }
    }

    void DisplayScoreboard()
    {
        scoreboardHeaderText.gameObject.SetActive(true);

        for (int i = 0; i < tanks.Length; i++)
        {
            if (i > 0 && tanks[i].GetTankColorStr().Equals(tanks[i - 1].GetTankColorStr()))
            {
                continue;
            }

            scoreboardColorTexts[i].text = tanks[i].GetTankColorStr();
            scoreboardWinsTexts[i].text = tanks[i].GetTotalWins().ToString();

            if (tanks[i].GetTankColorStr().Equals("Yellow"))
            {
                scoreboardColorTexts[i].outlineColor = scoreboardColorTexts[i].faceColor;
                scoreboardWinsTexts[i].outlineColor = scoreboardWinsTexts[i].faceColor;
            }

            scoreboardColorTexts[i].faceColor = tanks[i].GetTankColor();
            scoreboardWinsTexts[i].faceColor = tanks[i].GetTankColor();

            scoreboardColorTexts[i].gameObject.SetActive(true);
            scoreboardWinsTexts[i].gameObject.SetActive(true);
        }
    }
}
