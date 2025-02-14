using System;

public class TriggerSpaceService
{
    public struct ConstructorParams
    {
        public SpaceGroupConfig eventSpaces;
        public CompaniesConfig companies;
        public StationsConfig stations;

        public CommunityChestService communityService;
        public ChanceService chanceService;
        public BusTicketService busService;
        public CompanyDataService companyService;
        public AssetRentCostAccessor assetService;

        public PlayerDataService playerService;
    }

    ConstructorParams inputs;
    Action[] actionOnEventSpaces;
    public TriggerSpaceService(ConstructorParams inputs)
    {
        this.inputs = inputs;
        actionOnEventSpaces = new Action[Enum.GetValues(typeof(EventType)).Length];
    }

    public void TriggerSpace(int playerIndex, int spaceIndex)
    {
        TriggerEventSpaces(spaceIndex, out bool hasTriggered);
        if (hasTriggered)
        {
            return;
        }
    }
    void TriggerEventSpaces(int spaceIndex, out bool hasTriggered)
    {
        hasTriggered = false;
        if (inputs.eventSpaces.eventDictionary.TryGetValue(spaceIndex, out EventType theEvent))
        {
            actionOnEventSpaces[(int)theEvent].Invoke();
            hasTriggered = true;
        }
    }
    void TriggerPurchasableSpaces()
    {

    }

    void CompanyCost(int lessorIndex, int lesseeIndex, int companyIndex)
    {
        inputs.companyService.UpdateRentCost(companyIndex,
            inputs.playerService.GetDicePoint(lesseeIndex),
            inputs.playerService.GetCurrentCompanyCount(lessorIndex));
        Cost(lessorIndex, lesseeIndex, companyIndex);
    }

    void Cost(int lessorIndex, int lesseeIndex, int assetIndex)
    {
        inputs.playerService.SetCurrentCoin(lesseeIndex, -inputs.assetService.GetRentCost(assetIndex));
        inputs.playerService.SetCurrentCoin(lessorIndex, inputs.assetService.GetRentCost(assetIndex));
    }
}