using System;

[Serializable]
public struct CommunityChest
{
    public enum MoneyChangeType
    {
        AllChange,
        Opposite,
        Donate
    }
    public MoneyChangeType moneyChange;
    public int value;
}

[Serializable]
public class CommunityChestAction
{
    public void ChangeAllCoin(PlayerData[] players, int value)
    {
        foreach (var player in players)
        {
            player.SetCurrentCoin(value);
        }
    }

    public void OppositelyChangeCoin(PlayerData currentPlayer, PlayerData[] players, int value)
    {
        ChangeCoin(currentPlayer, players, value, 1);
    }

    public void ChangeCoinByDonate(PlayerData currentPlayer, PlayerData[] players, int value)
    {
        ChangeCoin(currentPlayer, players, value, players.Length - 1);
    }

    void ChangeCoin(PlayerData currentPlayer, PlayerData[] players, int value, int divisor)
    {
        foreach (var player in players)
        {
            if (player == currentPlayer)
            {
                player.SetCurrentCoin(value);
                continue;
            }
            player.SetCurrentCoin(-value / divisor);
        }
    }
}