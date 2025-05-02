using UnityEngine;
using TMPro;

public class CoinsPanel : MonoBehaviour
{
    [SerializeField]
    DataManager dataManager;

    [SerializeField]
    TextMeshProUGUI[] playersCoin;

    void Start()
    {
        for (int i = 0; i < playersCoin.Length; i++)
        {
            UpdateCoin(i, dataManager.PlayersInitialCoin());
        }
        dataManager.OnPlayerCoinChange(UpdateCoin);
    }

    void UpdateCoin(int playerIndex, int value)
    {
        playersCoin[playerIndex].text = value.ToString();
    }
}