using UnityEngine;

[CreateAssetMenu(fileName = "New Property", menuName = "Scriptable Objects/Property Config")]
public class PropertyConfig : SpaceConfig
{
    public int purchaseCost;
    public int buildCost;
    public int upgradeThreshold;
    public int upgradeCost;
    public int maxBuildingInSpace;
    public int rentCostAfterPurchase;
    public int rentCostIncreaseAfterBuild;
    public int rentCostIncreaseAfterUpgrade;
}