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
    GameObject pricesPresenter;

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
            activeToggle.Invoke(buildOptionPanelObj);
            activeToggle.Invoke(pricesPresenter);
        }
        else if (driver.HasExistedInPropertiesNames(input, out List<int> indices))
        {
            
        }
        else
        {
            buildOptionPanelObj.SetActive(false);
            pricesPresenter.SetActive(false);
        }
    }

    void OnDisable()
    {
        buildOptionPanelObj.SetActive(false);
        pricesPresenter.SetActive(false);
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }
}
