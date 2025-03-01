using System;
using UnityEngine;

public class TriggerSpaceService // có thể cần tách thành các composition
{
    public struct ConstructorParams
    {
        public SpaceGroupConfig eventSpaces;
        public CompaniesConfig companies;
        public StationsConfig stations;

        public TaxConfig taxConfig;
        public TaxConfig surtaxConfig;

        public Vector3 theJailPosition;
        public Action<Vector3> moveToJail;

        public CommunityChestService communityService;
        public ChanceService chanceService;
        public BusTicketService busService;
        public PropertyDataService propertyService;
        public CompanyDataService companyService;
        public StationDataService stationService;
        public AssetAccessor assetService;
        public BoardDataService boardService;
        public PlayerDataService playerService;
    }

    ConstructorParams inputs;
    Action<int>[] actionOnEventSpaces;
    Action<int> setUpStationRentCost;
    Action<int> payToPurchase;
    public TriggerSpaceService(ConstructorParams inputs)
    {
        this.inputs = inputs;
        actionOnEventSpaces = new Action<int>[Enum.GetValues(typeof(EventType)).Length];
        actionOnEventSpaces[(byte)EventType.Nontrigger] = _ => Debug.Log("Nontrigger-action runs from TriggerSpaceService");
        actionOnEventSpaces[(byte)EventType.CommunityChest] = index => inputs.communityService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.Chance] = index => inputs.chanceService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.BusTicket] = index => inputs.busService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.GotoJail] = index => GoToJail(index);
        actionOnEventSpaces[(byte)EventType.Tax] = index => inputs.playerService.SetCurrentCoin(index, -inputs.taxConfig.cost);
        actionOnEventSpaces[(byte)EventType.Surtax] = index => inputs.playerService.SetCurrentCoin(index, -inputs.surtaxConfig.cost);
        actionOnEventSpaces[(byte)EventType.GiftReceive] = index => ReceiveGift(index);
        actionOnEventSpaces[(byte)EventType.Auction] = _ => Debug.LogWarning("COMING");

        setUpStationRentCost = ownerIndex =>
        {
            Debug.Log("setUpStationRentCost-action runs from TriggerSpaceService");
            foreach (var index in inputs.stations.spacesIndices)
            {
                StationData currentStation = inputs.assetService.GetAsset<StationData>(index);
                if (inputs.playerService.IsOwner(ownerIndex, currentStation))
                {
                    Debug.Log($"The player at index {ownerIndex} is the owner of the station");
                    inputs.stationService.IncreaseRentCost(currentStation);
                }
            }
        };
        payToPurchase = cost => inputs.playerService.SetCurrentCoin(0, -cost);
    }

    void GoToJail(int prisonerIndex)
    {
        Debug.Log("GoToJail-method runs from TriggerSpaceService");
        inputs.moveToJail.Invoke(inputs.theJailPosition);
        inputs.playerService.BeInJail(prisonerIndex);
    }

    void ReceiveGift(int receiverIndex)
    {
        Debug.Log("ReceiveGift-method runs from TriggerSpaceService");
        if (inputs.boardService.GiftASpace(out int giftIndex))
        {
            Debug.Log($"Got a gift : a space at {giftIndex}");
            PlayerOwnsSpace(receiverIndex, giftIndex);
        }
        else
        {
            Debug.Log("No gift given");
        }
    }

    void PlayerOwnsSpace(int playerIndex, int spaceIndex)
    {
        Debug.Log("PlayerOwnsSpace-method runs from TriggerSpaceService");
        inputs.boardService.GrantSpace(spaceIndex);
        inputs.playerService.AddAsset(playerIndex,
            inputs.assetService.GetAsset(spaceIndex),
            setUpStationRentCost);
    }

    public void TriggerSpace(int playerIndex, int spaceIndex)
    {
        Debug.Log("TriggerSpace-method runs from TriggerSpaceService");
        TriggerEventSpaces(playerIndex, spaceIndex, out bool hasTriggered);
        if (hasTriggered)
        {
            return;
        }
        TriggerPurchasableSpaces(playerIndex, spaceIndex);
    }
    void TriggerEventSpaces(int playerIndex, int spaceIndex, out bool hasTriggered)
    {
        Debug.Log("TriggerEventSpaces-method runs from TriggerSpaceService");
        hasTriggered = false;
        if (inputs.eventSpaces.eventDictionary.TryGetValue(spaceIndex, out EventType theEvent))
        {
            Debug.Log("This is an event space");
            actionOnEventSpaces[(byte)theEvent].Invoke(playerIndex);
            hasTriggered = true;
            return;
        }
        Debug.Log("This is not an event space");
    }
    void TriggerPurchasableSpaces(int playerIndex, int spaceIndex)
    {
        Debug.Log("TriggerPurchasableSpaces-method runs from TriggerSpaceService");
        bool canBreak = false;
        bool isPurchased = true;
        inputs.playerService.IterateAllPlayers(
            currentIndex => ActionOnAssetOwnership(playerIndex, spaceIndex, ref isPurchased, currentIndex, ref canBreak),
            () => canBreak);
        if (!isPurchased)
        {
            inputs.propertyService.SetCurrentPropertyIndex(spaceIndex);
            inputs.assetService.PickTheService(
                spaceIndex, inputs.propertyService, inputs.stationService, inputs.companyService
                ).BePurchased(payToPurchase);
            PlayerOwnsSpace(playerIndex, spaceIndex);
            Debug.Log("Purchased");
        }
        else
        {
            Debug.Log("Costed");
        }
    }
    void ActionOnAssetOwnership(int playerIndex, int spaceIndex, ref bool isPurchased, int indexInLoop, ref bool canBreak)
    {
        Debug.Log("ActionOnAssetOwnership-method runs from TriggerSpaceService");
        if (inputs.playerService.IsOwner(indexInLoop, inputs.assetService.GetAsset(spaceIndex)))
        {
            canBreak = true;
            Debug.Log("Found the owner");
            if (playerIndex == indexInLoop)
            {
                Debug.Log("The player in the space is also the owner");
                return;
            }
            if (inputs.companies.spacesIndices.Contains(spaceIndex))
            {
                Debug.Log("The space is a company");
                CompanyCost(indexInLoop, playerIndex, spaceIndex);
            }
            else
            {
                Debug.Log("The space is a station or property");
                Cost(indexInLoop, playerIndex, spaceIndex);
            }
            return;
        }
        Debug.Log("Cannot found the owner");
        isPurchased = false;
    }

    void CompanyCost(int lessorIndex, int lesseeIndex, int companyIndex)
    {
        Debug.Log("CompanyCost-method runs from TriggerSpaceService");
        inputs.companyService.UpdateRentCost(companyIndex,
            inputs.playerService.GetDicePoint(lesseeIndex),
            inputs.playerService.GetCurrentCompanyCount(lessorIndex));
        Cost(lessorIndex, lesseeIndex, companyIndex);
    }

    void Cost(int lessorIndex, int lesseeIndex, int assetIndex)
    {
        Debug.Log("Cost-method runs from TriggerSpaceService");
        inputs.playerService.SetCurrentCoin(lesseeIndex, -inputs.assetService.GetRentCost(assetIndex));
        inputs.playerService.SetCurrentCoin(lessorIndex, inputs.assetService.GetRentCost(assetIndex));
    }
}