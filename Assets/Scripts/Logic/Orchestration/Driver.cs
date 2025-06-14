using System;
using System.Collections.Generic;

public class Driver
{
    public struct ConstructorParams
    {
        public GameData commonData;
        public DiceRoller diceRoller;
        public PlayerDataService playerService;
        public TriggerSpaceService triggerSpaceService;
        public BusTicketService busTicketService;
        public int playersInitialCoin;
        public AssetAccessor assetAccessor;
        public PropertyConfig[] properties;
        public BuildService buildService;
    }

    DiceRoller diceRoller;

    GameData commonData;

    int playersInitialCoin;
    PlayerDataService playerService;
    TriggerSpaceService triggerSpaceService;
    BuildService buildService;
    BusTicketService busTicketService;
    Dictionary<string, int> propertiesDictionary;
    Dictionary<string, HashSet<string>> propertiesNamesDictionary;
    List<string> inputTokens;
    AssetAccessor assetAccessor;
    bool isInitializingSourceDict = true;
    int latestRetrievedPropertyIndex;
    public Driver(ConstructorParams inputs)
    {
        commonData = inputs.commonData;
        diceRoller = inputs.diceRoller;
        playerService = inputs.playerService;
        triggerSpaceService = inputs.triggerSpaceService;
        buildService = inputs.buildService;
        playersInitialCoin = inputs.playersInitialCoin;
        busTicketService = inputs.busTicketService;
        assetAccessor = inputs.assetAccessor;
        propertiesDictionary = new();
        propertiesNamesDictionary = new();
        inputTokens = new List<string>();
        for (int i = 0; i < inputs.properties.Length; i++)
        {
            if (inputs.properties[i] != null)
            {
                propertiesDictionary.Add(inputs.properties[i].spaceName, i);
                InvertedIndexMachine.Instance.SplitString(inputs.properties[i].spaceName, AddTokenToCollection);
            }
        }
        isInitializingSourceDict = false;
        propertiesDictionary.TrimExcess();
        propertiesNamesDictionary.TrimExcess();
    }
    void AddTokenToCollection(string s, int first, int last)
    {
        string token = s.Substring(first, last - first);
        if (!isInitializingSourceDict)
        {
            inputTokens.Add(token);
            return;
        }

        if (propertiesNamesDictionary.ContainsKey(token))
        {
            propertiesNamesDictionary[token].Add(s);
        }
        else
        {
            propertiesNamesDictionary.Add(token, new HashSet<string>() { s });
        }
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

    public bool IsValidPropertyName(string name, out int propertyIndex)
    {
        bool contain = propertiesDictionary.TryGetValue(name, out int index);
        propertyIndex = -1;
        if (contain)
        {
            propertyIndex = latestRetrievedPropertyIndex = index;
        }
        return contain && playerService.IsOwner(commonData.gamerPlayIndex,
            assetAccessor.GetAsset(latestRetrievedPropertyIndex));
    }

    public bool HasExistedInPropertiesNames(string partOfName, out List<int> propertyIndices)
    {
        inputTokens.Clear();
        InvertedIndexMachine.Instance.SplitString(partOfName, AddTokenToCollection);
        HashSet<string> intersect = null;
        bool result = false;
        for (int i = 0; i < inputTokens.Count; i++)
        {
            UnityEngine.Debug.LogWarning(inputTokens[i]);
            if (propertiesNamesDictionary.TryGetValue(inputTokens[i], out HashSet<string> keys))
            {
                if (intersect == null)
                {
                    intersect = keys;
                    result = true;
                }
                else
                {
                    intersect.IntersectWith(keys);
                }
            }
        }

        if (result)
        {
            propertyIndices = new List<int>();
            foreach (string key in intersect)
            {
                UnityEngine.Debug.LogWarning(key);
                propertyIndices.Add(propertiesDictionary[key]);
            }
        }
        else
        {
            propertyIndices = null;
        }
        return result;
    }

    public void BuildNew()
    {
        buildService.BuildNewHouse(commonData.gamerPlayIndex, latestRetrievedPropertyIndex);
    }
    public void UpgradeBuildings(BuildingRate materialBuildings, BuildingRate expectedBuilding)
    {
        buildService.UpgradeHouses(commonData.gamerPlayIndex, latestRetrievedPropertyIndex, materialBuildings, expectedBuilding);
    }

}

public interface INeedDriver
{
    Driver driver { get; set; }
}