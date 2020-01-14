using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
}
