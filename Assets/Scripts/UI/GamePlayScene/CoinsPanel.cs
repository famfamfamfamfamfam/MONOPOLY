using UnityEngine;
using TMPro;

public class CoinsPanel : MonoBehaviour, INeedDriver
{
    public Driver driver { get; set; }

    [SerializeField]
    TextMeshProUGUI[] playersCoin;

    void Start()
    {
        for (int i = 0; i < playersCoin.Length; i++)
        {
            UpdateCoin(i, driver.PlayersInitialCoin());
        }
        driver.OnPlayerCoinChange(UpdateCoin);
    }

    void UpdateCoin(int playerIndex, int value)
    {
        playersCoin[playerIndex].text = value.ToString();
    }
}