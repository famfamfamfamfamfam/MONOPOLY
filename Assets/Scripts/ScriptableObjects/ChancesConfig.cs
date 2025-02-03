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

    public override void AccessTheCard(IOnEvent player, IChangeCoin[] _, int cardIndex)
    {
        if (cardIndex < changeMoneyValues.Length)
        {
            action.ChangeCoin(player as IChangeCoin, changeMoneyValues[cardIndex]);
        }
        else
        {
            switch (changeCards[cardIndex % changeMoneyValues.Length])
            {
                case CardChangeType.CommunityChest:
                    action.ChangeToCommunityCard(player as IChangeCoin);
                    return;
                case CardChangeType.BusTicket:
                    action.ChangeToBusTicket(player as IChangeCoin);
                    return;
            }
        }
    }
}