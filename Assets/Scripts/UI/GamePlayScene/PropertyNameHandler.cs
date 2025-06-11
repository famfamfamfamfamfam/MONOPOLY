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
            Show(buildOptionPanelObj);
            Show(pricesPresenter);
        }
        else if (driver.HasExistedInPropertiesNames(input, out List<int> indices))
        {
            Show(pricesPresenter);
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
        if (pricesPresenter.activeSelf)
        {
            activeToggle.Invoke(pricesPresenter);
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
