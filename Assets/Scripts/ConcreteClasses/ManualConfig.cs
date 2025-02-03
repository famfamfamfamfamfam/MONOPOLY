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
    public void ChangeAllCoin(IChangeCoin[] players, int value)
    {
        foreach (var player in players)
        {
            player.SetCurrentCoin(value);
        }
    }

    public void OppositelyChangeCoin(IChangeCoin currentPlayer, IChangeCoin[] players, int value)
    {
        ChangeCoin(currentPlayer, players, value, 1);
    }

    public void ChangeCoinByDonate(IChangeCoin currentPlayer, IChangeCoin[] players, int value)
    {
        ChangeCoin(currentPlayer, players, value, players.Length - 1);
    }

    void ChangeCoin(IChangeCoin currentPlayer, IChangeCoin[] players, int value, int divisor)
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
    Action<IChangeCoin> changeToCommunityCard;
    Action<IChangeCoin> changeToBusTicket;

    public void ChangeCoin(IChangeCoin data, int value)
    {
        data.SetCurrentCoin(value);
    }

    public void ChangeToCommunityCard(IChangeCoin data)
    {
        changeToCommunityCard.Invoke(data);
    }

    public void ChangeToBusTicket(IChangeCoin data)
    {
        changeToBusTicket.Invoke(data);
    }

    public void OnChangeToCommunityCard(Action<IChangeCoin> action)
    {
        changeToCommunityCard = action;
    }

    public void OnChangeToBusTicket(Action<IChangeCoin> action)
    {
        changeToBusTicket = action;
    }
}

[Serializable]
public class BusTicketAction
{
    Action<ICanKeepTicket, int> playerTakeBusTicket;

    Action<IOnEvent>[] actions;

    Dictionary<int, int> actionAccessor;

    public void GoToJail(Action<IOnEvent> go)
    {
        Init(0, go);
    }
    public void MoveToAUtilitySpace(Action<IOnEvent> move)
    {
        Init(1, move);
    }
    public void BackToGoSpace(Action<IOnEvent> back)
    {
        Init(2, back);
    }
    public void RollThirdDieToMove(Action<IOnEvent> rollAndMove)
    {
        Init(3, rollAndMove);
    }
    public void MoveToAuction(Action<IOnEvent> move)
    {
        Init(4, move);
    }
    public void QuitFromJail(Action<IOnEvent> quit)
    {
        Init(5, quit);
    }
    void Init(int index, Action<IOnEvent> action)
    {
        if (actions == null)
        {
            actions = new Action<IOnEvent>[GlobalFieldContainer.allTicketType];
        }
        actions[index] = action;
    }

    public void GiveTicket(ICanKeepTicket data, int ticketIndex, int actionIndex)
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
    public void OnGiveTicket(Action<ICanKeepTicket, int> action)
    {
        playerTakeBusTicket = action;
    }

    public void TriggerInstantUseTicket(IOnEvent currentPlayer, int actionIndex)
    {
        actions[actionIndex].Invoke(currentPlayer);
    }

    public void TriggerKeepToUseTicket(IOnEvent currentPlayer, int ticketIndex)
    {
        actions[actionAccessor[ticketIndex]].Invoke(currentPlayer);
    }
}