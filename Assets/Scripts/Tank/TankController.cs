using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] TankMovement tankMovement;
    [SerializeField] TankShoot tankShoot;
    [SerializeField] TankHealth tankHealth;
    [SerializeField] AudioSource engineAudio;

    Color tankColor;
    string tankColorStr;

    int totalWins;
    float totalHillTime;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void EnableControls()
    {
        tankMovement.enabled = true;
        tankShoot.enabled = true;

    }

    public void DisableControls()
    {
        tankMovement.enabled = false;
        tankShoot.enabled = false;
    }

    public void EnableHealth()
    {
        tankHealth.enabled = true;

        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void EnableAudio()
    {
        engineAudio.enabled = true;
    }

    public void DisableAudio()
    {
        engineAudio.enabled = false;
    }

    public void SetTankColor(Material color)
    {
        tankColor = color.color;

        SetTankColorStr(color);

        transform.Find("TankRenderers").Find("TankChassis").GetComponent<MeshRenderer>().material = color;
        transform.Find("TankRenderers").Find("TankTracksLeft").GetComponent<MeshRenderer>().material = color;
        transform.Find("TankRenderers").Find("TankTracksRight").GetComponent<MeshRenderer>().material = color;
        transform.Find("TankRenderers").Find("TankTurret").GetComponent<MeshRenderer>().material = color;
    }

    public Color GetTankColor()
    {
        return tankColor;
    }

    void SetTankColorStr(Material color)
    {
        tankColorStr = color.name;
    }

    public string GetTankColorStr()
    {
        return tankColorStr;
    }

    public void AddWin()
    {
        totalWins++;
    }

    public int GetTotalWins()
    {
        return totalWins;
    }

    public void AddHillTime(float time)
    {
        totalHillTime += time;
    }

    public void ResetTotalHillTime()
    {
        totalHillTime = 0f;
    }

    public float GetTotalHillTime()
    {
        return totalHillTime;
    }
}
