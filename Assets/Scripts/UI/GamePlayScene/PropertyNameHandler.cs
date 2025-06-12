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

    public Action<List<int>> onFindAListOfPropertiesByKeyword { get; set; }

    public Driver driver { get; set; }

    Action<GameObject> activeToggle;

    public void OnFindNameSuccess(Action<GameObject> showHideToggle)
    {
        activeToggle = showHideToggle;
        inputField.onEndEdit.AddListener(Handle);
    }

    void Handle(string input)
    {
        if (driver.IsValidPropertyName(input))
        {
            Show(buildOptionPanelObj);
            Show(pricesSwitch);
        }
        else if (driver.HasExistedInPropertiesNames(input, out List<int> indices))
        {
            onFindAListOfPropertiesByKeyword.Invoke(indices);
            Show(pricesSwitch);
            Debug.LogWarning(indices.Count);
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
        if (buildOptionPanelObj.activeSelf)
        {
            activeToggle.Invoke(buildOptionPanelObj);
        }
        if (pricesSwitch.activeSelf)
        {
            activeToggle.Invoke(pricesSwitch);
        }
        if (pricesDisplay.activeSelf)
        {
            activeToggle.Invoke(pricesDisplay);
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
