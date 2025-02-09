using UnityEngine;
using System.Collections.Generic;

public class BoardDataService : IBusTicketBoardService, ISpaceBoardService
{
    BoardData data;
    public BoardDataService(BoardData data)
    {
        this.data = data;
    }

    public void GiveBusTicket(out int ticketValue)
    {
        SetUpToGive(data.currentTakableBusTickets, out ticketValue);
    }
    public bool GiftASpace(out int spaceIndex)
    {
        if (data.currentPurchasableSpaces.Count == 0)
        {
            Debug.LogWarning("No space left");
            spaceIndex = -1;
            return false;
        }
        SetUpToGive(data.currentPurchasableSpaces, out spaceIndex);
        return true;
    }
    void SetUpToGive(List<int> holder, out int output)
    {
        int randomIndex = Random.Range(0, holder.Count);
        output = holder[randomIndex];
        holder.RemoveAt(randomIndex);
    }

    public void TakeBackBusTicket(int ticket)
    {
        data.currentTakableBusTickets.Add(ticket);
    }

    public void GrantSpace(int spaceIndex)
    {
        data.currentPurchasableSpaces.Remove(spaceIndex);
    }
}

public interface IBusTicketBoardService
{
    void GiveBusTicket(out int ticketValue);
    void TakeBackBusTicket(int ticket);
}

public interface ISpaceBoardService
{
    bool GiftASpace(out int spaceIndex);
    void GrantSpace(int spaceIndex);
}