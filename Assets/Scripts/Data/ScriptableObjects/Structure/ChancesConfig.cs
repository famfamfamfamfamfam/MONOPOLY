using UnityEngine;

[CreateAssetMenu(fileName = "New Chances Config", menuName = "Scriptable Objects/Chances Config")]
public class ChancesConfig : ScriptableObject
{
    public int[] changeMoneyValues;
    public enum CardChangeType : byte
    {
        CommunityChest,
        BusTicket
    }
    public CardChangeType[] changeCards;
}