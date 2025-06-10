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
    }

    DiceRoller diceRoller;

    GameData commonData;

    int playersInitialCoin;
    PlayerDataService playerService;
    TriggerSpaceService triggerSpaceService;
    BusTicketService busTicketService;
    Dictionary<string, int> propertiesDictionary;
    Dictionary<string, List<string>> propertiesNamesDictionary;
    AssetAccessor assetAccessor;
    Func<int, bool> isUppercased;
    int latestRetrievedPropertyIndex;
    public Driver(ConstructorParams inputs)
    {
        commonData = inputs.commonData;
        diceRoller = inputs.diceRoller;
        playerService = inputs.playerService;
        triggerSpaceService = inputs.triggerSpaceService;
        playersInitialCoin = inputs.playersInitialCoin;
        busTicketService = inputs.busTicketService;
        assetAccessor = inputs.assetAccessor;
        isUppercased = num => num >= 65 && num <= 90;
        propertiesDictionary = new();
        propertiesNamesDictionary = new();
        for (int i = 0; i < inputs.properties.Length; i++)
        {
            if (inputs.properties[i] != null)
            {
                propertiesDictionary.Add(inputs.properties[i].spaceName, i);
                HandleStringsToInitNameDict(inputs.properties[i].spaceName);
            }
        }
        propertiesDictionary.TrimExcess();
        propertiesNamesDictionary.TrimExcess();
    }
    void HandleStringsToInitNameDict(string s)
    {
        int c = 0;
        bool hasJustCheckedSpecialChar = false;
        for (int i = 0; i < s.Length; i++)
        {
            if (!isUppercased(s[i]) && (s[i] < 97 || s[i] > 122) && (s[i] < 48 || s[i] > 57))
            {
                HandleSpecialChar(hasJustCheckedSpecialChar, s, ref c, i);
                hasJustCheckedSpecialChar = true;
            }
            else
            {
                HandleUppercase(hasJustCheckedSpecialChar, s, ref c, i);
                hasJustCheckedSpecialChar = false;
            }
        }
    }
    void HandleSpecialChar(bool hasJustCheckedSpecialChar, string s, ref int startOfCut, int cutPoint)
    {
        if (hasJustCheckedSpecialChar)
        {
            startOfCut++;
            return;
        }
        AddTokenToDictionary(s, startOfCut, cutPoint);
        startOfCut = cutPoint + 1;
    }
    void AddTokenToDictionary(string s, int first, int last)
    {
        string token = s.Substring(first, last - first);
        if (propertiesNamesDictionary.ContainsKey(token))
        {
            propertiesNamesDictionary[token].Add(s);
        }
        else
        {
            propertiesNamesDictionary.Add(token, new List<string>() { s });
        }
    }
    void HandleUppercase(bool hasJustCheckedSpecialChar, string s, ref int startOfCut, int cutPoint)
    {
        if (!hasJustCheckedSpecialChar
            && cutPoint != 0
            && isUppercased(s[cutPoint]))
        {
            AddTokenToDictionary(s, startOfCut, cutPoint);
            startOfCut = cutPoint;
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

    public bool IsValidPropertyName(string name)
    {
        bool contain = propertiesDictionary.TryGetValue(name, out int index);
        if (contain)
        {
            latestRetrievedPropertyIndex = index;
        }
        return contain && playerService.IsOwner(commonData.gamerPlayIndex,
            assetAccessor.GetAsset(latestRetrievedPropertyIndex));
    }

    public bool HasExistedInPropertiesNames(string partOfName, out List<int> propertyIndices)
    {
        bool result = propertiesNamesDictionary.TryGetValue(partOfName, out List<string> keys);
        if (result)
        {
            propertyIndices = new List<int>();
            for (int i = 0; i < keys.Count; i++)
            {
                propertyIndices.Add(propertiesDictionary[keys[i]]);
            }
        }
        else
        {
            propertyIndices = null;
        }
        return result;
    }

}

public interface INeedDriver
{
    Driver driver { get; set; }
}