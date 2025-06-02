using System;
using TMPro;
using UnityEngine;

public class PropertyNameHandler : MonoBehaviour, INeedDriver
{
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField]
    GameObject buildOptionPanelObj;

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
        }
    }

    void OnDisable()
    {
        buildOptionPanelObj.SetActive(false);
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }
}
