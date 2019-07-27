﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        GameManager.instance.RegisterPowerUp();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject == player)
        {
            playerHealth.PowerUpHealth();
            Destroy(gameObject);
        }
    }
}
