using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    GlobalConfig gameConfig;
    PlayerGeneralConfig playersConfig;
    CompaniesConfig companiesConfig;
    StationsConfig stationsConfig;
    SpaceGroupConfig eventSpaceGroup;
    CommunityChestsConfig communityCards;

    Dictionary<EventType, Action<PlayerData>> triggerActionsOnEventSpaces;
    PlayerData[] playersData;
    AssetData[] assetsData;

    readonly string generalConfigsPath = "ScriptableObjects";
    readonly string purchasableSpaceGroupsPath = "ScriptableObjects/PurchasableSpaces/Groups";
    readonly string purchasableSpacesPath = "ScriptableObjects/PurchasableSpaces/Spaces";
    readonly string eventSpaceGroupPath = "ScriptableObjects/EventSpaces/EventSpaces";

    private void Start()
    {
        InitGeneralConfigs();
        InitPurchasableSpaceGroups();
        InitEventSpaceGroup();
        InitPlayersData();
        InitAssetsData();
    }

    void InitGeneralConfigs()
    {
        gameConfig = Resources.LoadAll<GlobalConfig>(generalConfigsPath)[0];
        playersConfig = Resources.LoadAll<PlayerGeneralConfig>(generalConfigsPath)[0];
    }

    void InitPurchasableSpaceGroups()
    {
        companiesConfig = Resources.LoadAll<CompaniesConfig>(purchasableSpaceGroupsPath)[0];
        stationsConfig = Resources.LoadAll<StationsConfig>(purchasableSpaceGroupsPath)[0];
    }

    void InitEventSpaceGroup()
    {
        eventSpaceGroup = Resources.Load<SpaceGroupConfig>(eventSpaceGroupPath);
    }

    void InitActionsOnEventSpaces()
    {
        triggerActionsOnEventSpaces = new Dictionary<EventType, Action<PlayerData>>()
        {
            { EventType.Nontrigger, _ => { } },
            { EventType.CommunityChest, data => TakeCard(communityCards) },
            { EventType.Chance, data => { } },
            { EventType.BusTicket, data => { } },
            { EventType.GotoJail, data => { } },
            { EventType.Tax, data => { } },
            { EventType.Surtax, data => { } },
            { EventType.GiftReceive, data => { } },
            { EventType.Auction, data => { } },
        };
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

    public void TriggerSpace(int playerIndex, int positionIndex)
    {
        if (eventSpaceGroup.eventDictionary.ContainsKey(positionIndex))
        {

            return;
        }
        TriggerPurchasableSpace(playersData[playerIndex], positionIndex);
    }

    void TriggerPurchasableSpace(PlayerData currentPlayer, int positionIndex)
    {
        bool isPurchased = true;
        foreach (var player in playersData)
        {
            if (player.IsOwner(assetsData[positionIndex]))
            {
                if (player == currentPlayer)
                {
                    break;
                }
                if (companiesConfig.spacesIndices.Contains(positionIndex))
                {
                    CompanyCost(player, currentPlayer, (CompanyData)assetsData[positionIndex]);
                }
                else
                {
                    Cost(player, currentPlayer, assetsData[positionIndex]);
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

    void TakeCard(CardsConfig cards)
    {

    }
}