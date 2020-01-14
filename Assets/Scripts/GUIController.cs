using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    [SerializeField] private Image primaryWeaponUI;
    [SerializeField] private Image secondaryWeaponUI;
    [SerializeField] private Image specialWeaponUI;
    [SerializeField] private Text turretsText;
    [SerializeField] private Image lifeUI;
    [SerializeField] private Text lifeText;
    [SerializeField] private DroneController droneController;
    [SerializeField] private GameObject rewindEffect;
    
    private bool primaryWeaponReady = true;
    private int totalPlayerLife;

    private void Awake()
    {
        droneController.OnPrimaryWeaponFire += PrimaryWeaponShoted;
        droneController.OnSecondaryWeaponFire += SecondaryWeaponShoted;
        DroneController.OnRewindStart += SpecialWeaponShoted;
        DroneController.OnRewindFinish += SpecialWeaponFinish;
        droneController.OnPlayerDamage += UpdatePlayerLife;
        totalPlayerLife = droneController.PlayerLife;
    }

    private void Start()
    {
        UpdatePlayerLife();    
    }

    private void PrimaryWeaponShoted()
    {
        StartCoroutine(FillImage(primaryWeaponUI,1f));
    }

    private void SecondaryWeaponShoted()
    {
        StartCoroutine(FillImage(secondaryWeaponUI,droneController.MissileCooldown));
    }

    private void SpecialWeaponShoted()
    {
        rewindEffect.SetActive(true);
        StartCoroutine(FillImage(specialWeaponUI,droneController.RewindCooldown));
    }

    private void SpecialWeaponFinish()
    {
        rewindEffect.SetActive(false);
    }

    private IEnumerator FillImage(Image image, float time)
    {
        image.fillAmount = 0;
        while(image.fillAmount !=1)
        {
            image.fillAmount +=.1f;
            yield return new WaitForSecondsRealtime(time/10);
        }
    }

    private void UpdatePlayerLife()
    {
        if(droneController.PlayerLife == 0)
            lifeUI.fillAmount = 0;
        else
            lifeUI.fillAmount = (float)droneController.PlayerLife/(float)totalPlayerLife;
        lifeText.text = $"{droneController.PlayerLife*10}/{totalPlayerLife*10}";
    }

    public void UpdateTurretText(int totalTurrets, int actualTurrets)
    {
        turretsText.text = $"{actualTurrets}/{totalTurrets}";
    }
}
