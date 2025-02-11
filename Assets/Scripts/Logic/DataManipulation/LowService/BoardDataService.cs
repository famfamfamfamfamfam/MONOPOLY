using UnityEngine;
using System.Collections.Generic;

public class BoardDataService
{
    BoardData data;
    public BoardDataService(BoardData data)
    {
        this.data = data;
    }

    public void GiveBusTicket(out int ticketIndex)
    {
        SetUpToGive(data.currentTakableBusTickets, out ticketIndex);
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

    public void TakeBackBusTicket(int ticketIndex)
    {
        data.currentTakableBusTickets.Add(ticketIndex);
    }

    public void GrantSpace(int spaceIndex)
    {
        data.currentPurchasableSpaces.Remove(spaceIndex);
    }
}