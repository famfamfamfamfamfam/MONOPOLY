using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI mesh;
    public void UpdateTheText(string newText)
    {
        mesh.text = newText;
    }
}
