using System;
using System.Collections.Generic;

public abstract class CardService<T>
{
    protected T cardsConfig;
    protected CardService(T config)
    {
        cardsConfig = config;
    }

    public abstract void TriggerACard(int accessorIndex);
}

public class CommunityChestService : CardService<CommunityChestsConfig>
{
    PlayerDataService playerService;
    Dictionary<CommunityChestsConfig.CommunityChest.MoneyChangeType, Action<int, int>> communityActions;
    public CommunityChestService(CommunityChestsConfig config, GlobalConfig gameConfig, PlayerDataService service) : base(config)
    {
        playerService = service;
        communityActions = new()
        {
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.AllChange, 
                (_, value) => playerService.IterateAllPlayers(indexInLoop => ChangeAllPlayersCoin(value, indexInLoop)) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Opposite,
                (index, value) => playerService.IterateAllPlayers(indexInLoop => ChangePlayersCoinWithSelection(index, value, indexInLoop)) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Donate,
                (index, value) => playerService.IterateAllPlayers(indexInLoop => ChangePlayersCoinWithSelection(index, value, indexInLoop, gameConfig.playerCount - 1)) }
        };
        communityActions.TrimExcess();
    }

    void ChangeAllPlayersCoin(int changeValue, int currentIndexInLoop)
    {
        playerService.SetCurrentCoin(currentIndexInLoop, changeValue);
    }

    void ChangePlayersCoinWithSelection(int accessorIndex, int changeValue, int currentIndexInLoop, int divisorForTheOthers = 1)
    {
        if (currentIndexInLoop == accessorIndex)
        {
            playerService.SetCurrentCoin(accessorIndex, changeValue);
            return;
        }
        playerService.SetCurrentCoin(accessorIndex, -changeValue / divisorForTheOthers);
    }

    public override void TriggerACard(int accessorIndex)
    {
        CommunityChestsConfig.CommunityChest card = GetRandomCard();
        communityActions[card.moneyChange].Invoke(accessorIndex, card.value);
    }
    CommunityChestsConfig.CommunityChest GetRandomCard()
    {
        return cardsConfig.chests[UnityEngine.Random.Range(0, cardsConfig.chests.Length)];
    }
}

public class ChanceService : CardService<ChancesConfig>
{
    public struct ConstructorParams
    {
        public ChancesConfig config;
        public GlobalConfig gameConfig;
        public PlayerDataService service;
        public Action triggerCommunityCard;
        public Action triggerBusTicket;
    }

    int chanceCardCount;
    PlayerDataService playerService;
    Action triggerCommunityCard;
    Action triggerBusTicket;
    public ChanceService(ConstructorParams inputs) : base(inputs.config)
    {
        chanceCardCount = inputs.gameConfig.chanceCard;
        playerService = inputs.service;
        triggerCommunityCard = inputs.triggerCommunityCard;
        triggerBusTicket = inputs.triggerBusTicket;
    }

    public override void TriggerACard(int accessorIndex)
    {
        int index = GetRandomCardIndex(out bool isChangeCardIndex);
        if (isChangeCardIndex)
        {
            TriggerNewCard(index);
        }
        else
        {
            playerService.SetCurrentCoin(accessorIndex, cardsConfig.changeMoneyValues[index]);
        }
    }
    int GetRandomCardIndex(out bool isChangeCardIndex)
    {
        int result = UnityEngine.Random.Range(0, chanceCardCount);
        if (result < cardsConfig.changeMoneyValues.Length)
        {
            isChangeCardIndex = false;
            return result;
        }
        isChangeCardIndex = true;
        return result % cardsConfig.changeMoneyValues.Length;
    }
    void TriggerNewCard(int index)
    {
        switch (cardsConfig.changeCards[index])
        {
            case ChancesConfig.CardChangeType.CommunityChest:
                triggerCommunityCard.Invoke();
                return;
            case ChancesConfig.CardChangeType.BusTicket:
                triggerBusTicket.Invoke();
                return;
        }
    }
}

public class BusTicketService : CardService<BusTicketsConfig>
{
    public struct ConstructorParams
    {
        public BusTicketsConfig config;
        public CompaniesConfig companies;
        public StationsConfig stations;
        public SpaceConfig goSpace;
        public SpaceConfig auctionSpace;
        public SpaceConfig jail;
        public SpaceConfig prison;
        public PlayerDataService playerService;
        public BoardDataService boardService;
        public Action<UnityEngine.Vector3, int> moveAction;
        public Action rollThirdDieAndStepAction;
    }

