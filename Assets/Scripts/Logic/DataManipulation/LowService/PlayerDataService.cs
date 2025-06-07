using System;

public class PlayerDataService
{
    public Action<int, int> coinChanged { get; set; }
    PlayerData[] allPlayersData;
    public PlayerDataService(PlayerData[] playersData)
    {
        allPlayersData = playersData;
    }

    public void AddAsset(int playerIndex, IAsset asset, Action<int> stationCostUpdate)
    {
        UnityEngine.Debug.Log("AddAsset-method runs from PlayerDataService");
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
        UnityEngine.Debug.Log("OnAddAStation-method runs from PlayerDataService");
        if (allPlayersData[playerIndex].currentStationCount == 0)
        {
            allPlayersData[playerIndex].currentStationCount++;
            UnityEngine.Debug.Log("Added the first station using initial rent cost");
            return;
        }
        stationCostUpdate.Invoke(playerIndex);
        UnityEngine.Debug.Log("Added a station and updated rent costs");
    }

    void OnAddACompany(PlayerData data)
    {
        UnityEngine.Debug.Log("OnAddACompany-method runs from PlayerDataService");
        data.currentCompanyCount++;
        UnityEngine.Debug.Log("Added a company");
    }

    public void KeepTicket(int playerIndex, int ticket)
    {
        UnityEngine.Debug.Log("KeepTicket-method runs from PlayerDataService");
        allPlayersData[playerIndex].busTickets.Add(ticket);
        UnityEngine.Debug.Log($"The player at index {playerIndex} kept the ticket {(BusTicketsConfig.KeepToUseTicket)ticket}");
    }

    public void GiveBackTicket(int playerIndex, int ticket)
    {
        UnityEngine.Debug.Log("GiveBackTicket-method runs from PlayerDataService");
        int ticketIndex = allPlayersData[playerIndex].busTickets.IndexOf(ticket);
        int lastIndex = allPlayersData[playerIndex].busTickets.Count - 1;
        allPlayersData[playerIndex].busTickets[ticket] = allPlayersData[playerIndex].busTickets[lastIndex];
        allPlayersData[playerIndex].busTickets.RemoveAt(lastIndex);
        UnityEngine.Debug.Log($"The player at index {playerIndex} gave back the ticket {(BusTicketsConfig.KeepToUseTicket)ticket}");
    }

    public bool IsOwner(int playerIndex, IAsset asset)
    {
        UnityEngine.Debug.Log("IsOwner-method runs from PlayerDataService");
        UnityEngine.Debug.Log("Be checking the player at index " + playerIndex);
        return allPlayersData[playerIndex].assets.Contains(asset);
    }

    public void SetCurrentCoin(int playerIndex, int addValue)
    {
        UnityEngine.Debug.Log("SetCurrentCoin-method runs from PlayerDataService");
        allPlayersData[playerIndex].currentCoin += addValue;
        UnityEngine.Debug.Log($"Set coin of the player at index {playerIndex} to {allPlayersData[playerIndex].currentCoin}");
        coinChanged.Invoke(playerIndex, allPlayersData[playerIndex].currentCoin);
    }

    public void IterateAllPlayers(Action<int> actionForEachPlayer, Func<bool> breakCondition = null)
    {
        UnityEngine.Debug.Log("IterateAllPlayers-method runs from PlayerDataService");
        bool canBreak;
        for (int i = 0; i < allPlayersData.Length; i++)
        {
            actionForEachPlayer.Invoke(i);
            if (breakCondition != null)
            {
                canBreak = breakCondition.Invoke();
                UnityEngine.Debug.Log("Current canBreak variable = " + canBreak);
                if (canBreak)
                {
                    UnityEngine.Debug.Log("Be breaking the loop at the player at index " + i);
                    break;
                }
            }
            UnityEngine.Debug.Log("Be continuing the loop at the player at index " + i);
        }
        UnityEngine.Debug.Log("The loop finished");
    }

    public int GetDicePoint(int playerIndex)
    {
        UnityEngine.Debug.Log("GetDicePoint-method runs from PlayerDataService");
        UnityEngine.Debug.Log("Be getting the dice point " + allPlayersData[playerIndex].currentDicePoint);
        return allPlayersData[playerIndex].currentDicePoint;
    }

    public void SetDicePoint(int playerIndex, int point)
    {
        UnityEngine.Debug.Log("SetDicePoint-method runs from PlayerDataService");
        allPlayersData[playerIndex].currentDicePoint = point;
        UnityEngine.Debug.Log("Set current dice point to " + point);
    }

    public int GetCurrentStaySpaceIndex(int playerIndex)
    {
        UnityEngine.Debug.Log("GetCurrentStaySpaceIndex-method runs from PlayerDataService");
        UnityEngine.Debug.Log($"Be getting space index that the player at {playerIndex} index is staying: {allPlayersData[playerIndex].currentStaySpaceIndex}");
        return allPlayersData[playerIndex].currentStaySpaceIndex;
    }

    public void SetCurrentStaySpaceIndex(int playerIndex, int spaceIndex)
    {
        UnityEngine.Debug.Log("SetCurrentStaySpaceIndex-method runs from PlayerDataService");
        allPlayersData[playerIndex].currentStaySpaceIndex = spaceIndex;
        UnityEngine.Debug.Log($"The player at index {playerIndex} are in the space at index {spaceIndex}");
    }

    public int GetCurrentCompanyCount(int playerIndex)
    {
        UnityEngine.Debug.Log("GetCurrentCompanyCount-method runs from PlayerDataService");
        UnityEngine.Debug.Log($"Be getting number of company of the player at index {playerIndex} : {allPlayersData[playerIndex].currentCompanyCount}");
        return allPlayersData[playerIndex].currentCompanyCount;
    }

    public int GetCurrentStationCount(int playerIndex)
    {
        UnityEngine.Debug.Log("GetCurrentStationCount-method runs from PlayerDataService");
        UnityEngine.Debug.Log($"Be getting number of station of the player at index {playerIndex} : {allPlayersData[playerIndex].currentStationCount}");
        return allPlayersData[playerIndex].currentStationCount;
    }

    public bool IsInJail(int playerIndex)
    {
        UnityEngine.Debug.Log("IsInJail-method runs from PlayerDataService");
        UnityEngine.Debug.Log($"Be checking if the player at index {playerIndex} is being in jail : {allPlayersData[playerIndex].isInJail}");
        return allPlayersData[playerIndex].isInJail;
    }

    public void BeInJail(int playerIndex)
    {
        UnityEngine.Debug.Log("BeInJail-method runs from PlayerDataService");
        allPlayersData[playerIndex].isInJail = true;
        UnityEngine.Debug.Log($"The player at index {playerIndex} is in jail currently");
    }

    public void QuitFromJail(int playerIndex)
    {
        UnityEngine.Debug.Log("QuitFromJail-method runs from PlayerDataService");
        allPlayersData[playerIndex].isInJail = false;
        UnityEngine.Debug.Log($"The player at index {playerIndex} quitted from jail");
    }

}