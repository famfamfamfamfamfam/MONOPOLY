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
        actionOnEventSpaces[(byte)EventType.Nontrigger] = _ => { };
        actionOnEventSpaces[(byte)EventType.CommunityChest] = index => inputs.communityService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.Chance] = index => inputs.chanceService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.BusTicket] = index => inputs.busService.TriggerACard(index);
        actionOnEventSpaces[(byte)EventType.GotoJail] = index => GoToJail(index);
        actionOnEventSpaces[(byte)EventType.Tax] = index => inputs.playerService.SetCurrentCoin(index, -inputs.taxConfig.cost);
        actionOnEventSpaces[(byte)EventType.Surtax] = index => inputs.playerService.SetCurrentCoin(index, -inputs.surtaxConfig.cost);
        actionOnEventSpaces[(byte)EventType.GiftReceive] = index => ReceiveGift(index);
        actionOnEventSpaces[(byte)EventType.Auction] = index => Debug.LogWarning("COMING");

        setUpStationRentCost = ownerIndex =>
        {
            foreach (var index in inputs.stations.spacesIndices)
            {
                StationData currentStation = inputs.assetService.GetAsset<StationData>(index);
                if (inputs.playerService.IsOwner(ownerIndex, currentStation))
                {
                    inputs.stationService.IncreaseRentCost(currentStation);
                }
            }
        };
        payToPurchase = cost => inputs.playerService.SetCurrentCoin(0, -cost);
    }

    void GoToJail(int prisonerIndex)
    {
        inputs.moveToJail.Invoke(inputs.theJailPosition);
        inputs.playerService.BeInJail(prisonerIndex);
    }

    void ReceiveGift(int receiverIndex)
    {
        if (inputs.boardService.GiftASpace(out int giftIndex))
        {
            PlayerOwnsSpace(receiverIndex, giftIndex);
        }
    }

    void PlayerOwnsSpace(int playerIndex, int spaceIndex)
    {
        inputs.boardService.GrantSpace(spaceIndex);
        inputs.playerService.AddAsset(playerIndex,
            inputs.assetService.GetAsset(spaceIndex),
            setUpStationRentCost);
    }

    public void TriggerSpace(int playerIndex, int spaceIndex)
    {
        TriggerEventSpaces(playerIndex, spaceIndex, out bool hasTriggered);
        if (hasTriggered)
        {
            return;
        }
        TriggerPurchasableSpaces(playerIndex, spaceIndex);
    }
    void TriggerEventSpaces(int playerIndex, int spaceIndex, out bool hasTriggered)
    {
        hasTriggered = false;
        if (inputs.eventSpaces.eventDictionary.TryGetValue(spaceIndex, out EventType theEvent))
        {
            actionOnEventSpaces[(byte)theEvent].Invoke(playerIndex);
            hasTriggered = true;
        }
    }
    void TriggerPurchasableSpaces(int playerIndex, int spaceIndex)
    {
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
    }
    void ActionOnAssetOwnership(int playerIndex, int spaceIndex, ref bool isPurchased, int indexInLoop, ref bool canBreak)
    {
        if (inputs.playerService.IsOwner(indexInLoop, inputs.assetService.GetAsset(spaceIndex)))
        {
            canBreak = true;
            if (playerIndex == indexInLoop)
            {
                return;
            }
            if (inputs.companies.spacesIndices.Contains(spaceIndex))
            {
                CompanyCost(indexInLoop, playerIndex, spaceIndex);
            }
            else
            {
                Cost(indexInLoop, playerIndex, spaceIndex);
            }
            return;
        }
        isPurchased = false;
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