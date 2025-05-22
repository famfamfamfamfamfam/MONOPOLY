using System;

public class Driver
{
    public struct ConstructorParams
    {
        public DataManager manager;
        public PlayerDataService playerService;
        public TriggerSpaceService triggerSpaceService;
        public BusTicketService busTicketService;
        public int playersInitialCoin;
    }

    DataManager dataManager;

    int playersInitialCoin;
    PlayerDataService playerService;
    TriggerSpaceService triggerSpaceService;
    BusTicketService busTicketService;
    public Driver(ConstructorParams inputs)
    {
        dataManager = inputs.manager;
        playerService = inputs.playerService;
        triggerSpaceService = inputs.triggerSpaceService;
        playersInitialCoin = inputs.playersInitialCoin;
        busTicketService = inputs.busTicketService;
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
        triggerSpaceService.PurchaseSpace(0, dataManager.curPosIndex % 52);
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

}

public interface INeedDriver
{
    Driver driver { get; set; }
}