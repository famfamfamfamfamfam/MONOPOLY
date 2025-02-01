using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    //public static DataManager instance;
    //bool canRelease = true;

    GlobalConfig gameConfig;
    PlayerGeneralConfig playersConfig;
    CompaniesConfig companiesConfig;
    StationsConfig stationsConfig;
    SpaceGroupConfig eventSpaceGroup;
    CommunityChestsConfig communityCards;
    ChancesConfig chanceCards;
    BusTicketsConfig busTickets;

    HashSet<int> currentBusTickets;
    Dictionary<EventType, Action<PlayerData>> triggerActionsOnEventSpaces;
    PlayerData[] playersData;
    AssetData[] assetsData;

    readonly string globalConfigPath = "ScriptableObjects/GlobalSetting";
    readonly string playerGeneralConfigPath = "ScriptableObjects/GeneralPlayer";
    readonly string companiesGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Companies";
    readonly string stationsGroupPath = "ScriptableObjects/PurchasableSpaces/Groups/Stations";
    readonly string purchasableSpacesPath = "ScriptableObjects/PurchasableSpaces/Spaces";
    readonly string eventSpaceGroupPath = "ScriptableObjects/EventSpaces/EventSpaces";
    readonly string communityChestCardsPath = "ScriptableObjects/Cards/CommunityChests";
    readonly string chanceCardsPath = "ScriptableObjects/Cards/Chances";
    readonly string busTicketsPath = "ScriptableObjects/Cards/BusTickets";

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        canRelease = false;
    //        Destroy(gameObject);
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (canRelease)
    //    {
    //        instance = null;
    //    }
    //}

    private void Start()
    {
        InitGeneralConfigs();
        InitPurchasableSpaceGroups();
        InitEventSpaceGroup();
        InitEventCards();
        InitActionsOnEventSpaces();
        InitPlayersData();
        InitAssetsData();
    }

    void InitGeneralConfigs()
    {
        gameConfig = Resources.Load<GlobalConfig>(globalConfigPath);
        playersConfig = Resources.Load<PlayerGeneralConfig>(playerGeneralConfigPath);
    }
    void InitPurchasableSpaceGroups()
    {
        companiesConfig = Resources.Load<CompaniesConfig>(companiesGroupPath);
        stationsConfig = Resources.Load<StationsConfig>(stationsGroupPath);
    }
    void InitEventSpaceGroup()
    {
        eventSpaceGroup = Resources.Load<SpaceGroupConfig>(eventSpaceGroupPath);
    }
    void InitEventCards()
    {
        communityCards = Resources.Load<CommunityChestsConfig>(communityChestCardsPath);
        chanceCards = Resources.Load<ChancesConfig>(chanceCardsPath);
        busTickets = Resources.Load<BusTicketsConfig>(busTicketsPath);

        int[] tempArray = new int[gameConfig.busTicket];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = i;
        }
        currentBusTickets = new HashSet<int>(tempArray);
    }
    void InitActionsOnEventSpaces()
    {
        triggerActionsOnEventSpaces = new Dictionary<EventType, Action<PlayerData>>()
        {
            { EventType.Nontrigger, _ => { } },
            { EventType.CommunityChest, data => TakeCard(data, communityCards, gameConfig.communityChestCard) },
            { EventType.Chance, data => TakeCard(data, chanceCards, gameConfig.chanceCard) },
            { EventType.BusTicket, data => TakeCard(data, busTickets, gameConfig.busTicket, currentBusTickets) },
            { EventType.GotoJail, data => { } },
            { EventType.Tax, data => data.SetCurrentCoin(-2) },
            { EventType.Surtax, data => data.SetCurrentCoin(-2) },
            { EventType.GiftReceive, data => data.SetCurrentCoin(2) },
            { EventType.Auction, data => { } },
        };

        chanceCards.action.OnChangeToCommunityCard(triggerActionsOnEventSpaces[EventType.CommunityChest]);
        chanceCards.action.OnChangeToBusTicket(triggerActionsOnEventSpaces[EventType.BusTicket]);
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
        if (eventSpaceGroup.eventDictionary.TryGetValue(positionIndex, out EventType theEvent))
        {
            triggerActionsOnEventSpaces[theEvent](playersData[playerIndex]);
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

    void TakeBusTicket(PlayerData data, int ticketIndex)
    {
        currentBusTickets.Remove(ticketIndex);
        data.KeepTicket(ticketIndex);
    }

    void UseBusTicket(PlayerData data, int ticketIndex)
    {
        //busTickets.action
        currentBusTickets.Add(ticketIndex);
        data.GiveBackTicket(ticketIndex);
    }
}