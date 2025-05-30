using System;
using System.Collections.Generic;

public abstract class CardService<T>
{
    protected T cardsConfig;
    protected PlayerDataService playerService;
    protected CardService(T config, PlayerDataService service)
    {
        cardsConfig = config;
        playerService = service;
    }

    public abstract void TriggerACard(int accessorIndex);
    // các collection để ánh xạ các Action trong các lớp con có thể cần được thiết kế lại, tuân thủ OCP
}

public class CommunityChestService : CardService<CommunityChestsConfig>
{
    Dictionary<CommunityChestsConfig.CommunityChest.MoneyChangeType, Action<int, int>> communityActions;
    public CommunityChestService(CommunityChestsConfig config, GlobalConfig gameConfig, PlayerDataService service) : base(config, service)
    {
        playerService = service;
        communityActions = new()
        {
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.AllChange, 
                (_, value) => playerService.IterateAllPlayers(indexInLoop => ChangeEveryPlayerCoin(value, indexInLoop)) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Opposite,
                (index, value) => playerService.IterateAllPlayers(indexInLoop => ChangePlayersCoinWithSelection(index, value, indexInLoop)) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Donate,
                (index, value) => playerService.IterateAllPlayers(indexInLoop => ChangePlayersCoinWithSelection(index, value, indexInLoop, gameConfig.playerCount - 1)) }
        };
        communityActions.TrimExcess();
    }

    void ChangeEveryPlayerCoin(int changeValue, int currentIndexInLoop)
    {
        UnityEngine.Debug.Log("ChangeEveryPlayerCoin-method runs from CommunityChestService");
        playerService.SetCurrentCoin(currentIndexInLoop, changeValue);
    }

    void ChangePlayersCoinWithSelection(int accessorIndex, int changeValue, int currentIndexInLoop, int divisorForTheOthers = 1)
    {
        UnityEngine.Debug.Log("ChangePlayersCoinWithSelection-method runs from CommunityChestService");
        if (currentIndexInLoop == accessorIndex)
        {
            playerService.SetCurrentCoin(accessorIndex, changeValue);
            UnityEngine.Debug.Log("Card drawer's coin changed by " + changeValue);
            return;
        }
        playerService.SetCurrentCoin(accessorIndex, -changeValue / divisorForTheOthers);
        UnityEngine.Debug.Log($"Coin of the player at index {accessorIndex} changed by {-changeValue / divisorForTheOthers}");
    }

    public override void TriggerACard(int accessorIndex)
    {
        UnityEngine.Debug.Log("TriggerACard-method runs from CommunityChestService");
        CommunityChestsConfig.CommunityChest card = GetRandomCard();
        UnityEngine.Debug.Log($"Got a community chest : {card.moneyChange} with {card.value}");
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
        public Action<int> triggerCommunityCard;
        public Action<int> triggerBusTicket;
    }

    int chanceCardCount;
    Action<int> triggerCommunityCard;
    Action<int> triggerBusTicket;
    public ChanceService(ConstructorParams inputs) : base(inputs.config, inputs.service)
    {
        chanceCardCount = inputs.gameConfig.chanceCard;
        playerService = inputs.service;
        triggerCommunityCard = inputs.triggerCommunityCard;
        triggerBusTicket = inputs.triggerBusTicket;
    }

    public override void TriggerACard(int accessorIndex)
    {
        UnityEngine.Debug.Log("TriggerACard-method runs from ChanceService");
        int cardIndex = GetRandomCardIndex(out bool isChangeCardIndex);
        if (isChangeCardIndex)
        {
            TriggerNewCard(accessorIndex, cardIndex);
        }
        else
        {
            playerService.SetCurrentCoin(accessorIndex, cardsConfig.changeMoneyValues[cardIndex]);
        }
    }
    int GetRandomCardIndex(out bool isChangeCardIndex)
    {
        UnityEngine.Debug.Log("GetRandomCardIndex-method runs from ChanceService");
        int result = UnityEngine.Random.Range(0, chanceCardCount);
        if (result < cardsConfig.changeMoneyValues.Length)
        {
            isChangeCardIndex = false;
            UnityEngine.Debug.Log($"Got a chance card to change coin: {cardsConfig.changeMoneyValues[result]}");
            return result;
        }
        isChangeCardIndex = true;
        return result % cardsConfig.changeMoneyValues.Length;
    }
    void TriggerNewCard(int accessorIndex, int cardIndex)
    {
        UnityEngine.Debug.Log("TriggerNewCard-method runs from ChanceService");
        switch (cardsConfig.changeCards[cardIndex])
        {
            case ChancesConfig.CardChangeType.CommunityChest:
                UnityEngine.Debug.Log($"Got a chance card : change to a community card");
                triggerCommunityCard.Invoke(accessorIndex);
                return;
            case ChancesConfig.CardChangeType.BusTicket:
                UnityEngine.Debug.Log($"Got a chance card : change to a bus ticket");
                triggerBusTicket.Invoke(accessorIndex);
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
        public SpaceConfig goToJailSpace;
        public SpaceConfig prison;
        public PlayerDataService playerService;
        public BoardDataService boardService;
        public Action<UnityEngine.Vector3, int> moveAction;
        public Action rollThirdDieAndStepAction;
    }

    public Action<BusTicketsConfig.KeepToUseTicket> onKeepTicket;

    CompaniesConfig companies;
    StationsConfig stations;
    BoardDataService boardService;
    Action[] busTicketActions;

    public BusTicketService(ConstructorParams inputs) : base(inputs.config, inputs.playerService)
    {
        companies = inputs.companies;
        stations = inputs.stations;
        playerService = inputs.playerService;
        boardService = inputs.boardService;

        int actionCount = Enum.GetValues(typeof(BusTicketsConfig.InstantUseTicket)).Length
            + Enum.GetValues(typeof(BusTicketsConfig.KeepToUseTicket)).Length;
        busTicketActions = new Action[actionCount];
        
        busTicketActions[(byte)BusTicketsConfig.InstantUseTicket.GoToJail]
            = () => inputs.moveAction.Invoke(inputs.goToJailSpace.position, inputs.goToJailSpace.indexFromGoSpace);
        busTicketActions[(byte)BusTicketsConfig.InstantUseTicket.RandomUtilitySpace]
            = () => GoToAUtility(inputs);
        busTicketActions[(byte)BusTicketsConfig.InstantUseTicket.GoSpace]
            = () => inputs.moveAction.Invoke(inputs.goSpace.position, inputs.goSpace.indexFromGoSpace);
        busTicketActions[(byte)BusTicketsConfig.KeepToUseTicket.ThirdDieRoll]
            = () => inputs.rollThirdDieAndStepAction.Invoke();
        busTicketActions[(byte)BusTicketsConfig.KeepToUseTicket.AuctionSpace]
            = () => inputs.moveAction.Invoke(inputs.auctionSpace.position, inputs.auctionSpace.indexFromGoSpace);
        busTicketActions[(byte)BusTicketsConfig.KeepToUseTicket.QuitFromJail]
            = () => inputs.moveAction.Invoke(inputs.prison.position, inputs.prison.indexFromGoSpace);
    }

    void GoToAUtility(ConstructorParams inputs)
    {
        UnityEngine.Debug.Log("GoToAUtility-method runs from BusTicketService");
        int rand = UnityEngine.Random.Range(0, 2);
        if (GetAUtilityPosition(rand, out (UnityEngine.Vector3 pos, int index) newPosition))
        {
            inputs.moveAction.Invoke(newPosition.pos, newPosition.index);
        }
    }
    bool GetAUtilityPosition(int randZeroOrOne, out (UnityEngine.Vector3 pos, int index) result)
    {
        UnityEngine.Debug.Log("GetAUtilityPosition-method runs from BusTicketService");
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
        UnityEngine.Debug.Log("TriggerACard-method runs from BusTicketService");
        boardService.GiveBusTicket(out int ticket);
        // ticket ở đây không phải index mà là chính giá trị phần tử
        if (Enum.IsDefined(typeof(BusTicketsConfig.InstantUseTicket),
            (BusTicketsConfig.InstantUseTicket)ticket))
        {
            TriggerTicket(accessorIndex, ticket, true);
        }
        else
        {
            playerService.KeepTicket(accessorIndex, ticket);
            onKeepTicket.Invoke((BusTicketsConfig.KeepToUseTicket)ticket);
        }
    }

    public void BusTicketInKeepNowBeUsed(int userIndex, int ticket)
    {
        TriggerTicket(userIndex, ticket);
    }

    void TriggerTicket(int accessorIndex, int ticket, bool isInstantUseTicket = false)
    {
        if (isInstantUseTicket)
        {
            UnityEngine.Debug.Log($"Got a bus ticket : {(BusTicketsConfig.InstantUseTicket)ticket}");
            busTicketActions[ticket].Invoke();
        }
        else
        {
            UnityEngine.Debug.Log($"Got a bus ticket : {(BusTicketsConfig.KeepToUseTicket)ticket}");
            busTicketActions[ticket].Invoke();
            playerService.GiveBackTicket(accessorIndex, ticket);
        }
        boardService.TakeBackBusTicket(ticket);
    }

}