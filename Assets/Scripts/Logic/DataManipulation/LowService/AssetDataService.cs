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
        Debug.Log("GetRentCost-method runs from AssetAccessor");
        return assetsData[assetIndex].currentRentCost;
    }
    public IAsset GetAsset(int assetIndex)
    {
        Debug.Log("GetAsset(interface)-method runs from AssetAccessor");
        return assetsData[assetIndex];
    }
    public T GetAsset<T>(int assetIndex) where T : AssetData
    {
        Debug.Log("GetAsset(generic type constrained to AssetData)-method runs from AssetAccessor");
        Debug.Log("Be returning : " + typeof(T));
        return assetsData[assetIndex] as T;
    }
    public IAssetDataService PickTheService(int assetIndex, PropertyDataService propertyService, StationDataService stationService, CompanyDataService companyService)
    {
        Debug.Log("PickTheService-method runs from AssetAccessor");
        switch (assetsData[assetIndex])
        {
            case PropertyData property:
                Debug.Log("Be picking Property");
                return propertyService;
            case StationData station:
                Debug.Log("Be picking Station");
                return stationService;
            case CompanyData company:
                Debug.Log("Be picking Company");
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
        Debug.Log("SetRentCost-method runs from AssetDataService");
        data.currentRentCost += addValue;
        Debug.Log("Rent cost after change : " + data.currentRentCost);
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

    public void AddBuilding(int propertyIndex, BuildType buildType, BuildingRate material = BuildingRate.None, BuildingRate expectOutcome = BuildingRate.None)
    {
        Debug.Log("AddBuilding-method runs from PropertyDataService");
        PropertyData data = assetsData[propertyIndex] as PropertyData;
        if (data == null)
        {
            Debug.LogAssertion("Not a property");
            return;
        }

        switch (buildType)
        {
            case BuildType.BuildNew:
                BuildNew(data, propertyIndex);
                return;
            case BuildType.Upgrade:
                Upgrade(data, propertyIndex, material, expectOutcome);
                return;
        }
    }
    void BuildNew(PropertyData data, int propertyIndex)
    {
        if (GetTotalBuildingCount(data) == config[propertyIndex].maxBuildingInSpace)
        {
            Debug.LogWarning("No build cause of reaching max");
            return;
        }
        data.currentBuildingCount[(byte)BuildingRate.D]++;
        SetRentCost(data, config[propertyIndex].rentCostIncreaseAfterBuild);
        Debug.Log("Updated property rent cost : increase " + config[propertyIndex].rentCostIncreaseAfterBuild);
        Debug.Log($"There are {data.currentBuildingCount} in property currently");
    }
    void Upgrade(PropertyData data, int propertyIndex, BuildingRate material, BuildingRate outcome)
    {
        if (material == BuildingRate.None || outcome == BuildingRate.None || material > outcome)
        {
            Debug.LogError("Invalid BuildingRate of material buildings or expected building");
            return;
        }
        if (data.currentBuildingCount[(byte)material] < config[propertyIndex].upgradeThreshold)
        {
            Debug.LogWarning("No upgrade cause of lack of materials");
            return;
        }
        data.currentBuildingCount[(byte)material] -= config[propertyIndex].upgradeThreshold;
        data.currentBuildingCount[(byte)outcome]++;
        SetRentCost(data, config[propertyIndex].rentCostIncreaseAfterUpgrade);
        Debug.Log("Updated property rent cost : increase " + config[propertyIndex].rentCostIncreaseAfterUpgrade);
        Debug.Log($"There are {data.currentBuildingCount} in property currently");
    }

    int GetTotalBuildingCount(PropertyData data)
    {
        int sum = 0;
        for (int i = 0; i < data.currentBuildingCount.Length; i++)
        {
            sum += data.currentBuildingCount[i];
        }
        return sum;
    }

    public int GetCost(int propertyIndex, BuildType buildType)
    {
        switch (buildType)
        {
            case BuildType.BuildNew:
                return config[propertyIndex].buildCost;
            case BuildType.Upgrade:
                return config[propertyIndex].upgradeCost;
            default:
                Debug.LogError("Cannot get cost cause invalid build");
                return 0;
        }
    }

    public void SetCurrentPropertyIndex(int propertyIndex)
    {
        Debug.Log("SetCurrentPropertyIndex-method runs from PropertyDataService");
        Debug.Log("Current space index to assign : " + propertyIndex);
        currentPropertyIndex = propertyIndex;
    }
    public override void BePurchased(Action<int> payToPurchase)
    {
        Debug.Log("BePurchased-method runs from PropertyDataService");
        payToPurchase.Invoke(config[currentPropertyIndex].purchaseCost);
        Debug.Log("Purchased with " + config[currentPropertyIndex].purchaseCost);
    }
}

public class CompanyDataService : AssetDataService<CompaniesConfig>
{
    public CompanyDataService(CompaniesConfig config, AssetData[] assetsData) : base(config, assetsData) { }

    public void UpdateRentCost(int companyIndex, int dicePoint, int companyCount)
    {
        Debug.Log("UpdateRentCost-method runs from CompanyDataService");
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
        data.currentRentCost = 0;
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
        Debug.Log("BePurchased-method runs from CompanyDataService");
        payToPurchase.Invoke(config.eachPurchaseCost);
        Debug.Log("Purchased with " + config.eachPurchaseCost);
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
        Debug.Log("IncreaseRentCost-method runs from StationDataService");
        if (data.currentRentCost == maxRentCost)
        {
            Debug.LogAssertion("Station rent cost has reached max");
            return;
        }
        SetRentCost(data, data.currentRentCost * (config.rentCostScaleFactor - 1));
    }

    public override void BePurchased(Action<int> payToPurchase)
    {
        Debug.Log("BePurchased-method runs from StationDataService");
        payToPurchase.Invoke(config.eachPurchaseCost);
        Debug.Log("Purchased with " + config.eachPurchaseCost);
    }
}