using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KingOfTheHill : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] GameObject loadingScreen;

    [Header("Spawns")]
    [SerializeField] GameObject[] tankSpawns;
    [SerializeField] TMP_Text[] respawnCountdownTexts;
    [SerializeField] int respawnTime;

    [Header("Hill")]
    [SerializeField] HillController hill;
    [SerializeField] int winningHillTime;

    [Header("Countdown")]
    [SerializeField] int countdownLength;
    [SerializeField] TMP_Text countdownText;

    [Header("Hill Times")]
    [SerializeField] TMP_Text[] hillTimeTexts;

    [Header("Winner")]
    [SerializeField] TMP_Text winnerText;
    [SerializeField] TMP_Text winsText;

    [Header("Scoreboard")]
    [SerializeField] TMP_Text scoreboardHeaderText;
    [SerializeField] TMP_Text[] scoreboardColorTexts;
    [SerializeField] TMP_Text[] scoreboardWinsTexts;

    [Header("End of Level Menu")]
    [SerializeField] GameObject endOfLevelMenu;

    TankController[] tanks;
    List<TankController> respawningTanks;
    TankController winner;

    GameController gameController;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        respawningTanks = new List<TankController>();

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

        DisplayHillTimes();

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

        hill.gameObject.SetActive(true);

        while (!Winner())
        {
            UpdateHillTimes();

            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i].transform.Find("TankRenderers").GetComponentInChildren<MeshRenderer>().enabled || respawningTanks.Contains(tanks[i]))
                {
                    continue;
                }

                respawningTanks.Add(tanks[i]);

                StartCoroutine(RespawnTank(tanks[i]));
            }

            yield return null;
        }

        UpdateHillTimes();
    }

    IEnumerator EndGame()
    {
        DisableTankControls();

        DisableTankAudio();

        hill.gameObject.SetActive(false);

        winner = GetWinner();

        if (winner != null)
        {
            AddWin();
        }

        DisplayWinner();

        yield return new WaitForSeconds(3f);

        winnerText.gameObject.SetActive(false);
        winsText.gameObject.SetActive(false);

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

    IEnumerator RespawnTank(TankController tank)
    {
        GameObject safestSpawn = tankSpawns[0];
        float greatestAvgDistToTanks = 0;
        float averageDistance;
        int tanksAlive;
        int safestSpawnIndex = 0;

        for (int i = 0; i < tankSpawns.Length; i++)
        {
            averageDistance = 0;
            tanksAlive = 0;

            for (int j = 0; j < tanks.Length; j++)
            {
                averageDistance += Vector3.Distance(tankSpawns[i].transform.position, tanks[j].transform.position);
                tanksAlive++;
            }

            averageDistance /= tanksAlive;

            if (averageDistance > greatestAvgDistToTanks)
            {
                safestSpawn = tankSpawns[i];
                safestSpawnIndex = i;
                greatestAvgDistToTanks = averageDistance;
            }
        }

        tank.transform.position = safestSpawn.transform.position;
        tank.transform.rotation = safestSpawn.transform.rotation;
        tank.transform.Find("TankRenderers").Find("TankTurret").transform.rotation = tank.transform.rotation;

        if (tank.GetTankColorStr().Equals("Yellow"))
        {
            respawnCountdownTexts[safestSpawnIndex].outlineColor = respawnCountdownTexts[safestSpawnIndex].faceColor;
        }

        respawnCountdownTexts[safestSpawnIndex].faceColor = tank.GetTankColor();

        respawnCountdownTexts[safestSpawnIndex].gameObject.SetActive(true);

        int countdown = respawnTime;

        while (countdown > 0)
        {
            respawnCountdownTexts[safestSpawnIndex].text = countdown.ToString();

            yield return new WaitForSeconds(1f);

            countdown--;
        }

        respawnCountdownTexts[safestSpawnIndex].gameObject.SetActive(false);

        tank.gameObject.GetComponent<TankHealth>().Respawn();

        respawningTanks.Remove(tank);
    }

    void DisplayHillTimes()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].ResetTotalHillTime();

            if ((i == 0 && tanks[i].GetTankColorStr().Equals(tanks[i + 1].GetTankColorStr())) ||
                    (i == 3 && tanks[i].GetTankColorStr().Equals(tanks[i - 1].GetTankColorStr())))
            {
                continue;
            }

            if (tanks[i].GetTankColorStr().Equals("Yellow"))
            {
                hillTimeTexts[i].outlineColor = hillTimeTexts[i].faceColor;
            }

            hillTimeTexts[i].faceColor = tanks[i].GetTankColor();

            if (tanks.Length == 2)
            {
                hillTimeTexts[i].transform.position = new Vector3(hillTimeTexts[i].transform.position.x + 200, hillTimeTexts[i].transform.position.y, hillTimeTexts[i].transform.position.z);
            }
            else if (tanks.Length == 3)
            {
                hillTimeTexts[i].transform.position = new Vector3(hillTimeTexts[i].transform.position.x + 100, hillTimeTexts[i].transform.position.y, hillTimeTexts[i].transform.position.z);
            }

            hillTimeTexts[i].gameObject.SetActive(true);
        }
    }

    void UpdateHillTimes()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if ((i == 0 && tanks[i].GetTankColorStr().Equals(tanks[i + 1].GetTankColorStr())) ||
                    (i == 3 && tanks[i].GetTankColorStr().Equals(tanks[i - 1].GetTankColorStr())))
            {
                continue;
            }

            hillTimeTexts[i].text = Mathf.Floor(tanks[i].GetTotalHillTime()).ToString();
        }
    }

    bool Winner()
    {
        bool winner = false;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].GetTotalHillTime() >= winningHillTime)
            {
                winner = true;
                break;
            }
        }

        return winner;
    }

    TankController GetWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].GetTotalHillTime() >= winningHillTime)
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
    }

    public void AddHillTime(TankController tank, float time)
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].GetTankColorStr().Equals(tank.GetTankColorStr()))
            {
                tanks[i].AddHillTime(time);
            }
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
