using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GUIController guiController;
    private int totalTurrets, actualTurrets;
    
    private void Start()
    {
        var turrets = FindObjectsOfType<Turret>();
        totalTurrets = actualTurrets = turrets.Length;
        foreach (var turret in turrets)
        {
            turret.OnTurretDestroyed += TurretDestroyed;
        }
        guiController.UpdateTurretText(totalTurrets,actualTurrets);
    }

    private void TurretDestroyed()
    {
        actualTurrets--;
        guiController.UpdateTurretText(totalTurrets,actualTurrets);
    }
}
