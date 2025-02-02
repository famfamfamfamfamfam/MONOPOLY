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
    ChancesConfig chanceCards;
    BusTicketsConfig busTickets;
    TaxConfig taxConfig;
    TaxConfig surtaxConfig;

    HashSet<int> currentTakableBusTickets;
    List<AssetData> currentPurchasableSpaces;
    Action<PlayerData>[] triggerActionsOnEventSpaces;
    PlayerData[] playersData;
    AssetData[] assetsData;

    private void Start()
    {
        InitGeneralConfigs();
        InitPurchasableSpaceGroups();
        InitEventSpaces();
        InitEventCards();
        InitActionsOnEventSpaces();
        InitPlayersData();
        InitAssetsData();
    }

    void InitGeneralConfigs()
    {
        gameConfig = Resources.Load<GlobalConfig>(GlobalFieldContainer.globalConfigPath);
        playersConfig = Resources.Load<PlayerGeneralConfig>(GlobalFieldContainer.playerGeneralConfigPath);
    }
    void InitPurchasableSpaceGroups()
    {
        companiesConfig = Resources.Load<CompaniesConfig>(GlobalFieldContainer.companiesGroupPath);
        stationsConfig = Resources.Load<StationsConfig>(GlobalFieldContainer.stationsGroupPath);
    }
    void InitEventSpaces()
    {
        eventSpaceGroup = Resources.Load<SpaceGroupConfig>(GlobalFieldContainer.eventSpaceGroupPath);
        taxConfig = Resources.Load<TaxConfig>(GlobalFieldContainer.taxSpacePath);
        surtaxConfig = Resources.Load<TaxConfig>(GlobalFieldContainer.surtaxSpacePath);
    }
    void InitEventCards()
    {
        communityCards = Resources.Load<CommunityChestsConfig>(GlobalFieldContainer.communityChestCardsPath);
        chanceCards = Resources.Load<ChancesConfig>(GlobalFieldContainer.chanceCardsPath);
        busTickets = Resources.Load<BusTicketsConfig>(GlobalFieldContainer.busTicketsPath);

        int[] tempArray = new int[gameConfig.busTicket];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = i;
        }
        currentTakableBusTickets = new HashSet<int>(tempArray);
    }
    void InitActionsOnEventSpaces()
    {
        triggerActionsOnEventSpaces = new Action<PlayerData>[]
        {
            _ => { },//Nontrigger
            data => TakeCard(data, communityCards, gameConfig.communityChestCard),//CommunityChest
            data => TakeCard(data, chanceCards, gameConfig.chanceCard),//Chance
            data => TakeCard(data, busTickets, gameConfig.busTicket, currentTakableBusTickets),//BusTicket
            _ => { },//GotoJail
            data => data.SetCurrentCoin(taxConfig.cost),//Tax
            data => data.SetCurrentCoin(surtaxConfig.cost),//Surtax
            data => TakeAsset(data, currentPurchasableSpaces[UnityEngine.Random.Range(0, currentPurchasableSpaces.Count)], false),//GiftReceive
            _ => { },//Auction
        };

        chanceCards.action.OnChangeToCommunityCard(triggerActionsOnEventSpaces[(int)EventType.CommunityChest]);
        chanceCards.action.OnChangeToBusTicket(triggerActionsOnEventSpaces[(int)EventType.BusTicket]);

        busTickets.action.OnGiveTicket((data, ticketIndex) => TakeBusTicket(data, ticketIndex));
        
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
        currentPurchasableSpaces = new List<AssetData>();
        SpaceConfig[] purchasableSpaces = Resources.LoadAll<SpaceConfig>(GlobalFieldContainer.purchasableSpacesPath);
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
            currentPurchasableSpaces.Add(assetsData[space.indexFromGoSpace]);
        }
    }

    public void TriggerSpace(int playerIndex, int positionIndex)
    {
        if (eventSpaceGroup.eventDictionary.TryGetValue(positionIndex, out EventType theEvent))
        {
            triggerActionsOnEventSpaces[(int)theEvent](playersData[playerIndex]);
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
            TakeAsset(currentPlayer, assetsData[positionIndex], true);
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

    void TakeAsset(PlayerData player, AssetData asset, bool byPurchasing)
    {
        if (byPurchasing)
        {
            asset.BePurchased(player);
        }
        player.AddAsset(asset);
        currentPurchasableSpaces.Remove(asset);
    }

    public void PassGoSpace(int playerIndex)
    {
        playersData[playerIndex].SetCurrentCoin(gameConfig.passGoSpaceBonus);
    }

    void TakeCard(PlayerData data, CardsConfig cards, int maxRandomValue, HashSet<int> validIndices = null)
    {
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, maxRandomValue);
        }
        while (validIndices != null && !validIndices.Contains(index));
        cards.AccessTheCard(data, playersData, index);
    }

    void TakeBusTicket(PlayerData player, int ticketIndex)
    {
        currentTakableBusTickets.Remove(ticketIndex);
        player.KeepTicket(ticketIndex);
    }

    public void UseBusTicket(int playerIndex, int ticketIndex)
    {
        busTickets.action.TriggerKeepToUseTicket(ticketIndex);
        playersData[playerIndex].GiveBackTicket(ticketIndex);
        currentTakableBusTickets.Add(ticketIndex);
    }
}