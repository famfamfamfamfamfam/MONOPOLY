using System;
using System.Collections.Generic;

public abstract class CardService<T>
{
    protected T cardsConfig;
    protected CardService(T config)
    {
        cardsConfig = config;
    }

    public abstract void TriggerACard(PlayerData accessorData);
}

public class CommunityChestService : CardService<CommunityChestsConfig>
{
    PlayerData[] playersData;
    PlayerDataService playerService;
    Dictionary<CommunityChestsConfig.CommunityChest.MoneyChangeType, Action<PlayerData, int>> communityActions;
    public CommunityChestService(CommunityChestsConfig config, PlayerData[] data, PlayerDataService service) : base(config)
    {
        playersData = data;
        playerService = service;
        communityActions = new()
        {
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.AllChange,
                (_, value) => ChangeAllPlayersCoin(_, value) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Opposite,
                (data, value) => ChangePlayersCoinWithSelection(data, value) },
            { CommunityChestsConfig.CommunityChest.MoneyChangeType.Donate,
                (data, value) => ChangePlayersCoinWithSelection(data, value, playersData.Length - 1) }
        };
        communityActions.TrimExcess();
    }

    void ChangeAllPlayersCoin(PlayerData _, int changeValue)
    {
        foreach (var data in playersData)
        {
            playerService.SetCurrentCoin(data, changeValue);
        }
    }
    void ChangePlayersCoinWithSelection(PlayerData accessorData, int changeValue, int divisorForTheOthers = 1)
    {
        foreach (var data in playersData)
        {
            if (data == accessorData)
            {
                playerService.SetCurrentCoin(accessorData, changeValue);
                continue;
            }
            playerService.SetCurrentCoin(accessorData, -changeValue / divisorForTheOthers);
        }
    }

    CommunityChestsConfig.CommunityChest GetRandomCard()
    {
        return cardsConfig.chests[UnityEngine.Random.Range(0, cardsConfig.chests.Length)];
    }
    public override void TriggerACard(PlayerData accessorData)
    {
        CommunityChestsConfig.CommunityChest card = GetRandomCard();
        communityActions[card.moneyChange].Invoke(accessorData, card.value);
    }
}

public class ChanceService : CardService<ChancesConfig>
{
    public ChanceService(ChancesConfig config) : base(config) { }




    public override void TriggerACard(PlayerData accessorData)
    {
        throw new NotImplementedException();
    }
}

public class BusTicketService : CardService<BusTicketsConfig>
{
    IBusTicketBoardService boardService;
    public BusTicketService(BusTicketsConfig config, IBusTicketBoardService boardService) : base(config)
    {
        this.boardService = boardService;
    }




    public override void TriggerACard(PlayerData accessorData)
    {
        throw new NotImplementedException();
    }
}