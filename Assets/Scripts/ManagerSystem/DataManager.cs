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
    SpaceConfig goSpace, gotoJailSpace, prisonVisitSpace, auctionSpace;

    HashSet<int> currentTakableBusTickets;
    List<AssetData> currentPurchasableSpaces;
    Action<IOnEvent>[] actionsOnEventSpaces;
    PlayerData[] playersData;
    AssetData[] assetsData;

    Action<Vector3, int> moveTo;
    Action rollThirdDieAndStep;

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
        goSpace = Resources.Load<SpaceConfig>(GlobalFieldContainer.goSpacePath);
        gotoJailSpace = Resources.Load<SpaceConfig>(GlobalFieldContainer.gotoJailSpacePath);
        prisonVisitSpace = Resources.Load<SpaceConfig>(GlobalFieldContainer.prisonVisitSpacePath);
        auctionSpace = Resources.Load<SpaceConfig>(GlobalFieldContainer.auctionSpacePath);
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
        actionsOnEventSpaces = new Action<IOnEvent>[]
        {
            _ => { },//Nontrigger
            data => TakeCard(data, communityCards, gameConfig.communityChestCard),//CommunityChest
            data => TakeCard(data, chanceCards, gameConfig.chanceCard),//Chance
            data => TakeCard(data, busTickets, gameConfig.busTicket, currentTakableBusTickets),//BusTicket
            data => GoToJail(data as ICanBeInJail),//GotoJail
            data => PayTaxes(data as IChangeCoin, taxConfig.cost),//Tax
            data => PayTaxes(data as IChangeCoin, surtaxConfig.cost),//Surtax
            data => TakeAsset(data as PlayerData, currentPurchasableSpaces[UnityEngine.Random.Range(0, currentPurchasableSpaces.Count)], false),//GiftReceive
            _ => { },//Auction
        };

        chanceCards.action.OnChangeToCommunityCard(actionsOnEventSpaces[(int)EventType.CommunityChest]);
        chanceCards.action.OnChangeToBusTicket(actionsOnEventSpaces[(int)EventType.BusTicket]);

        busTickets.action.OnGiveTicket((data, ticketIndex) => TakeBusTicket(data, ticketIndex));
        busTickets.action.BackToGoSpace(_ => moveTo(goSpace.position, goSpace.indexFromGoSpace));
        busTickets.action.GoToJail(actionsOnEventSpaces[(int)EventType.GotoJail]);
        busTickets.action.MoveToAUtilitySpace(_ => moveTo(GetRandomUtilitySpace(out int newPosition), newPosition));
        busTickets.action.RollThirdDieToMove(_ => rollThirdDieAndStep());
        busTickets.action.MoveToAuction(data =>
        {
            moveTo(auctionSpace.position, auctionSpace.indexFromGoSpace);
            actionsOnEventSpaces[(int)EventType.Auction].Invoke(data);
        });
        busTickets.action.QuitFromJail(data => QuitFromJail(data as ICanBeInJail));
    }

    void GoToJail(ICanBeInJail player)
    {
        player.BeInJail();
        moveTo(gotoJailSpace.position, gotoJailSpace.indexFromGoSpace);
    }
    void QuitFromJail(ICanBeInJail player)
    {
        player.QuitFromJail();
        moveTo(prisonVisitSpace.position, prisonVisitSpace.indexFromGoSpace);
    }
    Vector3 GetRandomUtilitySpace(out int positionIndex)
    {
        int pick = UnityEngine.Random.Range(0, 2);
        if (pick == 0)
        {
            positionIndex = GetPositionIndex(companiesConfig, out Vector3 position);
            return position;
        }
        else
        {
            positionIndex = GetPositionIndex(stationsConfig, out Vector3 position);
            return position;
        }
    }
    int GetPositionIndex(SpaceGroupConfig spaceGroup, out Vector3 position)
    {
        int result = UnityEngine.Random.Range(0, companiesConfig.companyCount);
        position = spaceGroup.spaces[result].position;
        return spaceGroup.spaces[result].indexFromGoSpace;
    }

    void PayTaxes(IChangeCoin player, int cost)
    {
        player.SetCurrentCoin(-cost);
    }

    void TakeCard(IOnEvent data, CardsConfig cards, int maxRandomValue, HashSet<int> validIndices = null)
    {
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, maxRandomValue);
        }
        while (validIndices != null && !validIndices.Contains(index));
        cards.AccessTheCard(data, playersData, index);
    }

    void TakeBusTicket(ICanKeepTicket player, int ticketIndex)
    {
        currentTakableBusTickets.Remove(ticketIndex);
        player.KeepTicket(ticketIndex);
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
            actionsOnEventSpaces[(int)theEvent](playersData[playerIndex]);
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
        asset.UpdateRentCost(lessee.GetDicePoint(), lessor.GetCurrentCompanyCount());
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
            asset.BePurchased(cost => player.SetCurrentCoin(-cost));
        }
        if (asset is CompanyData)
        {
            player.AddAsset(asset, AssetType.Company);
        }
        else if (asset is StationData)
        {
            player.AddAsset(asset, AssetType.Station);
        }
        else
        {
            player.AddAsset(asset, AssetType.Property);
        }
        currentPurchasableSpaces.Remove(asset);
    }

    public void PassGoSpace(int playerIndex)
    {
        playersData[playerIndex].SetCurrentCoin(gameConfig.passGoSpaceBonus);
    }

    public void UseBusTicket(int playerIndex, int ticketIndex)
    {
        busTickets.action.TriggerKeepToUseTicket(playersData[playerIndex], ticketIndex);
        playersData[playerIndex].GiveBackTicket(ticketIndex);
        currentTakableBusTickets.Add(ticketIndex);
    }

    public void OnEventMove(Action<Vector3, int> move)
    {
        moveTo = move;
    }
    public void OnRollThirdDieAndStep(Action rollAndStep)
    {
        rollThirdDieAndStep = rollAndStep;
    }
}