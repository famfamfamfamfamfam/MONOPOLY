using System;
using System.Collections.Generic;

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
    Action<PlayerData, int> playerTakeBusTicket;

    Action[] actions;

    Dictionary<int, int> actionAccessor;

    public void GoToJail(Action go)
    {
        Init(0, go);
    }
    public void MoveToAUtilitySpace(Action move)
    {
        Init(1, move);
    }
    public void BackToGoSpace(Action back)
    {
        Init(2, back);
    }
    public void RollThirdDieToMove(Action rollAndMove)
    {
        Init(3, rollAndMove);
    }
    public void MoveToAuction(Action move)
    {
        Init(4, move);
    }
    public void QuitFromJail(Action quit)
    {
        Init(5, quit);
    }
    void Init(int index, Action action)
    {
        if (actions == null)
        {
            actions = new Action[GlobalFieldContainer.allTicketType];
        }
        actions[index] = action;
    }

    public void GiveTicket(PlayerData data, int ticketIndex, int actionIndex)
    {
        playerTakeBusTicket.Invoke(data, ticketIndex);
        if (actionAccessor == null)
        {
            actionAccessor = new Dictionary<int, int>((int)(GlobalFieldContainer.allKeepToUseTicket / GlobalFieldContainer.RESIZE_THRESHOLD) + 1);
        }
        else
        {
            if (actionAccessor.TryAdd(ticketIndex, actionIndex) && actionAccessor.Count == GlobalFieldContainer.allKeepToUseTicket)
            {
                actionAccessor.TrimExcess();
            }
        }
    }
    public void OnGiveTicket(Action<PlayerData, int> action)
    {
        playerTakeBusTicket = action;
    }

    public void TriggerInstantUseTicket(int actionIndex)
    {
        actions[actionIndex].Invoke();
    }

    public void TriggerKeepToUseTicket(int ticketIndex)
    {
        actions[actionAccessor[ticketIndex]].Invoke();
    }
}