using UnityEngine;
using UnityEngine.UI;

public class ShowToggle : MonoBehaviour
{
    [SerializeField]
    ShowButton[] showButtons;

    void Start()
    {
        for (int i = 0; i < showButtons.Length; i++)
        {
            showButtons[i].OnInteract(ShowHideToggle);
        }
    }

    public void ShowHideToggle(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

}
