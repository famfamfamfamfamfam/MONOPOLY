using System;
using TMPro;
using UnityEngine;

public class InfoDisplayManager : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown optionsDropdown;
    [SerializeField]
    GameObject[] assetsLists; // kéo vào theo đúng thứ tự của options trong dropdown
    string closeLabel = "Close";
    string lookUpLabel = "Look up";

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
                assetsLists[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    private void OnDisable()
    {
        optionsDropdown.onValueChanged.RemoveAllListeners();
    }
}
