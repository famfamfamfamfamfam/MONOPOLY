using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("CONFIGS")]
    [Header("General configs")]
    [SerializeField]
    GlobalConfig gameConfig;
    [SerializeField]
    PlayerGeneralConfig playersConfig;
    [Header("Utilities spaces")]
    [SerializeField]
    CompaniesConfig companiesConfig;
    [SerializeField]
    StationsConfig stationsConfig;
    [Header("Event spaces")]
    [SerializeField]
    SpaceGroupConfig triggerNothingSpaces;
    [SerializeField]
    SpaceGroupConfig communityChestSpaces;
    [SerializeField]
    SpaceGroupConfig chanceSpaces;
    [SerializeField]
    SpaceConfig busSpace;
    [SerializeField]
    SpaceGroupConfig taxSpaces;
    [SerializeField]
    SpaceConfig auctionSpace;
    [SerializeField]
    SpaceGroupConfig gotoJailSpace;
    [SerializeField]
    SpaceConfig giftSpace;

    PlayerData[] playersData;
    AssetData[] assetsData;

    readonly string purchasableSpacesPath = "ScriptableObjects/PurchasableSpaces/Spaces";

    private void Start()
    {
        InitPlayersData();
        InitAssetsData();
    }

    void InitPlayersData()
    {
        playersData = new PlayerData[gameConfig.playerCount];
        for (int i = 0; i < playersData.Length; i++)
        {
            playersData[i] = new PlayerData(playersConfig);
        }
    }

    void InitAssetsData()
    {
        assetsData = new AssetData[gameConfig.spaceCount];
        SpaceConfig[] purchasableSpaces = Resources.LoadAll<SpaceConfig>(purchasableSpacesPath);
        foreach (var space in purchasableSpaces)
        {
            if (companiesConfig.spacesIndices.Contains(space.indexFromGoSpace))
            {
                assetsData[space.indexFromGoSpace] = new CompanyData(companiesConfig);
            }
            else if (stationsConfig.spacesIndices.Contains(space.indexFromGoSpace))
            {
                assetsData[space.indexFromGoSpace] = new StationData(stationsConfig);
            }
            else
            {
                assetsData[space.indexFromGoSpace] = new PropertyData((PropertyConfig)space);
            }
        }
    }

    public void TriggerSpace(PlayerData currentPlayer, int index)
    {
        //
        TriggerPurchasableSpace(currentPlayer, index);
    }

    void TriggerPurchasableSpace(PlayerData currentPlayer, int index)
    {
        bool isPurchased = true;
        foreach (var player in playersData)
        {
            if (player.IsOwner(assetsData[index]))
            {
                if (player == currentPlayer)
                {
                    break;
                }
                if (companiesConfig.spacesIndices.Contains(index))
                {
                    CompanyCost(player, currentPlayer, (CompanyData)assetsData[index]);
                }
                else
                {
                    Cost(player, currentPlayer, assetsData[index]);
                }
                break;
            }
            isPurchased = false;
        }
        if (!isPurchased)
        {

        }
    }

    void CompanyCost(PlayerData lessor, PlayerData lessee, CompanyData asset)
    {
        asset.UpdateRentCost(lessee.GetDicePoint(), lessor.currentCompanyCount);
        Cost(lessor, lessee, asset);
    }

    void Cost(PlayerData lessor, PlayerData lessee, AssetData asset)
    {
        lessee.SetCurrentCoin(-asset.GetRentCost());
        lessor.SetCurrentCoin(asset.GetRentCost());
    }

    public void PassGoSpace(PlayerData data)
    {
        data.SetCurrentCoin(gameConfig.passGoSpaceBonus);
    }
}