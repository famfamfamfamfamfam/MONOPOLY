using System;
using System.Collections.Generic;

public class Driver
{
    public struct ConstructorParams
    {
        public DataManager manager;
        public GameData commonData;
        public DiceRoller diceRoller;
        public PlayerDataService playerService;
        public TriggerSpaceService triggerSpaceService;
        public BusTicketService busTicketService;
        public int playersInitialCoin;
        public AssetAccessor assetAccessor;
        public PropertyConfig[] properties;
    }

    DataManager dataManager;

    DiceRoller diceRoller;

    GameData commonData;

    int playersInitialCoin;
    PlayerDataService playerService;
    TriggerSpaceService triggerSpaceService;
    BusTicketService busTicketService;
    Dictionary<string, int> propertiesDictionary;
    AssetAccessor assetAccessor;
    public Driver(ConstructorParams inputs)
    {
        dataManager = inputs.manager;
        commonData = inputs.commonData;
        diceRoller = inputs.diceRoller;
        playerService = inputs.playerService;
        triggerSpaceService = inputs.triggerSpaceService;
        playersInitialCoin = inputs.playersInitialCoin;
        busTicketService = inputs.busTicketService;
        assetAccessor = inputs.assetAccessor;
        propertiesDictionary = new();
        for (int i = 0; i < inputs.properties.Length; i++)
        {
            if (inputs.properties[i] != null)
            {
                propertiesDictionary.Add(inputs.properties[i].spaceName, i);
            }
        }
        propertiesDictionary.TrimExcess();
    }

    public int PlayersInitialCoin()
    {
        return playersInitialCoin;
    }

    public void OnPlayerCoinChange(Action<int, int> onPlayerCoinChange)
    {
        playerService.coinChanged = onPlayerCoinChange;
    }

    public void AskToPurchaseSpace(Action hasChoices)
    {
        triggerSpaceService.onNotYetPurchaseSpace = hasChoices;
    }

    public void PurchaseTheSpace()
    {
        triggerSpaceService.PurchaseSpace(commonData.gamerPlayIndex, playerService.GetCurrentStaySpaceIndex(commonData.gamerPlayIndex));
    }

    public void AfterPurchaseDecision()
    {
        diceRoller.ActiveRoll();
    }

    public void Notif(Action<string> notif)
    {
        triggerSpaceService.hasNotif = notif;
    }

    public void OnKeepTheTicket(Action<BusTicketsConfig.KeepToUseTicket> actOnTicketKeep)
    {
        busTicketService.onKeepTicket = actOnTicketKeep;
    }

    public void UseTicketInKeep(int userIndex, int ticket)
    {
        busTicketService.BusTicketInKeepNowBeUsed(userIndex, ticket);
    }

    public void AddPurchasedSpaceToAssetList(Action<int, string> addToAssetList)
    {
        triggerSpaceService.onAlreadyPurchasedSpace = addToAssetList;
    }

    public bool IsValidPropertyName(string name)
    {
        return propertiesDictionary.ContainsKey(name)
            && playerService.IsOwner(
                commonData.gamerPlayIndex, assetAccessor.GetAsset(propertiesDictionary[name]));
    }

}

public interface INeedDriver
{
    Driver driver { get; set; }
}