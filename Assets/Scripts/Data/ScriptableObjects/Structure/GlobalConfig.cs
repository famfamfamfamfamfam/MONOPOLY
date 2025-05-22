using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Scriptable Objects/Global Config")]
public class GlobalConfig : ScriptableObject
{
    public int playerCount = 4;
    public int spaceCount = 52;
    //public int communityChestCard = 12;
    public int chanceCard = 12;
    //public int busTicket = 8;
    public int passGoSpaceBonus = 200;
    public int monopolyBonus = 1000;
}