using System;
using TMPro;
using UnityEngine;

public class InfoDisplayManager : MonoBehaviour, INeedDriver
{
    [SerializeField]
    TMP_Dropdown optionsDropdown;
    [SerializeField]
    GameObject[] assetsLists; // kéo vào theo đúng thứ tự của options trong dropdown
    [SerializeField]
    TextMeshProUGUI[] assetListsContents; // kéo vào theo thứ tự sao cho index trùng index của player đại diện
    string closeLabel = "Close";
    string lookUpLabel = "Look up";

    public Driver driver { get; set ; }

    void Start()
    {
        driver.AddPurchasedSpaceToAssetList(ListToAsset);
    }

    void ListToAsset(int listIndex, string assetName)
    {
        assetListsContents[listIndex].text += assetName + "\n";
    }

    public void PrepareToUnfold(Action<GameObject> activePanels)
    {
        optionsDropdown.onValueChanged.AddListener(optionIndex =>
            {
                DeactiveAll();
                if (optionIndex == 0)
                {
                    optionsDropdown.options[0].text = lookUpLabel;
                    optionsDropdown.RefreshShownValue();
                    return;
                }
                optionsDropdown.options[0].text = closeLabel;
                //trừ chỉ số option đi 1 vì option đầu tiên được dùng làm tiêu đề
                activePanels(assetsLists[optionIndex - 1]);
            }
        );
    }

    void DeactiveAll()
    {
        for (int i = 0; i < assetsLists.Length; i++)
        {
            if (assetsLists[i].activeSelf)
            {
                assetsLists[i].SetActive(false);
                return;
            }
        }
    }

    private void OnDisable()
    {
        optionsDropdown.onValueChanged.RemoveAllListeners();
    }
}
