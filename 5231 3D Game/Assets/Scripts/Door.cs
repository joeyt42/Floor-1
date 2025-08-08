using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public int pointCost = 200;
    public GameObject door;
    private bool playerInRange = false;
    private PointManager pointManager;
    public GameObject doorUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pointManager = other.GetComponent<PointManager>();

            if (doorUI != null)
            {
                doorUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pointManager = null;

            if (doorUI != null)
            {
                doorUI.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (pointManager != null && pointManager.SpendPoints(pointCost))
            {
                Destroy(door);
                if (doorUI != null)
                {
                    doorUI.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Not enough points to open the door.");
            }
        }
    }
}
