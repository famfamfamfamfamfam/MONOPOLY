using System.Collections.Generic;
using MessagePack;

[Union(0, typeof(PropertyData))]
[Union(1, typeof(StationData))]
[Union(2, typeof(CompanyData))]
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

[MessagePackObject]
public class PlayerData
{
    [Key(0)]
    public int currentCoin { get; set; }
    [Key(1)]
    public bool isInJail { get; set; }
    [Key(2)]
    public HashSet<IAsset> assets { get; }
    [Key(3)]
    public List<int> busTickets { get; }
    [Key(4)]
    public int currentDicePoint { get; set; }
    [Key(5)]
    public int currentCompanyCount { get; set; }
    [Key(6)]
    public int currentStationCount { get; set; }
    [Key(7)]
    public int currentStaySpaceIndex { get; set; }

    public PlayerData() { }

    public PlayerData(PlayerGeneralConfig config)
    {
        currentCoin = config.initialCoin;
        assets = new HashSet<IAsset>();
        busTickets = new List<int>();
    }
}

[MessagePackObject]
[Union(0, typeof(PropertyData))]
[Union(1, typeof(StationData))]
[Union(2, typeof(CompanyData))]
public abstract class AssetData
{
    [Key(0)]
    public int currentRentCost { get; set; }

    public AssetData() { }

    protected AssetData(int initialRentCost)
    {
        currentRentCost = initialRentCost;
    }
}
[MessagePackObject]
public class PropertyData : AssetData, IAsset
{
    public PropertyData() { }
    public PropertyData(PropertyConfig config) : base(config.rentCostAfterPurchase) { }

    [Key(1)]
    public int currentBuildingCount { get; set; }

    [Key(2)]
    public IAsset.AssetType type => IAsset.AssetType.Property;
}
[MessagePackObject]
public class StationData : AssetData, IAsset
{
    public StationData() { }

    public StationData(StationsConfig config) : base(config.eachInitialRentCost) { }
    
    [Key(1)]
    public IAsset.AssetType type => IAsset.AssetType.Station;
}
[MessagePackObject]
public class CompanyData : AssetData, IAsset
{
    public CompanyData() : base(default) { }

    [Key(1)]
    public IAsset.AssetType type => IAsset.AssetType.Company;
}

[MessagePackObject]
public class BoardData
{
    [Key(0)]
    public List<int> currentTakableBusTickets { get; }
    [Key(1)]
    public List<int> currentPurchasableSpaces { get; }

    public BoardData() { }

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

[MessagePackObject]
public class GameData
{
    [Key(0)]
    public int gamerPlayIndex { get; set; }
    [Key(1)]
    public int currentTurnPlayerIndex { get; set; }
}