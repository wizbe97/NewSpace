using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int gold = 0;
    public TextMeshProUGUI goldText; // Reference to the TextMeshProUGUI component displaying gold

    private void Start()
    {
        UpdateGoldUI(); // Update the gold UI at the start of the game
    }

    // Method to add gold to the player's stats
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    // Method to update the gold UI
    private void UpdateGoldUI()
    {
        // Check if goldText is assigned
        if (goldText != null)
        {
            // Update the text to display the player's gold amount
            goldText.text = "Gold: " + gold.ToString();
        }
        else
        {
            Debug.LogWarning("Gold Text component is not assigned.");
        }
    }
}
