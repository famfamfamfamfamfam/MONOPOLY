using System;

public class PlayerDataService
{
    public void AddAsset(PlayerData data, IAsset asset, Action stationCostUpdate)
    {
        data.assets.Add(asset);
        switch (asset.type)
        {
            case IAsset.AssetType.Station:
                OnAddAStation(data, stationCostUpdate);
                return;
            case IAsset.AssetType.Company:
                OnAddACompany(data);
                return;
        }
    }

    void OnAddAStation(PlayerData data, Action stationCostUpdate)
    {
        if (data.currentStationCount == 0)
        {
            data.currentStationCount++;
            return;
        }
        stationCostUpdate.Invoke();
    }

    void OnAddACompany(PlayerData data)
    {
        data.currentCompanyCount++;
    }

    public void KeepTicket(PlayerData data, int ticket)
    {
        data.busTickets.Add(ticket);
    }

    public void GiveBackTicket(PlayerData data, int ticket)
    {
        data.busTickets.Remove(ticket);
    }

    public bool IsOwner(PlayerData data, IAsset asset)
    {
        return data.assets.Contains(asset);
    }

    public void SetCurrentCoin(PlayerData data, int addValue)
    {
        data.currentCoin += addValue;
    }

    public int GetDicePoint(PlayerData data)
    {
        return data.currentDicePoint;
    }

    public void SetDicePoint(PlayerData data, int point)
    {
        data.currentDicePoint = point;
    }

    public int GetCurrentCompanyCount(PlayerData data)
    {
        return data.currentCompanyCount;
    }

    public int GetCurrentStationCount(PlayerData data)
    {
        return data.currentStationCount;
    }

    public bool IsInJail(PlayerData data)
    {
        return data.isInJail;
    }

    public void BeInJail(PlayerData data)
    {
        data.isInJail = true;
    }

    public void QuitFromJail(PlayerData data)
    {
        data.isInJail = false;
    }

}