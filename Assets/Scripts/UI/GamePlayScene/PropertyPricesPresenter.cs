using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertyPricesPresenter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content;

    readonly string buildCost = "Build Cost: -";
    readonly string upgradeCost = "Upgrade Cost: -";
    readonly string upgradeThreshold = "Can upgrade when buildings reach: ";
    readonly string maxBuildingInSpace = "Property Capacity at the same time: ";
    readonly string rentCostAfterPurchase = "Price-to-Rent Ratio: ";
    readonly string rentCostAfterBuild = "Rent after a build: +";
    readonly string rentCostAfterUpgrade = "Rent after a upgrade: +";

    PropertyConfig[] propertyConfigs;
    public void SetPropertyConfigs(PropertyConfig[] propertyConfigs)
    {
        this.propertyConfigs = propertyConfigs;
    }

    public void UpdatePricesToDisplay(List<int> propertyIndices)
    {
        content.text = null;
        for (int i = 0; i < propertyIndices.Count; i++)
        {
            int currentIndex = propertyIndices[i];
            content.text += '\t' + propertyConfigs[currentIndex].spaceName + '\n';
            content.text += buildCost + propertyConfigs[currentIndex].buildCost + "$\n";
            content.text += upgradeCost + propertyConfigs[currentIndex].upgradeCost + "$\n";
            content.text += upgradeThreshold + propertyConfigs[currentIndex].upgradeThreshold + '\n';
            content.text += maxBuildingInSpace + propertyConfigs[currentIndex].maxBuildingInSpace + '\n';
            content.text += rentCostAfterPurchase + 
                ((float)propertyConfigs[currentIndex].purchaseCost / propertyConfigs[currentIndex].rentCostAfterPurchase) + '\n';
            content.text += rentCostAfterBuild + propertyConfigs[currentIndex].rentCostIncreaseAfterBuild + "$\n";
            content.text += rentCostAfterUpgrade + propertyConfigs[currentIndex].rentCostIncreaseAfterUpgrade + "$\n\n";
        }
    }
}
