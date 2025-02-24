using System;
using System.Diagnostics;

public class PlayerDataService
{
    PlayerData[] allPlayersData;
    public PlayerDataService(PlayerData[] playersData)
    {
        allPlayersData = playersData;
    }

    public void AddAsset(int playerIndex, IAsset asset, Action<int> stationCostUpdate)
    {
        allPlayersData[playerIndex].assets.Add(asset);
        switch (asset.type)
        {
            case IAsset.AssetType.Station:
                OnAddAStation(playerIndex, stationCostUpdate);
                return;
            case IAsset.AssetType.Company:
                OnAddACompany(allPlayersData[playerIndex]);
                return;
        }
    }

    void OnAddAStation(int playerIndex, Action<int> stationCostUpdate)
    {
        if (allPlayersData[playerIndex].currentStationCount == 0)
        {
            allPlayersData[playerIndex].currentStationCount++;
            return;
        }
        stationCostUpdate.Invoke(playerIndex);
    }

    void OnAddACompany(PlayerData data)
    {
        data.currentCompanyCount++;
    }

    public void KeepTicket(int playerIndex, int ticketIndex)
    {
        allPlayersData[playerIndex].busTickets.Add(ticketIndex);
    }

    public void GiveBackTicket(int playerIndex, int ticketIndex)
    {
        allPlayersData[playerIndex].busTickets.Remove(ticketIndex);
    }

    public bool IsOwner(int playerIndex, IAsset asset)
    {
        return allPlayersData[playerIndex].assets.Contains(asset);
    }

    public void SetCurrentCoin(int playerIndex, int addValue)
    {
        allPlayersData[playerIndex].currentCoin += addValue;
        UnityEngine.Debug.Log(allPlayersData[playerIndex].currentCoin);
    }

    public void IterateAllPlayers(Action<int> actionForEachPlayer, Func<bool> canBreak = null)
    {
        for (int i = 0; i < allPlayersData.Length; i++)
        {
            actionForEachPlayer.Invoke(i);
            if (canBreak != null && canBreak.Invoke())
            {
                break;
            }
        }
    }

    public int GetDicePoint(int playerIndex)
    {
        return allPlayersData[playerIndex].currentDicePoint;
    }

    public void SetDicePoint(int playerIndex, int point)
    {
        allPlayersData[playerIndex].currentDicePoint = point;
    }

    public int GetCurrentCompanyCount(int playerIndex)
    {
        return allPlayersData[playerIndex].currentCompanyCount;
    }

    public int GetCurrentStationCount(int playerIndex)
    {
        return allPlayersData[playerIndex].currentStationCount;
    }

    public bool IsInJail(int playerIndex)
    {
        return allPlayersData[playerIndex].isInJail;
    }

    public void BeInJail(int playerIndex)
    {
        allPlayersData[playerIndex].isInJail = true;
    }

    public void QuitFromJail(int playerIndex)
    {
        allPlayersData[playerIndex].isInJail = false;
    }

}