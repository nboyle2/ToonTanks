using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] tankSetupMenus;

    List<TankController> tanks;

    string[] levels;
    int levelIndex;

    string gamemode;

    int readyTanks;

    bool started;

    void Awake()
    {
        DontDestroyOnLoad(this);

        tanks = new List<TankController>();

        levels = new string[] { "Barracks", "Forest", "Refinery" };

        ShuffleMaps();
    }

    void Update()
    {
        if (!started && CanStart())
        {
            started = true;

            OrderTanks();

            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (levelIndex >= levels.Length)
        {
            ShuffleMaps();
            levelIndex = 0;
        }

        SceneManager.LoadScene(levels[levelIndex++]);
    }

    void ShuffleMaps()
    {
        int i = levels.Length;

        while (i > 1)
        {
            int randIndex = Random.Range(0, i--);

            string temp = levels[i];
            levels[i] = levels[randIndex];
            levels[randIndex] = temp;
        }

        if (levels[0].Equals(SceneManager.GetActiveScene().name))
        {
            string temp = levels[0];
            levels[0] = levels[1];
            levels[1] = temp;
        }
    }

    void OrderTanks()
    {
        tanks = tanks.OrderBy(tank => tank.GetTankColorStr()).ToList();
    }

    public List<TankController> GetTanks()
    {
        return tanks;
    }

    public void SetGamemode(string gamemode)
    {
        this.gamemode = gamemode;
    }

    public string GetGamemode()
    {
        return gamemode;
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        switch (playerInput.playerIndex)
        {
            case 0:
                playerInput.gameObject.transform.position = new Vector3(-6f, 0f, 0f);
                break;
            case 1:
                playerInput.gameObject.transform.position = new Vector3(-2f, 0f, 0f);
                break;
            case 2:
                playerInput.gameObject.transform.position = new Vector3(2f, 0f, 0f);
                break;
            case 3:
                playerInput.gameObject.transform.position = new Vector3(6f, 0f, 0f);
                break;
        }

        playerInput.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        playerInput.uiInputModule = tankSetupMenus[playerInput.playerIndex].GetComponentInChildren<InputSystemUIInputModule>();
        tankSetupMenus[playerInput.playerIndex].GetComponent<TankSetupMenu>().SetTankIndex(playerInput.playerIndex);
        tankSetupMenus[playerInput.playerIndex].gameObject.SetActive(true);

        tanks.Add(playerInput.gameObject.GetComponent<TankController>());
    }

    public void SetTankColor(int tankIndex, Material color)
    {
        tanks[tankIndex].SetTankColor(color);
    }

    public void ReadyTank()
    {
        readyTanks++;
    }

    public void UnreadyTank()
    {
        readyTanks--;
    }

    public bool CanStart()
    {
        return tanks.Count > 1 && readyTanks == tanks.Count;
    }
}
