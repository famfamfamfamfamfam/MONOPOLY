using UnityEngine;

[CreateAssetMenu(fileName = "New Companies", menuName = "Scriptable Objects/Companies Config")]
public class CompaniesConfig : SpaceGroupConfig
{
    public int companyCount = 3;
    public int eachPurchaseCost = 150;
    public int dicePointMultiplierIfHasOne = 4;
    public int dicePointMultiplierIfHasTwo = 10;
}