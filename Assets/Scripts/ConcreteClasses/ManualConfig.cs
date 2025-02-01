using System;

[Serializable]
public struct CommunityChest
{
    public enum MoneyChangeType : byte
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

[Serializable]
public class ChanceAction
{
    Action<PlayerData> changeToCommunityCard;
    Action<PlayerData> changeToBusTicket;

    public void ChangeCoin(PlayerData data, int value)
    {
        data.SetCurrentCoin(value);
    }

    public void ChangeToCommunityCard(PlayerData data)
    {
        changeToCommunityCard.Invoke(data);
    }

    public void ChangeToBusTicket(PlayerData data)
    {
        changeToBusTicket.Invoke(data);
    }

    public void OnChangeToCommunityCard(Action<PlayerData> action)
    {
        changeToCommunityCard = action;
    }

    public void OnChangeToBusTicket(Action<PlayerData> action)
    {
        changeToBusTicket = action;
    }
}

[Serializable]
public class BusTicketAction
{
    Action<PlayerData> takeBusTicket;

    public void GoToJail()
    {

    }
    public void BackToGoSpace()
    {

    }
    public void MoveToAUtilitySpace()
    {

    }

    public void TakeTicket(PlayerData data)
    {
        takeBusTicket.Invoke(data);
    }
    public void OnTakeTicket(Action<PlayerData> action)
    {
        takeBusTicket = action;
    }
}