using UnityEngine;

[CreateAssetMenu(fileName = "New Companies", menuName = "Scriptable Objects/Companies Config")]
public class CompaniesConfig : SpaceGroupConfig
{
    public uint companyCount = 3;
    public uint eachPurchaseCost = 150;
    public uint dicePointMultiplierIfHasOne = 4;
    public uint dicePointMultiplierIfHasTwo = 10;
}