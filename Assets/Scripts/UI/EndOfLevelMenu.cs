using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelMenu : MonoBehaviour
{
    GameController gameController;

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void NextLevelButton()
    {
        gameController.LoadNextLevel();
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        List<TankController> tanks = gameController.GetTanks();

        Destroy(gameController.gameObject);

        for (int i = 0; i < tanks.Count; i++)
        {
            Destroy(tanks[i].gameObject);
        }

        Destroy(GameObject.Find("Music"));

        SceneManager.LoadScene("MainMenu");
    }
}
