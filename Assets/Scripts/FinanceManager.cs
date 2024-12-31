using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinanceManager : MonoBehaviour
{
    public TextMeshProUGUI soulcounter;
    public int playerSoul = 100;
    public TextMeshProUGUI finalsoul;
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
        return false; 
    }

    private void UpdateSoulUI()
    {
        soulcounter.text = ":" + playerSoul;
        finalsoul.text = "You freed " + playerSoul + " souls!";
    }
    public int GetPlayerSoul() {
        return playerSoul;
    }
}
