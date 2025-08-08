using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheDeveloperTrain.SciFiGuns;

public class Powerups : MonoBehaviour
{

    public int pointCost = 200;
    public GameObject panel;
    public GameObject redPanel;
    public GameObject panelUI;
    private bool playerInRange = false;
    private PointManager pointManager;
    private PlayerMovement playerMovement;
    private HealthManager healthManager;
    private BulletDamage bulletDamage;
    Gun gun = null;



    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gun = mainCamera.GetComponentInChildren<Gun>();
        gun.currentFireMode = FireMode.Single;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (pointManager != null && pointManager.SpendPoints(pointCost))
            {
                if (panelUI != null)
                {
                    panelUI.SetActive(false);
                    Destroy(panel);
                    Destroy(redPanel);
                }

                if (panel.CompareTag("SpeedBoost"))
                {
                    SpeedBoost();
                } else if (panel.CompareTag("HealthBoost"))
                {
                    HealthBoost();
                }
                else if (panel.CompareTag("DamageBoost"))
                {
                    DamageBoost();
                }
                else if (panel.CompareTag("PointBoost"))
                {
                    PointBoost();
                }
                else if (panel.CompareTag("BurstFire"))
                {
                    BurstFire();
                }
            }
            else
            {
                Debug.Log("Not enough points.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pointManager = other.GetComponent<PointManager>();
            playerMovement = other.GetComponent<PlayerMovement>();
            healthManager = other.GetComponent<HealthManager>();
            bulletDamage = other.GetComponent<BulletDamage>();
            
            if (panelUI != null)
            {
                panelUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pointManager = null;

            if (panelUI != null)
            {
                panelUI.SetActive(false);
            }
        }
    }

    public void SpeedBoost()
    {
        playerMovement.moveSpeed = 7;
    }

    public void HealthBoost()
    {
        healthManager.maxHealth += 50;
        healthManager.currentHealth = healthManager.maxHealth;
        healthManager.healthbar.SetMaxHealth(healthManager.maxHealth);
    }

    public void DamageBoost()
    {
        gun.baseDamage += 15;
    }

    public void PointBoost()
    {
        pointManager.addAmount += 25;
    }

    public void BurstFire()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (mainCamera != null)
        {
            gun = mainCamera.GetComponentInChildren<Gun>();
        }
        if (gun != null)
        {
            gun.currentFireMode = FireMode.Burst;
        }
    }
}
