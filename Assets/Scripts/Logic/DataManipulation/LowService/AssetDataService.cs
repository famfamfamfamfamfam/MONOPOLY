using System;
using UnityEngine;

public class AssetDataRepo
{
    AssetData[] assetsData;
    public AssetDataRepo(AssetData[] assetsData)
    {
        this.assetsData = assetsData;
    }
    public AssetData GetData(int index)
    {
        return assetsData[index];
    }
}

public class AssetRentCostAccessor
{
    AssetDataRepo assetDataRepo;
    public AssetRentCostAccessor(AssetDataRepo repo)
    {
        assetDataRepo = repo;
    }
    public int GetRentCost(int assetIndex)
    {
        return assetDataRepo.GetData(assetIndex).currentRentCost;
    }
}

public abstract class AssetDataService<T>
{
    protected readonly T config;
    protected AssetDataRepo assetDataRepo;
    protected AssetDataService(T config, AssetDataRepo repo = null)
    {
        this.config = config;
        assetDataRepo = repo;
    }

    protected void SetRentCost(AssetData data, int addValue)
    {
        data.currentRentCost += addValue;
    }

    public abstract void BePurchased(Action<int> payToPurchase);
}

public class PropertyDataService : AssetDataService<PropertyConfig>
{
    public PropertyDataService(PropertyConfig config, AssetDataRepo repo) : base(config, repo) { }

    public enum BuildType : byte
    {
        BuildNew,
        Upgrade
    }

    public void AddBuilding(int propertyIndex, BuildType type)
    {
        PropertyData data = assetDataRepo.GetData(propertyIndex) as PropertyData;
        if (data == null)
        {
            Debug.LogError("Not a property");
            return;
        }
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
    public CompanyDataService(CompaniesConfig config, AssetDataRepo repo) : base(config, repo) { }

    public void UpdateRentCost(int companyIndex, int dicePoint, int companyCount)
    {
        CompanyData data = assetDataRepo.GetData(companyIndex) as CompanyData;
        if (data == null)
        {
            Debug.LogError("Not a company");
            return;
        }
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