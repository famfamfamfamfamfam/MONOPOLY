using UnityEngine;
using System.Collections.Generic;

public class BoardDataService
{
    BoardData data;
    bool hasRemovedFromPurchasableSpaces;
    public BoardDataService(BoardData data)
    {
        this.data = data;
    }

    public void GiveBusTicket(out int ticket)
    {
        SetUpToGive(data.currentTakableBusTickets, out ticket);
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
        hasRemovedFromPurchasableSpaces = true;
        return true;
    }
    void SetUpToGive(List<int> holder, out int output)
    {
        int randomIndex = Random.Range(0, holder.Count);
        output = holder[randomIndex];
        holder[randomIndex] = holder[holder.Count - 1];
        holder.RemoveAt(holder.Count - 1);
    }

    public void TakeBackBusTicket(int ticket)
    {
        data.currentTakableBusTickets.Add(ticket);
    }

    public void GrantSpace(int spaceIndex)
    {
        //lấy index của index-của-space trong list
        //sau đó ghi đè giá trị cuối của list vào phần tử có index đã lấy
        //độ phức tạp O(n) nhưng thực tế nhanh hơn so với phương thức Remove (ít hoán đổi hơn)
        if (hasRemovedFromPurchasableSpaces)
        {
            hasRemovedFromPurchasableSpaces = false;
            return;
        }
        data.currentPurchasableSpaces[data.currentPurchasableSpaces.IndexOf(spaceIndex)]
            = data.currentPurchasableSpaces[data.currentPurchasableSpaces.Count - 1];
        //xóa phần tử cuối để đạt độ phức tạp O(1)
        data.currentPurchasableSpaces.RemoveAt(data.currentPurchasableSpaces.Count - 1);
    }
}