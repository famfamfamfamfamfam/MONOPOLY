using System.Collections.Generic;

public interface IAsset
{
    public enum AssetType : byte
    {
        Property,
        Company,
        Station
    }
    AssetType type { get; }
}

public class PlayerData
{
    public int currentCoin { get; set; }
    public bool isInJail { get; set; }
    public HashSet<IAsset> assets { get; }
    public List<int> busTickets { get; }
    public int currentDicePoint { get; set; }
    public int currentCompanyCount { get; set; }
    public int currentStationCount { get; set; }

    public PlayerData(PlayerGeneralConfig config)
    {
        currentCoin = config.initialCoin;
        assets = new HashSet<IAsset>();
        busTickets = new List<int>();
    }
}

public abstract class AssetData
{
    public int currentRentCost { get; set; }

    protected AssetData(int initialRentCost)
    {
        currentRentCost = initialRentCost;
    }
}
public class PropertyData : AssetData, IAsset
{
    public PropertyData(PropertyConfig config) : base(config.rentCostAfterPurchase) { }

    public int currentBuildingCount { get; set; }

    public IAsset.AssetType type => IAsset.AssetType.Property;
}
public class StationData : AssetData, IAsset
{
    public StationData(StationsConfig config) : base(config.eachInitialRentCost) { }

    public IAsset.AssetType type => IAsset.AssetType.Station;
}
public class CompanyData : AssetData, IAsset
{
    public CompanyData() : base(default) { }

    public IAsset.AssetType type => IAsset.AssetType.Company;
}

public class BoardData
{
    public List<int> currentTakableBusTickets { get; }
    public List<int> currentPurchasableSpaces { get; }

    public BoardData(GlobalConfig config, SpaceGroupConfig eventSpaces, BusTicketsConfig bus)
    {
        currentTakableBusTickets = new List<int>();
        for (int i = 0; i < bus.instantUseTickets.Length; i++)
        {
            currentTakableBusTickets.Add((int)bus.instantUseTickets[i]);
        }
        for (int i = 0; i < bus.keepToUseTickets.Length; i++)
        {
            currentTakableBusTickets.Add((int)bus.keepToUseTickets[i]);
        }

        currentPurchasableSpaces = new List<int>();
        for (int i = 0; i < config.spaceCount; i++)
        {
            if (!eventSpaces.spacesIndices.Contains(i))
            {
                currentPurchasableSpaces.Add(i);
            }
        }
    }
}