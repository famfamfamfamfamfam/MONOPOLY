using System;
using TMPro;
using UnityEngine;

public class PropertyNameHandler : MonoBehaviour, INeedDriver
{
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField]
    GameObject buildOptionPanel;

    public Driver driver { get; set; }

    Action<GameObject> activeToggle;

    public void OnFindNameSuccess(Action<GameObject> showHideToggle)
    {
        activeToggle = showHideToggle;
        inputField.onEndEdit.AddListener(Handler);
    }

    void Handler(string input)
    {
        if (driver.IsValidPropertyName(input))
        {
            activeToggle.Invoke(buildOptionPanel);
        }
    }

    void OnDisable()
    {
        buildOptionPanel.SetActive(false);
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }
}
