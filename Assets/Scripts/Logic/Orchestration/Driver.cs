using System;

public class Driver
{
    DataManager dataManager;

    int playersInitialCoin;
    PlayerDataService playerService;
    TriggerSpaceService triggerSpaceService;
    public Driver(DataManager manager, PlayerDataService playerService, TriggerSpaceService triggerSpaceService, int playersInitialCoin)
    {
        dataManager = manager;
        this.playerService = playerService;
        this.triggerSpaceService = triggerSpaceService;
        this.playersInitialCoin = playersInitialCoin;
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

}

public interface INeedDriver
{
    Driver driver { get; set; }
}