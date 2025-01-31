using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    GlobalConfig gameConfig;
    [SerializeField]
    PlayerGeneralConfig playersConfig;
    [SerializeField]
    CompaniesConfig companiesConfig;
    [SerializeField]
    StationsConfig stationsConfig;
    [SerializeField]
    SpaceGroupConfig[] eventSpaceGroups;

    PlayerData[] playersData;
    AssetData[] assetsData;

    private void Start()
    {
        playersData = new PlayerData[gameConfig.playerCount];
        for (int i = 0; i < playersData.Length; i++)
        {
            playersData[i] = new PlayerData(playersConfig);
        }
        assetsData = new AssetData[gameConfig.purchasableSpaceCount];

    }

    public void TriggerSpace(PlayerData currentPlayer, int index)
    {
        foreach (var group in eventSpaceGroups)
        {
            if (group.spacesIndices.Contains(index))
            {
                return;
            }
        }
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
}