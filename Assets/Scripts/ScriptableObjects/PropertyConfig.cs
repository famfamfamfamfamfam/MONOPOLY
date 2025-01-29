using UnityEngine;

[CreateAssetMenu(fileName = "New Property", menuName = "Scriptable Objects/Property Config")]
public class PropertyConfig : SpaceConfig
{
    public uint purchaseCost;
    public uint buildCost;
    public uint upgradeThreshold;
    public uint upgradeCost;
    public uint maxBuildingInSpace;
    public uint rentCostAfterPurchase;
    public uint rentCostIncreaseAfterBuild;
    public uint rentCostIncreaseAfterUpgrade;
}