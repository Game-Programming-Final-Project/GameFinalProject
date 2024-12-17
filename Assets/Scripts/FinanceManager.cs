using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinanceManager : MonoBehaviour
{
    public TextMeshProUGUI soulcounter;
    public int playerSoul = 100;
    
    private void Start()
    {
        UpdateSoulUI();
    }

    public void AddSoul(int amount)
    {
        playerSoul += amount;
        UpdateSoulUI();
    }

    public bool SpendSoul(int amount)
    {
        if (playerSoul >= amount)
        {
            playerSoul -= amount;
            UpdateSoulUI();
            return true;
        }
        return false; // Yeterli soul yok 
    }

    private void UpdateSoulUI()
    {
        soulcounter.text = ":" + playerSoul;
    }
    public int GetPlayerSoul() {
        return playerSoul;
    }
}
