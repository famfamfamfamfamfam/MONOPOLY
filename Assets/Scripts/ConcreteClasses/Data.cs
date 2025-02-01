using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    int currentCoin;
    HashSet<AssetData> assets;
    List<int> busTickets;
    int currentDicePoint;
    public int currentCompanyCount { get; private set; }
    public int currentStationCount { get; private set; }
    public PlayerData(PlayerGeneralConfig config)
    {
        currentCoin = config.initialCoin;
        assets = new HashSet<AssetData>();
        busTickets = new List<int>();
    }

    public void AddAsset(AssetData space)
    {
        assets.Add(space);
        switch (space)
        {
            case CompanyData:
                currentCompanyCount++;
                return;
            case StationData station:
                currentStationCount++;
                if (currentStationCount > 1)
                {
                    station.IncreaseRentCost(out bool hasIncreased);
                    if (!hasIncreased)
                    {
                        currentStationCount--;
                    }
                }
                return;
        }
    }

    public void KeepTicket(int ticket)
    {
        busTickets.Add(ticket);
    }

    public void GiveBackTicket(int index)
    {
        busTickets.Remove(index);
    }

    public bool IsOwner(AssetData space)
    {
        return assets.Contains(space);
    }

    public void SetCurrentCoin(int addValue)
    {
        currentCoin += addValue;
    }

    public int GetDicePoint()
    {
        return currentDicePoint;
    }

    public void SetDicePoint(int point)
    {
        currentDicePoint = point;
    }
}

public class AssetData
{
    protected int currentRentCost;
    public int GetRentCost()
    {
        return currentRentCost;
    }
    protected void SetRentCost(int addValue)
    {
        currentRentCost += addValue;
    }
}

public class PropertyData : AssetData
{
    PropertyConfig config;
    int currentBuildingCount;

    public PropertyData(PropertyConfig instance)
    {
        currentRentCost = instance.rentCostAfterPurchase;
        config = instance;
    }

    public void AddBuilding(BuildType type)
    {
        if (currentBuildingCount == config.maxBuildingInSpace)
        {
            Debug.LogError("No build cause of reaching max");
            return;
        }
        currentBuildingCount++;
        switch (type)
        {
            case BuildType.BuildNew:
                SetRentCost(config.rentCostIncreaseAfterBuild);
                return;
            case BuildType.Upgrade:
                currentBuildingCount -= config.upgradeThreshold - 1;
                SetRentCost(config.rentCostIncreaseAfterUpgrade);
                return;
        }
    }
}

public class CompanyData : AssetData
{
    CompaniesConfig config;
    public CompanyData(CompaniesConfig instance)
    {
        config = instance;
    }

    public void UpdateRentCost(int dicePoint, int companyCount)
    {
        if (companyCount > config.companyCount)
        {
            Debug.LogError("Invalid number of company");
            return;
        }
        if (companyCount > 1)
        {
            SetRentCost(dicePoint * config.dicePointMultiplierIfHasTwo);
            return;
        }
        SetRentCost(dicePoint * config.dicePointMultiplierIfHasOne);
    }
}

public class StationData : AssetData
{
    int rentCostScaleFactor;
    int maxRentCost;
    public StationData(StationsConfig config)
    {
        maxRentCost = (int)Mathf.Pow(config.rentCostScaleFactor, config.stationCount - 1) * config.eachInitialRentCost;
        rentCostScaleFactor = config.rentCostScaleFactor;
        currentRentCost = config.eachInitialRentCost;
    }

    public void IncreaseRentCost(out bool hasIncreased)
    {
        if (currentRentCost == maxRentCost)
        {
            Debug.LogError("Station rent cost has reached max");
            hasIncreased = false;
            return;
        }
        SetRentCost(currentRentCost * (rentCostScaleFactor - 1));
        hasIncreased = true;
    }
}