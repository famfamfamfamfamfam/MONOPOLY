using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : IChangeCoin, ICanKeepTicket, ICanBeInJail
{
    int currentCoin;
    bool isInJail;
    HashSet<IAsset> assets;
    List<int> busTickets;
    int currentDicePoint;
    int currentCompanyCount;
    int currentStationCount;
    public PlayerData(PlayerGeneralConfig config)
    {
        currentCoin = config.initialCoin;
        assets = new HashSet<IAsset>();
        busTickets = new List<int>();
    }

    public void AddAsset(IAsset space, AssetType type)
    {
        assets.Add(space);
        switch (type)
        {
            case AssetType.Company:
                space.UpdateTheNumber(ref currentCompanyCount);
                return;
            case AssetType.Station:
                space.UpdateTheNumber(ref currentStationCount);
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

    public bool IsOwner(IAsset space)
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

    public int GetCurrentCompanyCount()
    {
        return currentCompanyCount;
    }

    public int GetCurrentStationCount()
    {
        return currentStationCount;
    }

    public bool IsInJail()
    {
        return isInJail;
    }

    public void BeInJail()
    {
        isInJail = true;
    }

    public void QuitFromJail()
    {
        isInJail = false;
    }
}

public class AssetData : IAsset
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
    public virtual void BePurchased(Action<int> onPurchase) { }

    public virtual void UpdateTheNumber(ref int currentCount)
    {
        currentCount++;
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

    public override void BePurchased(Action<int> onPurchase)
    {
        onPurchase.Invoke(config.purchaseCost);
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

    public override void BePurchased(Action<int> onPurchase)
    {
        onPurchase.Invoke(config.eachPurchaseCost);
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
    StationsConfig config;
    int maxRentCost;
    public StationData(StationsConfig instance)
    {
        maxRentCost = (int)Mathf.Pow(instance.rentCostScaleFactor, instance.stationCount - 1) * instance.eachInitialRentCost;
        currentRentCost = instance.eachInitialRentCost;
        config = instance;
    }

    public override void BePurchased(Action<int> onPurchase)
    {
        onPurchase.Invoke(config.eachPurchaseCost);
    }

    void IncreaseRentCost(out bool hasIncreased)
    {
        if (currentRentCost == maxRentCost)
        {
            Debug.LogError("Station rent cost has reached max");
            hasIncreased = false;
            return;
        }
        SetRentCost(currentRentCost * (config.rentCostScaleFactor - 1));
        hasIncreased = true;
    }

    public override void UpdateTheNumber(ref int currentCount)
    {
        base.UpdateTheNumber(ref currentCount);
        if (currentCount > 1)
        {
            IncreaseRentCost(out bool hasIncreased);
            if (!hasIncreased)
            {
                currentCount--;
            }
        }
    }
}