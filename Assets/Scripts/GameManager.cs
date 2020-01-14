using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GUIController guiController;
    [SerializeField] private DroneController droneController;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;

    private int totalTurrets, actualTurrets;
    
    private void Start()
    {
        Cursor.visible = false;
        var turrets = FindObjectsOfType<Turret>();
        totalTurrets = actualTurrets = turrets.Length;
        foreach (var turret in turrets)
        {
            turret.OnTurretDestroyed += TurretDestroyed;
        }
        guiController.UpdateTurretText(totalTurrets,actualTurrets);

        droneController.OnDroneDestroid += GameOver;
    }

    private void TurretDestroyed()
    {
        actualTurrets--;
        guiController.UpdateTurretText(totalTurrets,actualTurrets);
        if(actualTurrets == 0)
        {
            Win();
        }
    }

    private void Win()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        winScreen.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
