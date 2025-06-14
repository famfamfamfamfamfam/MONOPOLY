using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertyNameHandler : MonoBehaviour, INeedDriver
{
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField]
    GameObject buildOptionPanelObj;
    [SerializeField]
    GameObject pricesSwitch;
    [SerializeField]
    GameObject pricesDisplay;

    public Action<int> onFoundAssetByFullName { get; set; }
    public Action<List<int>> onFoundAListOfPropertiesByKeyword { get; set; }

    public Driver driver { get; set; }

    Action<GameObject> activeToggle;

    public void OnFindNameSuccess(Action<GameObject> showHideToggle)
    {
        activeToggle = showHideToggle;
        inputField.onEndEdit.AddListener(Handle);
    }

    void Handle(string input)
    {
        if (driver.IsValidPropertyName(input, out int index))
        {
            onFoundAssetByFullName.Invoke(index);
            Show(buildOptionPanelObj);
            Show(pricesSwitch);
        }
        else if (driver.HasExistedInPropertiesNames(input, out List<int> indices))
        {
            Hide(buildOptionPanelObj);
            onFoundAListOfPropertiesByKeyword.Invoke(indices);
            Show(pricesSwitch);
        }
        else
        {
            HideAll();
        }
    }

    void Show(GameObject gObj)
    {
        if (!gObj.activeSelf)
        {
            activeToggle.Invoke(gObj);
        }
    }

    void HideAll()
    {
        Hide(buildOptionPanelObj);
        Hide(pricesSwitch);
        Hide(pricesDisplay);
    }
    void Hide(GameObject gObj)
    {
        if (gObj.activeSelf)
        {
            activeToggle.Invoke(gObj);
        }
    }

    void OnDisable()
    {
        HideAll();
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }
}
