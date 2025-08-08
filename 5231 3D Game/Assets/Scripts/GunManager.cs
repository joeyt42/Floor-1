using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheDeveloperTrain.SciFiGuns;

public class GunManager : MonoBehaviour
{

    public Gun Gun;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Gun.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Gun.Reload();
        }
    }
}