    CompaniesConfig companies;
    StationsConfig stations;
    PlayerDataService playerService;
    BoardDataService boardService;
    Action[] busTicketActions;
    int latestActionAccessKey;
    public BusTicketService(ConstructorParams inputs) : base(inputs.config)
    {
        companies = inputs.companies;
        stations = inputs.stations;
        playerService = inputs.playerService;
        boardService = inputs.boardService;
        busTicketActions = new Action[BusTicketsConfig.NUMBER_OF_TICKET_TYPE];
        busTicketActions[BusTicketsConfig.GO_TO_JAIL]
            = () => inputs.moveAction.Invoke(inputs.jail.position, inputs.jail.indexFromGoSpace);
        busTicketActions[BusTicketsConfig.RANDOM_UTILITY_SPACE]
            = () => GoToAUtility(inputs);
        busTicketActions[BusTicketsConfig.GO_SPACE]
            = () => inputs.moveAction.Invoke(inputs.goSpace.position, inputs.goSpace.indexFromGoSpace);
        busTicketActions[BusTicketsConfig.THIRD_DIE_ROLL]
            = () => inputs.rollThirdDieAndStepAction.Invoke();
        busTicketActions[BusTicketsConfig.AUCTION_SPACE]
            = () => inputs.moveAction.Invoke(inputs.auctionSpace.position, inputs.auctionSpace.indexFromGoSpace);
        busTicketActions[BusTicketsConfig.QUIT_FROM_JAIL]
            = () => inputs.moveAction.Invoke(inputs.prison.position, inputs.prison.indexFromGoSpace);
    }

    void GoToAUtility(ConstructorParams inputs)
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (GetAUtilityPosition(rand, out (UnityEngine.Vector3 pos, int index) newPosition))
        {
            inputs.moveAction.Invoke(newPosition.pos, newPosition.index);
        }
    }
    bool GetAUtilityPosition(int randZeroOrOne, out (UnityEngine.Vector3 pos, int index) result)
    {
        result = default;
        switch (randZeroOrOne)
        {
            case 0:
                int companyIndex = UnityEngine.Random.Range(0, companies.companyCount);
                result.pos = companies.spaces[companyIndex].position;
                result.index = companies.spaces[companyIndex].indexFromGoSpace;
                return true;
            case 1:
                int stationIndex = UnityEngine.Random.Range(0, stations.stationCount);
                result.pos = stations.spaces[stationIndex].position;
                result.index = stations.spaces[stationIndex].indexFromGoSpace;
                return true;
        }
        return false;
    }

    public override void TriggerACard(int accessorIndex)
    {
        boardService.GiveBusTicket(out int ticketIndex);
        if (ticketIndex < cardsConfig.instantUseTickets.Length)
        {
            TriggerTicket(accessorIndex, ticketIndex, true);
        }
        else
        {
            playerService.KeepTicket(accessorIndex, ticketIndex);
            latestActionAccessKey = -1;
        }
    }

    public void BusTicketInKeepNowBeUsed(int userIndex, int ticketIndex)
    {
        TriggerTicket(userIndex, ticketIndex);
    }

    void TriggerTicket(int accessorIndex, int ticketIndex, bool isInstantUseTicket = false)
    {
        if (isInstantUseTicket)
        {
            latestActionAccessKey = (int)cardsConfig.instantUseTickets[ticketIndex];
            busTicketActions[latestActionAccessKey].Invoke();
        }
        else
        {
            latestActionAccessKey = (int)cardsConfig.keepToUseTickets[ticketIndex % cardsConfig.instantUseTickets.Length];
            busTicketActions[latestActionAccessKey].Invoke();
            playerService.GiveBackTicket(accessorIndex, ticketIndex);
        }
        boardService.TakeBackBusTicket(ticketIndex);
    }

    public bool TryGetTheLatestAccessKey(out int key)
    {
        key = latestActionAccessKey;
        return latestActionAccessKey >= 0 && latestActionAccessKey < busTicketActions.Length;
    }
}