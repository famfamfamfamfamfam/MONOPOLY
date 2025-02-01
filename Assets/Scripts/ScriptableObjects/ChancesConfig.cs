using UnityEngine;

[CreateAssetMenu(fileName = "New Chances Config", menuName = "Scriptable Objects/Chances Config")]
public class ChancesConfig : CardsConfig
{
    public ChanceAction action;
    [SerializeField]
    int[] changeMoneyValues;
    enum CardChangeType : byte
    {
        CommunityChest,
        BusTicket
    }
    [SerializeField]
    CardChangeType[] changeCards;

    public override void AccessTheCard(PlayerData player, PlayerData[] _, int cardIndex)
    {
        if (cardIndex < changeMoneyValues.Length)
        {
            action.ChangeCoin(player, changeMoneyValues[cardIndex]);
        }
        else
        {
            switch (changeCards[cardIndex % changeMoneyValues.Length])
            {
                case CardChangeType.CommunityChest:
                    action.ChangeToCommunityCard(player);
                    return;
                case CardChangeType.BusTicket:
                    action.ChangeToBusTicket(player);
                    return;
            }
        }
    }
}