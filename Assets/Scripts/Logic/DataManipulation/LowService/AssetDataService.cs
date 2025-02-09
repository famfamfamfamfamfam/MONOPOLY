using System;
using UnityEngine;

public abstract class AssetDataService<T>
{
    protected readonly T config;
    protected AssetDataService(T config)
    {
        this.config = config;
    }

    public int GetRentCost(AssetData data)
    {
        return data.currentRentCost;
    }
    protected void SetRentCost(AssetData data, int addValue)
    {
        data.currentRentCost += addValue;
    }

    public abstract void BePurchased(Action<int> payToPurchase);
}

public class PropertyDataService : AssetDataService<PropertyConfig>
{
    public PropertyDataService(PropertyConfig config) : base(config) { }

    public enum BuildType : byte
    {
        BuildNew,
        Upgrade
    }

    public void AddBuilding(PropertyData data, BuildType type)
    {
        if (data.currentBuildingCount == config.maxBuildingInSpace)
        {
            Debug.LogError("No build cause of reaching max");
            return;
        }
        data.currentBuildingCount++;
        switch (type)
        {
            case BuildType.BuildNew:
                SetRentCost(data, config.rentCostIncreaseAfterBuild);
                return;
            case BuildType.Upgrade:
                data.currentBuildingCount -= config.upgradeThreshold - 1;
                SetRentCost(data, config.rentCostIncreaseAfterUpgrade);
                return;
        }
    }

    public override void BePurchased(Action<int> payToPurchase)
    {
        payToPurchase.Invoke(config.purchaseCost);
    }
}

public class CompanyDataService : AssetDataService<CompaniesConfig>
{
    public CompanyDataService(CompaniesConfig config) : base(config) { }

    public void UpdateRentCost(CompanyData data, int dicePoint, int companyCount)
    {
        if (companyCount == 0)
        {
            Debug.LogError("Has no company");
            return;
        }
        if (companyCount > config.companyCount || companyCount < 0)
        {
            Debug.LogError("Invalid number of company");
            return;
        }
        if (companyCount == 1)
        {
            SetRentCost(data, dicePoint * config.dicePointMultiplierIfHasOne);
        }
        else
        {
            SetRentCost(data, dicePoint * config.dicePointMultiplierIfHasTwo);
        }
    }

    public override void BePurchased(Action<int> payToPurchase)
    {
        payToPurchase.Invoke(config.eachPurchaseCost);
    }
}

public class StationDataService : AssetDataService<StationsConfig>
{
    readonly int maxRentCost;
    public StationDataService(StationsConfig config) : base(config)
    {
        maxRentCost = (int)Mathf.Pow(config.rentCostScaleFactor, config.stationCount - 1) * config.eachInitialRentCost;
    }

    public void IncreaseRentCost(StationData data, out bool hasIncreased)
    {
        if (data.currentRentCost == maxRentCost)
        {
            Debug.LogError("Station rent cost has reached max");
            hasIncreased = false;
            return;
        }
        SetRentCost(data, data.currentRentCost * (config.rentCostScaleFactor - 1));
        hasIncreased = true;
    }

    public override void BePurchased(Action<int> payToPurchase)
    {
        payToPurchase.Invoke(config.eachPurchaseCost);
    }
}