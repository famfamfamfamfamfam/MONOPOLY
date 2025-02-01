using UnityEngine;

[CreateAssetMenu(fileName = "New Community", menuName = "Scriptable Objects/Community Chests Config")]
public class CommunityChestsConfig : CardsConfig
{
    [SerializeField]
    CommunityChestAction action;
    [SerializeField]
    CommunityChest[] chests;

    public override void AccessTheCard(PlayerData currentPlayer, PlayerData[] players, int chestIndex)
    {
        switch (chests[chestIndex].moneyChange)
        {
            case CommunityChest.MoneyChangeType.AllChange:
                action.ChangeAllCoin(players, chests[chestIndex].value);
                return;
            case CommunityChest.MoneyChangeType.Opposite:
                action.OppositelyChangeCoin(currentPlayer, players, chests[chestIndex].value);
                return;
            case CommunityChest.MoneyChangeType.Donate:
                action.ChangeCoinByDonate(currentPlayer, players, chests[chestIndex].value);
                return;
        }
    }
}