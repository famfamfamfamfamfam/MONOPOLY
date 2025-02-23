using System;
using UnityEngine;

public class AssetAccessor
{
    AssetData[] assetsData;
    public AssetAccessor(AssetData[] assetsData)
    {
        this.assetsData = assetsData;
    }
    public int GetRentCost(int assetIndex)
    {
        return assetsData[assetIndex].currentRentCost;
    }
    public IAsset GetAsset(int assetIndex)
    {
        return assetsData[assetIndex] as IAsset;
    }
    public T GetAsset<T>(int assetIndex) where T : AssetData
    {
        return assetsData[assetIndex] as T;
    }
    public IAssetDataService PickTheService(int assetIndex, PropertyDataService propertyService, StationDataService stationService, CompanyDataService companyService)
    {
        switch (assetsData[assetIndex])
        {
            case PropertyData property:
                return propertyService;
            case StationData station:
                return stationService;
            case CompanyData company:
                return companyService;
            default:
                Debug.LogAssertion("Not a valid asset");
                return null;
        }
    }
}
public interface IAssetDataService
{
    void BePurchased(Action<int> payToPurchase);
}
public abstract class AssetDataService<T> : IAssetDataService
{
    protected readonly T config;
    protected AssetData[] assetsData;
    protected AssetDataService(T config, AssetData[] assetsData = null)
    {
        this.config = config;
        this.assetsData = assetsData;
    }

    protected void SetRentCost(AssetData data, int addValue)
    {
        data.currentRentCost += addValue;
    }

    public abstract void BePurchased(Action<int> payToPurchase);
}

public class PropertyDataService : AssetDataService<PropertyConfig[]>
{
    int currentPropertyIndex;
    public PropertyDataService(PropertyConfig[] configs, AssetData[] assetsData) : base(configs, assetsData) { }

    public enum BuildType : byte
    {
        BuildNew,
        Upgrade
    }

    public void AddBuilding(int propertyIndex, BuildType type)
    {
        PropertyData data = assetsData[propertyIndex] as PropertyData;
        if (data == null)
        {
            Debug.LogAssertion("Not a property");
            return;
        }
        if (data.currentBuildingCount == config[propertyIndex].maxBuildingInSpace)
        {
            Debug.LogError("No build cause of reaching max");
            return;
        }
        data.currentBuildingCount++;
        switch (type)
        {
            case BuildType.BuildNew:
                SetRentCost(data, config[propertyIndex].rentCostIncreaseAfterBuild);
                return;
            case BuildType.Upgrade:
                data.currentBuildingCount -= config[propertyIndex].upgradeThreshold - 1;
                SetRentCost(data, config[propertyIndex].rentCostIncreaseAfterUpgrade);
                return;
        }
    }

    public void SetCurrentPropertyIndex(int propertyIndex)
    {
        currentPropertyIndex = propertyIndex;
    }
    public override void BePurchased(Action<int> payToPurchase)
    {
        payToPurchase.Invoke(config[currentPropertyIndex].purchaseCost);
    }
}

public class CompanyDataService : AssetDataService<CompaniesConfig>
{
    public CompanyDataService(CompaniesConfig config, AssetData[] assetsData) : base(config, assetsData) { }

    public void UpdateRentCost(int companyIndex, int dicePoint, int companyCount)
    {
        CompanyData data = assetsData[companyIndex] as CompanyData;
        if (data == null)
        {
            Debug.LogAssertion("Not a company");
            return;
        }
        if (companyCount == 0)
        {
            Debug.LogAssertion("Has no company");
            return;
        }
        if (companyCount > config.companyCount || companyCount < 0)
        {
            Debug.LogAssertion("Invalid number of company");
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

    public void IncreaseRentCost(StationData data)
    {
        if (data.currentRentCost == maxRentCost)
        {
            Debug.LogAssertion("Station rent cost has reached max");
            return;
        }
        SetRentCost(data, data.currentRentCost * (config.rentCostScaleFactor - 1));
    }

    public override void BePurchased(Action<int> payToPurchase)
    {
        payToPurchase.Invoke(config.eachPurchaseCost);
    }
}